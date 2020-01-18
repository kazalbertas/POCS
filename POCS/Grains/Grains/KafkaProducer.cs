using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grains.Grains
{
    public class KafkaProducer
    {
        public async Task SendToKafka(string payload, string topic) 
        {
            Message msg = new Message(payload);
            Uri uri = new Uri("http://localhost:9092");
            var options = new KafkaOptions(uri);
            var router = new BrokerRouter(options);
            var client = new Producer(router);
            var response = await client.SendMessageAsync(topic, new List<Message> { msg });
            client.Dispose();
            //Console.WriteLine(response[0].Error);
        }
    }
}
