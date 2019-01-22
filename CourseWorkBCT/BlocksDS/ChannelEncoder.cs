using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseWorkBCT.BlocksDS
{
    public class ChannelEncoder
    {
        public List<string> Message { get; private set; } = new List<string>();
        public Dictionary<string, List<string>> CodesHemming { get; private set; }
        public Dictionary<string, double> Parameters { get; private set; }

        public ChannelEncoder(SourceCoder sourceCoder)
        {
            CreatingCodesHemming();
            HemmingCodingMessage(string.Join("", sourceCoder.Message));
            Initialization(sourceCoder.Parameters);
        }

        public double PKk(int r, int n)
        {
            return (r / n);
        }

        public double R(int k, int n)
        {
            return (k / n);
        }

        public double DMin(List<string> message)
        {
            double minWeight = 0;
            
            foreach(string block in message)
            {
                double weightBlock = block.Count(num => num == '1');

                if (weightBlock > minWeight && weightBlock != 0)
                {
                    minWeight = weightBlock;
                }
            }

            return minWeight;
        }

        public double Qi(double dMin)
        {
            return ((dMin - 1) / 2); 
        }

        public double N(double k, double R)
        {
            return k / R;
        }

        public double VKk(double VKi, double R)
        {
            return (VKi / R);
        }

        private void Initialization(Dictionary<string, double> parametersSourceCoder)
        {
            Parameters = new Dictionary<string, double>();

            Parameters.Add("PKk", PKk(3, 7));
            Parameters.Add("R", R(4, 7));
            Parameters.Add("DMin", DMin(Message));
            Parameters.Add("Qi", Qi(Parameters["DMin"]));
            Parameters.Add("N", N(4, Parameters["R"]));
            Parameters.Add("VKk", VKk(parametersSourceCoder["VKi"], Parameters["R"]));
        }

        private void HemmingCodingMessage(string messageSourceCoder)
        {
            while (true)
            {
                if (messageSourceCoder.Length % 4 == 0)
                {
                    break;
                }
                messageSourceCoder += '0';
            }
            for (int i = 0;i < messageSourceCoder.Length;i+=4)
            {
                Message.Add(CodesHemming[messageSourceCoder.Substring(i, 4)][1]);
            }
        }

        private void CreatingCodesHemming()
        {
            CodesHemming = new Dictionary<string, List<string>>();

            string informationBits;
            string testBits;
            string codeHemming;

            for (int i = 0;i < 16; i++)
            {
                informationBits = Convert.ToString(i, 2).PadLeft(4, '0');

                testBits = TestBits(informationBits);
                codeHemming = informationBits + testBits;
                CodesHemming.Add(
                    informationBits,
                    new List<string>() {testBits,codeHemming,Convert.ToString(codeHemming.Count(num => num == '1'))}
                    );
            }
        }

        private string TestBits(string informationBits)
        {
            string testBits;

            testBits = XORTestBits(informationBits[0], informationBits[1], informationBits[2]);
            testBits += XORTestBits(informationBits[1], informationBits[2], informationBits[3]);
            testBits += XORTestBits(informationBits[0], informationBits[2], informationBits[3]);

            return testBits;
        }

        private string XORTestBits(char a, char b, char c)
        {
            return Convert.ToString(
                Convert.ToInt32(a.ToString(), 2) ^
                Convert.ToInt32(b.ToString(), 2) ^
                Convert.ToInt32(c.ToString(), 2));
        }
    }
}
