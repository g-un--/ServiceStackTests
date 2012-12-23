using Messages;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceClient = new JsonRestClientAsync("http://g-un--:1337/");
            var responseSubject = new BehaviorSubject<Unit>(Unit.Default);
            
            responseSubject.Subscribe(_ =>
                {
                    serviceClient.PostAsync<ResourceResponse>(
                        "/async",
                        new ResourceRequest()
                        {
                            ResourceKey = Guid.NewGuid().ToString()
                        },
                        response =>
                        {
                            Console.WriteLine("Data received");
                            Console.WriteLine(response.ResourceData);
                            responseSubject.OnNext(Unit.Default);
                        },
                        (response, ex) =>
                        {
                            Console.WriteLine("Exception on calling service post method");
                            Console.WriteLine(ex);
                            responseSubject.OnCompleted();
                        }); 
                });

            Observable.Interval(TimeSpan.FromSeconds(1))
                .Subscribe(_ =>
                    {
                        var newClient = new JsonRestClientAsync("http://g-un--:1337/");
                        newClient.PutAsync<object>(
                            "/async",
                            new ResourceRequest() { ResourceKey = Guid.NewGuid().ToString() },
                            response =>
                            {
                                Console.WriteLine("Put was sent!");
                            },
                            (response, exception) =>
                            {
                                Console.WriteLine(exception);
                            });
                    });

            Console.ReadLine();
        }
    }
}
