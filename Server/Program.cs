using System;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var server = new HttpServer("127.0.0.1", "8080"))
            {

                server.RequestLog += (s, e) => { Console.WriteLine($"{e.Message} for {e.Code}"); };

                server.ResponseLog += (s, e) =>
                {
                    Console.WriteLine(e.Message);

                    if (e.Code != "")
                        Console.WriteLine(e.Code);
                };

                server.CyclingListen();

                Thread.Sleep(10);
            }

            Console.ReadKey();
        }
    }
}
