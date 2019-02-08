using System;
using System.Collections.Generic;
using System.Linq;

using CourseWorkBCT.SupportClass;

namespace CourseWorkBCT.BlocksDS
{
    public class ChannelEncoder
    {
        public List<string> Message { get; private set; } = new List<string>();
        public Dictionary<string, List<string>> CodesHemming { get; private set; }
        public Dictionary<string, double> Parameters { get; private set; }

        public double CodeRedundancy { get; private set; }
        public double CodeSpeed { get; private set; }
        public double CodeDistance { get; private set; }
        public double CodeCorrectingAbility { get; private set; }
        public double AvegareCountEncoderBits { get; private set; }
        public double AverageSpeedBits { get; private set; }

        private int countTestBits = 3;
        private int countInformationBits = 4;
        private int countHemmingCodeBits = 7;

        public ChannelEncoder(SourceCoder sourceCoder)
        {
            CreatingCodesHemming();
            HemmingCodingMessage(string.Join("", sourceCoder.Message));
            Initialization(sourceCoder);
        }

        public double CalculationCodeRedundancy(double countTestBits, double countHemmingCodeBits)
        {
            return (countTestBits / countHemmingCodeBits);
        }

        public double CalculationCodeSpeed(double countInformationBits, double countHemmingCodeBits)
        {
            return (countInformationBits / countHemmingCodeBits);
        }

        public double CalculationCodeDistance(List<string> message)
        {
            double minWeight = 0;
            
            foreach(string codeHemming in message)
            {
                double weightBlock = codeHemming.Count(num => num == '1');

                if (weightBlock > minWeight && weightBlock != 0)
                {
                    minWeight = weightBlock;
                }
            }
           
            return minWeight;
        }

        public double CalculationCorrectingCodeAbility(double codeDistance)
        {
            return (codeDistance - 1) / 2; 
        }

        public double CalculationAverageCountEncoderBits(double countInformationBits, double codeSpeed)
        {
            return countInformationBits / codeSpeed;
        }

        public double CalculationAverageSpeedCode(double averageSpeedSourceCoder, double codeSpeed)
        {
            return (averageSpeedSourceCoder / codeSpeed);
        }

        private void Initialization(SourceCoder sourceCoder)
        {
            CodeRedundancy = CalculationCodeRedundancy(countTestBits, countHemmingCodeBits);
            CodeSpeed = CalculationCodeSpeed(countInformationBits, countHemmingCodeBits);
            CodeDistance = CalculationCodeDistance(Message);
            CodeCorrectingAbility = CalculationCorrectingCodeAbility(CodeDistance);
            AvegareCountEncoderBits = CalculationAverageCountEncoderBits(countInformationBits, CodeSpeed);
            AverageSpeedBits = CalculationAverageSpeedCode(sourceCoder.AverageSpeed, CodeSpeed);
            
        }

        private void HemmingCodingMessage(string messageSourceCoder)
        {
            messageSourceCoder = PadRightMessageBeforeDividingFour(messageSourceCoder);

            for (int i = 0;i < messageSourceCoder.Length;i+=countInformationBits)
            {
                Message.Add(CodesHemming[messageSourceCoder.Substring(i, countInformationBits)][1]);
            }
        }

        private string PadRightMessageBeforeDividingFour(string message)
        {
            while (true)
            {
                if (message.Length % countInformationBits == 0)
                {
                    break;
                }
                message += '0';
            }

            return message;
        }

        private void CreatingCodesHemming()
        {
            CodesHemming = new Dictionary<string, List<string>>();

            string informationBits;
            string testBits;
            string codeHemming;

            for (int i = 0;i < 16; i++)
            {
                informationBits = Convert.ToString(i, 2).PadLeft(countInformationBits, '0');

                testBits = CreatingTestBits(informationBits);
                codeHemming = informationBits + testBits;
                CodesHemming.Add(
                    informationBits,
                    new List<string>() {testBits,codeHemming,Convert.ToString(codeHemming.Count(num => num == '1'))}
                    );
            }
        }

        private string CreatingTestBits(string informationBits)
        {
            string testBits;

            testBits = XORBits.XOR(informationBits[0], informationBits[1], informationBits[2]);
            testBits += XORBits.XOR(informationBits[1], informationBits[2], informationBits[3]);
            testBits += XORBits.XOR(informationBits[0], informationBits[2], informationBits[3]);

            return testBits;
        }
    }
}
