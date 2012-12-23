using ServiceStack.ServiceInterface;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcesService
{
    class Program
    {
        public class Hello
        {
            public string Name { get; set; }
        }

        public class HelloResponse
        {
            public string Result { get; set; }
        }

        public class HelloService : Service
        {
            public object Any(Hello request)
            {
                return new HelloResponse { Result = "Hello, " + request.Name };
            }
        }

        static void Main(string[] args)
        {
            var listeningOn = args.Length == 0 ? "http://*:1337/" : args[0];
            var appHost = new AppHost();
            appHost.Init();
            appHost.Start(listeningOn);

            Console.WriteLine("AppHost Created at {0}, listening on {1}", DateTime.Now, listeningOn);
            Console.ReadKey();
        }

        class AppHost : AppHostHttpListenerBase
        {
            public AppHost() : base("StarterTemplate HttpListener", typeof(HelloService).Assembly) { }

            public override void Configure(Funq.Container container)
            {
                Routes
                    .Add<Hello>("/hello")
                    .Add<Hello>("/hello/{Name}");
            }
        }
    }
}
