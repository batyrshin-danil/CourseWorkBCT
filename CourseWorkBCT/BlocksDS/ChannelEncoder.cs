using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseWorkBCT.BlocksDS
{
    public class ChannelEncoder
    {
        public List<string> Message { get; private set; }
        public Dictionary<string, List<string>> CodesHemming { get; private set; }
        public Dictionary<string, double> Parameters { get; private set; }

        public ChannelEncoder(SourceCoder sourceCoder)
        {
            CreatingTableHemming();
            HemmingCodingMessage(string.Join("", sourceCoder.Message));
            StartBlock(sourceCoder.Parameters);
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

        private void StartBlock(Dictionary<string, double> parametersSC)
        {
            Parameters = new Dictionary<string, double>();

            Parameters.Add("PKk", PKk(3, 7));
            Parameters.Add("R", R(4, 7));
            Parameters.Add("DMin", DMin(Message));
            Parameters.Add("Qi", Qi(Parameters["DMin"]));
            Parameters.Add("N", N(4, Parameters["R"]));
            Parameters.Add("VKk", VKk(parametersSC["VKi"], Parameters["R"]));
        }

        private void HemmingCodingMessage(string message)
        {
            for (int i = 0;i < message.Length;i+=4)
            {
                Message.Add(CodesHemming[message.Substring(i, 4).PadRight(4, '0')][1]);
            }
        }

        private void CreatingTableHemming()
        {
            CodesHemming = new Dictionary<string, List<string>>();

            string binNumber;
            string testBits;
            string code;

            for (int i = 0;i < 16; i++)
            {
                binNumber = Convert.ToString(i, 2).PadLeft(4, '0');
                testBits = TestBits(new int[4]{
                            Convert.ToInt32(binNumber[0]),
                            Convert.ToInt32(binNumber[1]),
                            Convert.ToInt32(binNumber[2]),
                            Convert.ToInt32(binNumber[3])});
                code = binNumber + testBits;

                CodesHemming.Add(
                    binNumber,
                    new List<string>() {testBits,code,Convert.ToString(code.Count(num => num == '1'))}
                    );
            }
        }

        private string TestBits(int[] number)
        {
            string testBits = Convert.ToString(number[0] ^ number[1] ^ number[2]);
            testBits += Convert.ToString(number[1] ^ number[2] ^ number[3]);
            testBits += Convert.ToString(number[0] ^ number[2] ^ number[3]);

            return testBits;
        }
    }
}
