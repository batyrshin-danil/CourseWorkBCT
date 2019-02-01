using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkBCT.BlocksDS
{
    public class SourceDecoder
    {
        public List<string> Message { get; private set; }

        public SourceDecoder()
        {

        }

        public SourceDecoder(SourceCoder sourceCoder, ChannelDecoder channelDecoder)
        {
            Message = RestoreMessage(sourceCoder.EconomicalCodes, channelDecoder.Message);
            foreach(string sym in Message)
            {
                Console.WriteLine(sym);
            }
        }

        public List<string> RestoreMessage(
            Dictionary<string, string> economicalCodes, List<string> messageChannelDecoder)
        {
            List<string> restoreMessage = new List<string>();
            string message = string.Join("", messageChannelDecoder);

            while (message.Length != 0)
            {
                Console.WriteLine(message.Length);
                for(int i = 0; i < message.Length; i++)
                {
                    if (economicalCodes.ContainsValue(message.Substring(0,i)))
                    {
                        restoreMessage.Add(SearchKeyDictionary(economicalCodes, message.Substring(0, i)));
                        message = message.Remove(0, i);
                        break;
                    }
                    if (i == message.Length - 1)
                    {
                        message = "";
                    }
                }
            }

            return restoreMessage;
        }

        private string SearchKeyDictionary(Dictionary<string, string> economicalCodes, string value)
        {
            string key = "";
            foreach(string nextKey in economicalCodes.Keys)
            {
                if (economicalCodes[nextKey] == value)
                {
                    key = nextKey;
                    break;
                }
            }
            return key;
        }
    }
}
