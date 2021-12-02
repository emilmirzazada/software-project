using System;
using System.Collections.Generic;
using System.Text;

namespace IMSProject
{
    class GenerateRandom
    {
        public static int random()
        {
            Random random = new Random();
            int low = 15; // from one hour
            int high = 18; // to two hours
            return random.Next(high - low) + low;
        }
    }
}
