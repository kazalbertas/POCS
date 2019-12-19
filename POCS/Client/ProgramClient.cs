using Configuration;
using Grains.Grains.TimeActor;
using Grains.Grains.Vessels;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class ProgramClient
    {
        static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                using (var client = await ConnectClient())
                {
                    await DoClientWork(client);
                    Console.ReadKey();
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nException while trying to run client: {e.Message}");
                Console.WriteLine("Make sure the silo the client is trying to connect to is running.");
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
                return 1;
            }
        }


        private static async Task<IClusterClient> ConnectClient()
        {

            IClusterClient client;
            client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect();
            Console.WriteLine("Client successfully connected to silo host \n");
            return client;
        }

        private static async Task DoClientWork(IClusterClient client)
        {
            var c = client.GetGrain<ITimeGrain>(Constants.TimeActorId);

            await c.InitialiseSimulation(DateTime.Now);
            // example of calling grains from the initialized client
            for (var i = 0; i < 10; i++)
            {
                
                var response = await c.NextTick();

                Console.WriteLine("\n\n{0}\n\n", response);
                Thread.Sleep(5000);
            }

        }
    }
}
