using System;

namespace Data
{
    public class Input
    {
        public int K;
        public decimal[] Sums = { };
        public int[] Muls = { };

        public Input()
        {
            var random = new Random();

            K = random.Next();

            Sums = new decimal[random.Next(1, 10)];

            for (var i = 0; i < Sums.Length; i++)
            {
                Sums[i] = (decimal)random.NextDouble() * 100;
            }

            Muls = new int[random.Next(1, 10)];

            for (var i = 0; i < Muls.Length; i++)
            {
                Muls[i] = random.Next();
            }


        }
    }
}
