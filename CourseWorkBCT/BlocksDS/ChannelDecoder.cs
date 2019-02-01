using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CourseWorkBCT.SupportClass;

namespace CourseWorkBCT.BlocksDS
{
    public class ChannelDecoder
    {
        private int lengthBlockMessage = 7;

        public List<string> Sindroms { get; private set; } = new List<string>(7);
        public List<string> Message { get; private set; }
        public List<string> MessageNotTestBits { get; private set; }

        public double BlockErrorProbability { get; private set; }
        public double WrongDecodedBlock { get; private set; }

        public ChannelDecoder()
        {

        }

        public ChannelDecoder(ChannelEncoder channelEncoder, Demodulator demodulator)
        {
            BlockErrorProbability = CalculationBlockErrorProbability(
                channelEncoder.CodeCorrectingAbility, demodulator.ProbabilityError);
            WrongDecodedBlock = CalculationWrongDecodedBlock(BlockErrorProbability);

            CreateSindroms();

            Message = CorrectionError(demodulator.Message);
            MessageNotTestBits = RemoveTestBits(Message);
        }

        public double CalculationBlockErrorProbability(double codeCorrectingAbility, double probabilityError)
        {
            return Combinatorics.Combination(lengthBlockMessage, Convert.ToInt32(codeCorrectingAbility + 1)) *
                Math.Pow(probabilityError, Convert.ToInt32(codeCorrectingAbility + 1));
        }

        public double CalculationWrongDecodedBlock(double blockErrorProbability)
        {
            return 0.5 * blockErrorProbability;
        }

        public List<string> CorrectionError(List<string> messageDemodulator)
        {
            List<string> correctionMessage = messageDemodulator;

            StringBuilder messageBlock = new StringBuilder();
            string sindromMessageBlock;
           
            for (int i = 0; i < messageDemodulator.Count; i++)
            {
                messageBlock.Clear().AppendFormat(correctionMessage[i]);
                sindromMessageBlock = CreateSindrom(messageBlock);

                if (Sindroms.Contains(sindromMessageBlock))
                {
                    int errorPosition = Sindroms.IndexOf(sindromMessageBlock);
                    char errorBit = messageBlock[errorPosition];

                    messageBlock[errorPosition] = errorBit == '1' ? '0' : '1';
                }

                correctionMessage[i] = messageBlock.ToString();
            }

            return correctionMessage;
        }

        public List<string> RemoveTestBits(List<string> message)
        {
            List<string> messageNotTestBits = message;
            
            for(int i = 0;i < message.Count; i++)
            {
                messageNotTestBits[i] = messageNotTestBits[i].Substring(0, 4); 
            }

            return messageNotTestBits;
        }

        private void CreateSindroms()
        {
            StringBuilder testCombination = new StringBuilder("1000000");

            for (int i = 0;i < Sindroms.Count; i++)
            {
                Sindroms[i] = CreateSindrom(testCombination);

                if (i > 0)
                {
                    testCombination[i - 1] = '0';
                }

                testCombination[i] = '1';
            }
        }
        
        private string CreateSindrom(StringBuilder combinationBits)
        {
            string sindrom = "";

            sindrom = XORBits.XOR(
                    combinationBits[4], combinationBits[0], combinationBits[1], combinationBits[2]);
            sindrom += XORBits.XOR(
                combinationBits[5], combinationBits[1], combinationBits[2], combinationBits[3]);
            sindrom += XORBits.XOR(
                combinationBits[6], combinationBits[0], combinationBits[2], combinationBits[3]);

            return sindrom;
        }
    }
}
