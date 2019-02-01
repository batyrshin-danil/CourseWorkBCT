using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkBCT.SupportClass
{
    public static class XORBits
    {
        public static string XOR(char a, char b, char c)
        {
            return Convert.ToString(
                Convert.ToInt32(a.ToString(), 2) ^
                Convert.ToInt32(b.ToString(), 2) ^
                Convert.ToInt32(c.ToString(), 2));
        }

        public static string XOR(char a, char b, char c, char d)
        {
            return Convert.ToString(
                Convert.ToInt32(a.ToString(), 2) ^
                Convert.ToInt32(b.ToString(), 2) ^
                Convert.ToInt32(c.ToString(), 2) ^
                Convert.ToInt32(d.ToString(), 2));
        }
    }
}
