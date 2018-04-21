using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppSample.PubSubPattern
{
    public class PubSubProg
    {
        public void Main()
        {

            Publisher dogPublisher = new Publisher();
            Publisher catPublisher = new Publisher();

            Subscriber AnimalLover = new Subscriber();
            Subscriber OldCatLady = new Subscriber();

            PubSubServer server = new PubSubServer();

            Message dogMsg = new Message { topic = "Dogs", payLoad = "Dogs are men's best friend." };
            Message catMeg = new Message { topic = "Cats", payLoad = "Cats can take care of themselves." };


            dogPublisher.Send(dogMsg, server);
            catPublisher.Send(catMeg, server);

            AnimalLover.Listen("Dogs", 0);
            AnimalLover.Listen("Cats", 1);

            OldCatLady.Listen("Cats", 0);

            server.subscriber[0] = AnimalLover;
            server.subscriber[1] = OldCatLady;
            server.Forward();

            Console.WriteLine("Animallover is subscribed to the following message.");
            AnimalLover.Print();

            Console.WriteLine("OldCatLady is subscribed to the following message.");
            OldCatLady.Print();

        }

    }
}
