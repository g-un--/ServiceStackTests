using Messages;
using ServiceStack.Plugins.Tasks;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStackAsync
{
    class ServiceHost : AppHostHttpListenerBase
    {
        public ServiceHost() : base("StarterTemplate HttpListener", typeof(ResourcesService).Assembly) { }

        public override void Configure(Funq.Container container)
        {
            this.LoadPlugin(new TaskSupport());
            Routes
                .Add<ResourceRequest>("/async");
        }
    }
}
