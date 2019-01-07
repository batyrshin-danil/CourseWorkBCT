using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseWorkBCT.BlocksDS;
using CourseWorkBCT.BudgetCodes;

namespace CourseWorkOTS
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageSource messageSource = new MessageSource(new int[4] { 6, 3, 0, 5 });
            SourceCoder sourceCoder = new SourceCoder(messageSource);

            foreach(string symbol in messageSource.Message)
            {
                Console.WriteLine(symbol);
            }

            int a = 3;

            Console.WriteLine(Convert.ToString(a, 2));
            Console.WriteLine(Convert.ToString(a, 16));

            Console.ReadLine();
        }
    }
}
