using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace RedisPubSub
{
    class Program
    {
        private static ConnectionMultiplexer _redis;
        private static Subscriber[] sub;

        static void Main(string[] args)
        {
            _redis = ConnectionMultiplexer.Connect("localhost"); // Connect to localhost on default port
            sub = new Subscriber[2];
            for (int i = 0; i < 2; ++i)
            {
                sub[i] = new Subscriber(_redis.GetSubscriber(), "Subscriber"+i);

            }
            sub[0].Subscribe("Mars");
            sub[1].Subscribe("Phobos");
            sub[0].Subscribe("Cosmos");
            sub[1].Subscribe("Cosmos");

            Console.WriteLine("Started listening to messagef from Mars, Phobos and Cosmos...");
            while (true)
            {
                

                if (Console.ReadKey(true).Key == ConsoleKey.Home)
                        break;
            }

            Console.WriteLine("Goodbye galaxy...");

        }

    }
}
