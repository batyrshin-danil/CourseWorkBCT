using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseWorkBCT.BlocksDS;

namespace CourseWorkOTS
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageSource messageSource = new MessageSource(new int[4] { 6, 3, 0, 5 });
            CodeHaffman codeHaffman = new CodeHaffman(messageSource.probalitiesSymbol);

            foreach (string key in codeHaffman.getTableCodes().Keys)
            {
                Console.WriteLine(key + " : " + codeHaffman.getTableCodes()[key]);
            }

            CodeShennonaFano codeShennonaFano = new CodeShennonaFano(messageSource.probalitiesSymbol);

            Console.ReadLine();
        }
    }
}
