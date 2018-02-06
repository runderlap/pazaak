using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Euler.Tools
{
    public static class RandomTool
    {
        private static readonly RandomNumberGenerator generator;

        static RandomTool()
        {
            generator = RandomNumberGenerator.Create();
        }

        private static int GetNext(bool MustBePositive = false)
        {
            byte[] rndArray = new byte[4];
            generator.GetBytes(rndArray);
            var result = BitConverter.ToInt32(rndArray, 0);
            return (MustBePositive && result<0) ? result*-1 : result;
        }
        public static int Get(int from, int to, bool excludeZero=true)
        {
            var result = -1;
            var offsetWithNegativeFrom = from < 0 ? from * -1 : 0;
            var ceiling = to + offsetWithNegativeFrom +2;
            while ((result-offsetWithNegativeFrom-1 == 0&&excludeZero) || result < 1 || result >= ceiling+1)
            {
                var rand = GetNext(true);
                result = rand % ceiling;
            }
            return result-offsetWithNegativeFrom-1;
        }
        public static bool Get()
        {
             return GetNext() %2 > 0;
        }
        public static void Run()
        {
            int from = -6;
            int to = 6;
            int iterations = 10000;

            //get us some random numbers
            var results = new List<int>();
            for (int i=0;i<iterations;i++)
            {
                results.Add(Get(from, to));
            }
            //check how its divided
            for (int i = from-1; i <= to+1; i++)
            {
                Console.WriteLine(value: $"Number {i}: {results.Count(n => n==i)}");
            }
        }
    }
}
