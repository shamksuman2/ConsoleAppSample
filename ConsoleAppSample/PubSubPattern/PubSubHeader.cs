using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppSample.PubSubPattern
{
    public class PubSubServer
    {
        public PubSubServer()
        {

        }

        public Queue<Message> buffer = new Queue<Message>();
        public Subscriber[] subscriber = new Subscriber[2];

        public void Forward()
        {
            while (buffer.Count != 0)
            {
                Message message = buffer.Dequeue();
                for(int i =0; i< subscriber.Length;i++)
                {
                    for(int j=0;j< subscriber[i].topics.Length; j++)
                    {
                        if(message.topic == subscriber[i].topics[j])
                        {
                            subscriber[i].messages.Enqueue(message);
                        }
                    }
                }
            }
        }
    } 

    public class Message
    {
        public Message()
        {

        }

        public string payLoad;
        public string topic;

    }
    public class Publisher
    {
        public Publisher()
        {

        }
        public void Send(Message msg, PubSubServer server)
        {
            server.buffer.Enqueue(msg);
        }
    }

    public class Subscriber
    {
        public string[] topics = new string[2];
        public Queue<Message> messages = new Queue<Message>();
        public void Listen(string topic, int index)
        {
            topics[index] = topic;
        }

        public void Print()
        {
            for(int i=0; i < topics.Length; i++)
            {
                while (messages.Count !=0)
                {
                    Message msg = messages.Dequeue();
                    Console.WriteLine($"Topic : {msg.topic}, PayLoad:{msg.payLoad}");
                }
            }
        }
    }
}
