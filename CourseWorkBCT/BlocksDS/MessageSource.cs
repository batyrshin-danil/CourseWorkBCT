using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkBCT.BlocksDS
{
    // 
    // Класс описывающий блок "Источник сообщения" в CПДС.  
    //
    public class MessageSource
    {
        public int BaseAlphabet { get; private set; }
        public List<string> Message { get; private set; }
        public Dictionary<string,double> Parameters { get; private set; }
        public Dictionary<string, double> ProbSymbol{ get; private set; }
        public int[] Variable { get; private set; }

        public MessageSource(int [] variable)
        {
            Variable = variable;
            CreatingProbalityTable();
            StartBlock();
        }

        public MessageSource(Dictionary<string, double> probSymbol)
        {
            ProbSymbol = probSymbol;
            StartBlock();
        }

        public double Vi(int[] variable)
        {
            double vi = 0;
            for (int i = 0; i < 4; i++)
            {
                vi += variable[i];
            }
            return vi;
        }

        public double H(Dictionary<string, double> probSymbol)
        {
            double H = 0;
            foreach (string key in probSymbol.Keys)
            {
                H += probSymbol[key] * Math.Log(probSymbol[key], 2);
            }
            return H;
        }

        public double HMax(int BaseAlphabet)
        {
            return Math.Log(BaseAlphabet, 2);
        }

        public double Pi(double H, double HMax)
        {
            return 1 - (H / HMax);
        }

        public double HHatch(double Vi, double H)
        {
            return Vi * H;
        }
        //
        // Следующий метод генерирует сообщение из 10-ти случайно выбранных допустимых символов.
        public List<string> GenerationgMessage(List<string> alphabet)
        {
            List<string> messageLocal = new List<string>();
            List<string> listSymbol = alphabet;

            Random random = new Random();

            for (int i = 0; i < 10; i++)
            {
                messageLocal.Add(Convert.ToString(listSymbol[random.Next(listSymbol.Count())]));
            }
            return messageLocal;
        }
        //
        // Установка сообщения принудительно.
        public void SelectMessage(string message)
        {
            Message = message.Split().ToList();
        }

        //
        // Метод вычисления вероятностей появления символов.
        private void CreatingProbalityTable()
        {
            ProbSymbol = new Dictionary<string, double>()
            {
                {"а", 0.310 * (1.0 - 0.1 * Variable[3])},   {"б", 0.002 + (0.001 * Variable[0])},
                {"в", 0.007 + (0.025 * Variable[2])},       {"г", 0.009 * (1.0 - 0.1 * Variable[1])},
                {"д", 0.020 * (1 - 0.1 * Variable[3])},     {"е", 0.005 + (0.015 * Variable[1])},
                {"з", 0.010 * (1 - 0.1 * Variable[0])},     {"и", 0.15 * (1 - 0.1 * Variable[1])},
                {"к", 0.008 + (0.031 * Variable[3]) },      {"л", 0.2 * (1 - 0.1 * Variable[0])},
                {"м", 0.006 + (0.02 * Variable[0]) },       {"н",  0.015 * (1 - 0.1 * Variable[2])},
                {"о", 0.004 + (0.002 * Variable[3]) },      {"п", 0.003 + (0.0015 * Variable[2])},
                {"р", 0.250 * (1 - 0.1 * Variable[2])},     {"с", 0.001 + (0.0009 * Variable[1])}
            };
        }

        private void SettingBaseAlphabet()
        {
            BaseAlphabet = ProbSymbol.Count();
        }

        private void StartBlock()
        {
            Parameters = new Dictionary<string, double>();

            Parameters.Add("Vi", Vi(Variable));
            Parameters.Add("H", H(ProbSymbol));
            Parameters.Add("HMax", HMax(BaseAlphabet));
            Parameters.Add("Pi", Pi(Parameters["H"], Parameters["HMax"]));
            Parameters.Add("HHatch", HHatch(Parameters["Vi"], Parameters["H"]));

            Message = GenerationgMessage(ProbSymbol.Keys.ToList());
        }

    }
}
