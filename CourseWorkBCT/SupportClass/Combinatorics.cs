using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkBCT.SupportClass
{
    public static class Combinatorics
    {
        public static double Combination(int n, int k)
        {
            return Factorial(n) / (Factorial(n - k) * Factorial(k));
        }

        private static double Factorial(int x)
        {
            int y = 1;

            for (int i = 0; i < x; x++)
            {
                y *= i;
            }

            return y;
        }
    }
}
