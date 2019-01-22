using System;
using System.Collections.Generic;
using System.Linq;

using CourseWorkBCT.EconomicalCodes;

namespace CourseWorkBCT.BlocksDS
{
    public class SourceCoder
    {
        public string[][] ParametersEconomicalCodes { get; private set; }
        public Dictionary<string,string> EconomicalCodes { get; private set; }
        public Dictionary<string,double> Parameters { get; private set; }
        public MessageSource messageSource { get; private set; }

        public List<string> Message { get; private set; }

        private Dictionary<string,double> parametersMessageSource;
        

        public SourceCoder(MessageSource messageSource)
        {
            this.messageSource = messageSource;
            parametersMessageSource = messageSource.Parameters;
            ChoiseEconomicalCode(messageSource.variationCourseWork, messageSource.ProbalitiesSymbol);
            InitializationParametersEconomicalCodes(messageSource.ProbalitiesSymbol);

            Initialization();
        }

        public void InitializationParametersEconomicalCodes(Dictionary<string, double> probalitiesSymbol)
        {
            /* Инициализируем массив("таблицу") с количеством строк(одномерных массивов строк) 
             * равным количеству символов в переданном словаре. */
            ParametersEconomicalCodes = new string[probalitiesSymbol.Count][];
            List<string> symbolsAlphabet = probalitiesSymbol.Keys.ToList();

            for (int i = 0; i < symbolsAlphabet.Count; i++)
            {
                ParametersEconomicalCodes[i] = new string[]
                {
                    symbolsAlphabet[i],
                    probalitiesSymbol[symbolsAlphabet[i]].ToString(),
                    EconomicalCodes[symbolsAlphabet[i]],
                    EconomicalCodes[symbolsAlphabet[i]].Length.ToString(),
                    EconomicalCodes[symbolsAlphabet[i]].Count(sym => sym == '0').ToString(),
                    EconomicalCodes[symbolsAlphabet[i]].Count(sym => sym == '1').ToString()
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
                K += Convert.ToDouble(ParametersEconomicalCodes[i][3]) * Convert.ToDouble(ParametersEconomicalCodes[i][1]);
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
                pZero += Convert.ToDouble(ParametersEconomicalCodes[i][1]) * Convert.ToDouble(ParametersEconomicalCodes[i][4]);
            }
            return pZero;
        }

        public double POne(int baseAlphabet)
        {
            double pOne = 0;
            for (int i=0;i < baseAlphabet; i++)
            {
                pOne += Convert.ToDouble(ParametersEconomicalCodes[i][1]) * Convert.ToDouble(ParametersEconomicalCodes[i][5]);
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

        public List<string> GenerationMessage(List<string> messageMessageSource)
        {
            List<string> message = new List<string>();
            foreach(string symbol in messageMessageSource)
            {
                message.Add(EconomicalCodes[symbol]);
            }
            return message;
        }


        private void ChoiseEconomicalCode(int[] variationCourseWork, Dictionary<string,double> probalitiesSymbol)
        {
            int choisePointer = (variationCourseWork[1] + variationCourseWork[3]) % 2;
            EconomicalCode economicalCode;

            if (choisePointer == 0)
            { 
                economicalCode = new CodeHaffman(probalitiesSymbol);
            }
            else
            {
                economicalCode = new CodeShennonaFano(probalitiesSymbol);
            }

            EconomicalCodes = economicalCode.economicalCodes;
        }

        private void Initialization()
        {
            Parameters = new Dictionary<string, double>();

            Parameters.Add("KMIn", KMin(parametersMessageSource["H"]));
            Parameters.Add("K", K(messageSource.BaseAlphabet));
            Parameters.Add("VKi", VKi(parametersMessageSource["Vi"], Parameters["K"]));
            Parameters.Add("PZero", PZero(messageSource.BaseAlphabet));
            Parameters.Add("POne", POne(messageSource.BaseAlphabet));
            Parameters.Add("H", H(Parameters["PZero"], Parameters["POne"]));
            Parameters.Add("PKi", PKi(Parameters["H"]));

            Message = GenerationMessage(messageSource.Message);
        }
    }
}
