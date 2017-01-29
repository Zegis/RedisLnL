using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace RedisPubSub
{
    class Subscriber
    {
        ISubscriber _subscriber;
        string _name;

        public Subscriber(ISubscriber sub, string name)
        {
            _subscriber = sub;
            _name = name;
        }

        public void Subscribe(string channelName)
        {
            _subscriber.Subscribe(channelName, (channel, message) => { Console.WriteLine(this._name + " recieved " + message + " from " + channel); });
        }
    }
}
