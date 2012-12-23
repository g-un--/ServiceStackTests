using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using Messages;
using ServiceStack.Plugins.Tasks;
using ServiceStack.ServiceInterface;
using System.Reactive;
using System.Reactive.Linq;

namespace ServiceStackAsync
{
    public class ResourcesService : AsyncRestServiceBase<ResourceRequest>
    {
        public override Task<object> OnPost(ResourceRequest request)
        {
            return CollectData(request);
        }

        public override Task<object> OnPut(ResourceRequest request)
        {
            return Task.Factory.StartNew<object>(() =>
            {
                MessageBus.Instance.OnNext(string.Format("Update at {0}", DateTime.UtcNow));
                return null;
            });
        }

        private async Task<object> CollectData(ResourceRequest request)
        {
            var dataBuilder = new StringBuilder();

            var collectTask = MessageBus.Instance.Bus
                                 .TakeUntil(DateTimeOffset.UtcNow.AddSeconds(10))
                                 .ForEachAsync(item => dataBuilder.AppendLine(item))
                                 .ContinueWith(task =>
                                 {
                                     var data = dataBuilder.ToString();

                                     return new ResourceResponse()
                                     {
                                         ResourceKey = request.ResourceKey,
                                         ResourceData = data
                                     };
                                 });

            var response = await collectTask;
            return response;
        }
    }
}
