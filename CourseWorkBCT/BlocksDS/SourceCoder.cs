using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseWorkBCT.BlocksDS;
using CourseWorkBCT.BudgetCodes;

namespace CourseWorkBCT.BlocksDS
{
    public class SourceCoder
    {
        public string[][] TableBudgetCode { get; private set; }
        public Dictionary<string,string> TableCodes { get; private set; }
        public Dictionary<string,double> Parameters { get; private set; }

        public List<string> Message { get; private set; }

        private Dictionary<string,double> parametersMS;
        private MessageSource messageSource;

        public SourceCoder(MessageSource messageSource)
        {
            this.messageSource = messageSource;
            parametersMS = messageSource.Parameters;
            ChoiseBudgetCode(messageSource.Variable, messageSource.ProbSymbol);
            CreatingTableBudgetCode(messageSource.ProbSymbol);
        }

        public void CreatingTableBudgetCode(Dictionary<string, double> probSymbol)
        {
            /* Инициализируем массив("таблицу") с количеством строк(одномерных массивов строк) 
             * равным количеству символов в переданном словаре. */
            TableBudgetCode = new string[probSymbol.Count][];
            List<string> keys = probSymbol.Keys.ToList();

            for (int i = 0; i < keys.Count; i++)
            {
                TableBudgetCode[i] = new string[]
                {
                    keys[i],
                    probSymbol[keys[i]].ToString(),
                    TableCodes[keys[i]],
                    TableCodes[keys[i]].Length.ToString(),
                    TableCodes[keys[i]].Count(sym => sym == '0').ToString(),
                    TableCodes[keys[i]].Count(sym => sym == '1').ToString()
                };
            }
        }

        public double KMin(double H)
        {
            return H / Math.Log(2, 2);
        }

        public double K(int baseAlphabet)
        {
            double K = 0;
            for (int i = 0; i < baseAlphabet; i++)
            {
                K += Convert.ToDouble(TableBudgetCode[i][3]) * Convert.ToDouble(TableBudgetCode[i][1]);
            }
            return K;
        }

        public double VKi(double Vi, double K)
        {
            return Vi * K;
        }

        public double PZero(int baseAlphabet)
        {
            double pZero = 0;
            for (int i = 0;i < baseAlphabet; i++)
            {
                pZero += Convert.ToDouble(TableBudgetCode[i][1]) * Convert.ToDouble(TableBudgetCode[i][4]);
            }
            return pZero;
        }

        public double POne(int baseAlphabet)
        {
            double pOne = 0;
            for (int i=0;i < baseAlphabet; i++)
            {
                pOne += Convert.ToDouble(TableBudgetCode[i][1]) * Convert.ToDouble(TableBudgetCode[i][5]);
            }
            return pOne;
        }

        public double H(double pZero, double pOne)
        {
            double a = pZero * Math.Log(pZero, 2);
            double b = pOne * Math.Log(pOne, 2);

            return - a - b;
        }

        public double PKi(double H)
        {
            return 1 - (H / Math.Log(2, 2));
        }

        public List<string> GenerationMessage(List<string> message)
        {
            List<string> messageLocal = new List<string>();
            foreach(string sym in message)
            {
                messageLocal.Add(TableCodes[sym]);
            }
            return messageLocal;
        }


        private void ChoiseBudgetCode(int[] vatiable, Dictionary<string,double> probSymbol)
        {
            int choisePointer = (vatiable[1] + vatiable[3]) % 2;
            if (choisePointer == 0)
            {
                CodeHaffman codeHaffman = new CodeHaffman(probSymbol);
                TableCodes = codeHaffman.tableCodes;
            }
            else
            {
                CodeShennonaFano codeShennonaFano = new CodeShennonaFano(probSymbol);
                TableCodes = codeShennonaFano.tableCodes;
            }
        }

        private void StartBlock()
        {
            Parameters = new Dictionary<string, double>();

            Parameters.Add("KMIn", KMin(parametersMS["H"]));
            Parameters.Add("K", K(messageSource.BaseAlphabet));
            Parameters.Add("VKi", VKi(parametersMS["Vi"], Parameters["K"]));
            Parameters.Add("PZero", PZero(messageSource.BaseAlphabet));
            Parameters.Add("POne", POne(messageSource.BaseAlphabet));
            Parameters.Add("H", H(Parameters["PZero"], Parameters["POne"]));
            Parameters.Add("PKi", PKi(Parameters["H"]));

            Message = GenerationMessage(messageSource.Message);
        }
    }
}
