using CourseWorkBCT.EconomicalCodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseWorkBCT.BlocksDS
{
    public class SourceCoder
    {
        public string[][] ParametersEconomicalCodes { get; private set; }
        public Dictionary<string,string> EconomicalCodes { get; private set; }
        public MessageSource messageSource { get; private set; }
        public List<string> Message { get; private set; }

        public double LimitShennona { get; private set; }
        public double AverageCountBinaryCharacters { get; private set; }
        public double AverageSpeed { get; private set; }
        public double ProbabilityZero { get; private set; }
        public double ProbabilityOne { get; private set; }
        public double Entropy { get; set; }
        public double Efficiency { get; set; }

        public SourceCoder()
        {

        }

        public SourceCoder(MessageSource messageSource)
        {
            this.messageSource = messageSource;
            ChoiseEconomicalCode(messageSource.VariationCourseWork, messageSource.ProbabilitiesCharacters);
            InitializationParametersEconomicalCodes(messageSource.ProbabilitiesCharacters);

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

        public double CalculationLimitShennona(double entropyMessageSource)
        {
            return entropyMessageSource / Math.Log(2, 2);
        }

        public double CalculationAverageCountBinaryCharacters(int baseAlphabet)
        {
            double averageCountBinaryCharacters = 0;
            for (int i = 0; i < baseAlphabet; i++)
            {              
                averageCountBinaryCharacters += 
                    Convert.ToDouble(ParametersEconomicalCodes[i][3]) * Convert.ToDouble(ParametersEconomicalCodes[i][1]);
            }
            return averageCountBinaryCharacters;
        }

        public double CalculationAverageSpeed(double speedMessageSource, double averageCountBinaryCharacter)
        {
            return speedMessageSource * averageCountBinaryCharacter;
        }

        public double CalculationProbabilityZero(int baseAlphabet)
        {
            double probabilityZero = 0;
            for (int i = 0;i < baseAlphabet; i++)
            {
                probabilityZero += 
                    Convert.ToDouble(ParametersEconomicalCodes[i][1]) * Convert.ToDouble(ParametersEconomicalCodes[i][4]);
            }
            return probabilityZero;
        }

        public double CalculationProbabilityOne(int baseAlphabet)
        {
            double probabilityOne = 0;
            for (int i=0;i < baseAlphabet; i++)
            {
                probabilityOne += 
                    Convert.ToDouble(ParametersEconomicalCodes[i][1]) * Convert.ToDouble(ParametersEconomicalCodes[i][5]);
            }
            return probabilityOne;
        }

        public double CalculationEntropy(double probabilityZero, double probabilityOne)
        {
            double a = probabilityZero * Math.Log(probabilityZero, 2);
            double b = probabilityOne * Math.Log(probabilityOne, 2);

            return - a - b;
        }

        public double CalculationEfficiency(double entropy)
        {
            return 1 - (entropy / Math.Log(2, 2));
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
            LimitShennona = CalculationLimitShennona(messageSource.Entropy);
            AverageCountBinaryCharacters = CalculationAverageCountBinaryCharacters(messageSource.BaseAlphabet);
            AverageSpeed = CalculationAverageSpeed(messageSource.Speed, AverageCountBinaryCharacters);
            
            ProbabilityZero = CalculationProbabilityZero(messageSource.BaseAlphabet);
            ProbabilityOne = CalculationProbabilityOne(messageSource.BaseAlphabet);
            Entropy = CalculationEntropy(ProbabilityZero, ProbabilityOne);
            Efficiency = CalculationEfficiency(Entropy);

            Message = GenerationMessage(messageSource.Message);
        }
    }
}
