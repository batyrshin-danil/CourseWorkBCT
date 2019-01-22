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
        public Dictionary<string, double> ProbalitiesSymbol{ get; private set; }
        public int[] variationCourseWork { get; private set; }

        public MessageSource(int [] variable)
        {
            variationCourseWork = variable;
            CreatingProbalityTable();
            Initialization();
        }

        public MessageSource(Dictionary<string, double> probSymbol)
        {
            ProbalitiesSymbol = probSymbol;
            Initialization();
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
            List<string> message = new List<string>();

            Random randomSymbol = new Random();

            for (int i = 0; i < 10; i++)
            {
                message.Add(alphabet[randomSymbol.Next(alphabet.Count())]);
            }
            return message;
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
            ProbalitiesSymbol = new Dictionary<string, double>()
            {
                {"а", 0.310 * (1.0 - 0.1 * variationCourseWork[3])},   {"б", 0.002 + (0.001 * variationCourseWork[0])},
                {"в", 0.007 + (0.025 * variationCourseWork[2])},       {"г", 0.009 * (1.0 - 0.1 * variationCourseWork[1])},
                {"д", 0.020 * (1 - 0.1 * variationCourseWork[3])},     {"е", 0.005 + (0.015 * variationCourseWork[1])},
                {"з", 0.010 * (1 - 0.1 * variationCourseWork[0])},     {"и", 0.15 * (1 - 0.1 * variationCourseWork[1])},
                {"к", 0.008 + (0.031 * variationCourseWork[3]) },      {"л", 0.2 * (1 - 0.1 * variationCourseWork[0])},
                {"м", 0.006 + (0.02 * variationCourseWork[0]) },       {"н",  0.015 * (1 - 0.1 * variationCourseWork[2])},
                {"о", 0.004 + (0.002 * variationCourseWork[3]) },      {"п", 0.003 + (0.0015 * variationCourseWork[2])},
                {"р", 0.250 * (1 - 0.1 * variationCourseWork[2])},     {"с", 0.001 + (0.0009 * variationCourseWork[1])}
            };
        }

        private void SettingBaseAlphabet()
        {
            BaseAlphabet = ProbalitiesSymbol.Count();
        }

        private void Initialization()
        {
            Parameters = new Dictionary<string, double>();

            Parameters.Add("Vi", Vi(variationCourseWork));
            Parameters.Add("H", H(ProbalitiesSymbol));
            Parameters.Add("HMax", HMax(BaseAlphabet));
            Parameters.Add("Pi", Pi(Parameters["H"], Parameters["HMax"]));
            Parameters.Add("HHatch", HHatch(Parameters["Vi"], Parameters["H"]));

            Message = GenerationgMessage(ProbalitiesSymbol.Keys.ToList());
        }

    }
}
