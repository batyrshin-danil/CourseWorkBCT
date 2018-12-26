using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseWorkOTS.BlocksDS;

namespace CourseWorkOTS
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageSource messageSource = new MessageSource(new int[4] { 6, 3, 0, 5 });
            CodeHaffman codeHaffman = new CodeHaffman(messageSource.getProbalitiesSymbol());

            foreach (string key in codeHaffman.getTableCodes().Keys)
            {
                Console.WriteLine(key + " : " + codeHaffman.getTableCodes()[key]);
            }

            Console.ReadLine();
        }
    }
}
