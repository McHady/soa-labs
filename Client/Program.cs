using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Serialize;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client("127.0.0.1", "8080");

            client.RequestLogger += (s, e) =>
            {
                Console.WriteLine(e.Response);
            };
            
            client.ResponseLogger += (s, e) =>
            {
                Console.WriteLine(e.Response);
            };

            if (client.Ping())
            {
                Console.WriteLine("Here we go");
                Thread.Sleep(3000);

                Input input = (Input)new Serialization<Input>("Json").Deserialize(client.GetInputData());

                Thread.Sleep(3000);

                var tmp = new List<decimal>();
                
                tmp.AddRange(input.Sums);

                foreach (var m in input.Muls)
                {
                    tmp.Add(m);
                }

                Output output = new Output()
                {
                    SumResult = input.Sums.Sum() * input.K,
                    MulResult = input.Muls.Aggregate((x, y) => x * y),
                    SortedInputs = (from i in tmp
                        orderby i
                        select i).ToArray()
                };

                client.WriteAnswer(new Serialization<Input>("Json").Serialize(output));

            }
            else
            {
                Console.WriteLine("Nope");
            }
                

            Console.ReadKey();
        }
    }
}
