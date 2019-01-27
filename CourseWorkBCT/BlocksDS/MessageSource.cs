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
        public Dictionary<string, double> ProbabilitiesCharacters{ get; private set; }
        public int[] VariationCourseWork { get; private set; }

        public double Speed { get; private set; }
        public double Entropy { get; private set; }
        public double EntropyMax { get; private set; }
        public double Redundancy { get; private set; }
        public double Efficiency { get; private set; }

        public MessageSource() {

        }

        public MessageSource(int [] variation)
        {
            VariationCourseWork = variation;
            CreatingProbalityTable();
            Initialization();
        }

        public MessageSource(Dictionary<string, double> probabilitiesCharacters)
        {
            ProbabilitiesCharacters = probabilitiesCharacters;
            Initialization();
        }

        public double CalcuationSpeed(int[] variation)
        {
            double vi = 0;
            for (int i = 0; i < 4; i++)
            {
                vi += variation[i];
            }
            return vi;
        }

        public double CalculationEntropy(Dictionary<string, double> probabilitiesCharacters)
        {
            double H = 0;
            foreach (string key in probabilitiesCharacters.Keys)
            {
                H += probabilitiesCharacters[key] * Math.Log(probabilitiesCharacters[key], 2);
            }
            return H;
        }

        public double CalculationEntropyMax(int BaseAlphabet)
        {
            return Math.Log(BaseAlphabet, 2);
        }

        public double CalculationEfficiency(double entropy, double entropyMax)
        {
            return 1 - (entropy / entropyMax);
        }

        public double CalculationRedundancy(double speed, double entropy)
        {
            return speed * entropy;
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
            ProbabilitiesCharacters = new Dictionary<string, double>()
            {
                {"а", 0.310 * (1.0 - 0.1 * VariationCourseWork[3])},   {"б", 0.002 + (0.001 * VariationCourseWork[0])},
                {"в", 0.007 + (0.025 * VariationCourseWork[2])},       {"г", 0.009 * (1.0 - 0.1 * VariationCourseWork[1])},
                {"д", 0.020 * (1 - 0.1 * VariationCourseWork[3])},     {"е", 0.005 + (0.015 * VariationCourseWork[1])},
                {"з", 0.010 * (1 - 0.1 * VariationCourseWork[0])},     {"и", 0.15 * (1 - 0.1 * VariationCourseWork[1])},
                {"к", 0.008 + (0.031 * VariationCourseWork[3]) },      {"л", 0.2 * (1 - 0.1 * VariationCourseWork[0])},
                {"м", 0.006 + (0.02 * VariationCourseWork[0]) },       {"н", 0.015 * (1 - 0.1 * VariationCourseWork[2])},
                {"о", 0.004 + (0.002 * VariationCourseWork[3]) },      {"п", 0.003 + (0.0015 * VariationCourseWork[2])},
                {"р", 0.250 * (1 - 0.1 * VariationCourseWork[2])},     {"с", 0.001 + (0.0009 * VariationCourseWork[1])}
            };
        }

        private void SettingBaseAlphabet()
        {
            BaseAlphabet = ProbabilitiesCharacters.Count();
        }

        private void Initialization()
        {
            Speed = CalcuationSpeed(VariationCourseWork);
            Entropy = CalculationEntropy(ProbabilitiesCharacters);
            EntropyMax = CalculationEntropyMax(BaseAlphabet);
            Redundancy = CalculationRedundancy(Speed, Entropy);
            Efficiency = CalculationEfficiency(Entropy, EntropyMax);

            Message = GenerationgMessage(ProbabilitiesCharacters.Keys.ToList());
        }

    }
}
