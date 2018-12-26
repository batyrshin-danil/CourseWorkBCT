using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkOTS.BlocksDS
{
    // 
    // Класс описывающий блок "Источник сообщения" в CПДС.  
    //
    public class MessageSource
    {
        private int BaseAlphabet;
        private int[] variable;
        private List<string> message = new List<string>();
        private double[] parameterMessageSource = new double[5];

        private Dictionary<string, double> probalitiesSymbol;

        public MessageSource(int [] variable)
        {
            this.variable = variable;
            CreatingProbalityTable();
            StartBlock();
        }

        public MessageSource(Dictionary<string, double> probalitiesSymbol)
        {
            this.probalitiesSymbol = probalitiesSymbol;
            StartBlock();
        }

        public double Pi(double H, double HMax)
        {
            return 1 - (H / HMax);
        }

        public double HHatch(double Vi, double H)
        {
            return Vi * H;
        }

        public List<string> GenerationgMessage(List<string> listSymbol, int lenghtMessage)
        {
            List<string> messageLocal = new List<string>();
            for (int i = 0; i < lenghtMessage; i++)
            {
                messageLocal.Add(listSymbol[new Random().Next(listSymbol.Count)]);
            }
            return messageLocal;
        }
        
        public Dictionary<string, double> getProbalitiesSymbol()
        {
            return probalitiesSymbol;
        }

        // Метод вычисления вероятностей появления символов.
        private void CreatingProbalityTable()
        {
            probalitiesSymbol = new Dictionary<string, double>()
            {
                {"а", 0.310 * (1.0 - 0.1 * variable[3])},   {"б", 0.002 + (0.001 * variable[0])},
                {"в", 0.007 + (0.025 * variable[2])},       {"г", 0.009 * (1.0 - 0.1 * variable[1])},
                {"д", 0.020 * (1 - 0.1 * variable[3])},     {"е", 0.005 + (0.015 * variable[1])},
                {"з", 0.010 * (1 - 0.1 * variable[0])},     {"и", 0.15 * (1 - 0.1 * variable[1])},
                {"к", 0.008 + (0.031 * variable[3]) },      {"л", 0.2 * (1 - 0.1 * variable[0])},
                {"м", 0.006 + (0.02 * variable[0]) },       {"н",  0.015 * (1 - 0.1 * variable[2])},
                {"о", 0.004 + (0.002 * variable[3]) },      {"п", 0.003 + (0.0015 * variable[2])},
                {"р", 0.250 * (1 - 0.1 * variable[2])},     {"с", 0.001 + (0.0009 * variable[1])}
            };
        }

        private void SettingBaseAlphabet()
        {
            BaseAlphabet = probalitiesSymbol.Count();
        }

        private void StartBlock()
        {
            parameterMessageSource[0] = Vi();
            parameterMessageSource[1] = H();
            parameterMessageSource[2] = HMax();
            parameterMessageSource[3] = Pi();
            parameterMessageSource[4] = HHatch();
        }

        // 
        // Далее следуют методы вычисления каждого из параметров источника сообщения.
        //
        private double Vi()
        {
            double vi = 0;
            for (int i = 0; i < 4 ;i++)
            {
                vi += variable[i];
            }
            return vi;
        }

        private double H()
        {
            double H = 0;
            foreach(string key in probalitiesSymbol.Keys)
            {
                H += probalitiesSymbol[key]  * Math.Log(probalitiesSymbol[key],2);
            }
            return H;
        }

        private double HMax()
        {
            return Math.Log(BaseAlphabet, 2);
        }

        private double Pi()
        {
            return 1 - (parameterMessageSource[1] / parameterMessageSource[2]);
        }

        private double HHatch()
        {
            return parameterMessageSource[0] * parameterMessageSource[1];
        }

        private List<string> GenerationgMessage()
        {
            List<string> messageLocal = new List<string>();
            List<string> keys = probalitiesSymbol.Keys.ToList();
            for (int i = 0; i < 10; i++)
            {
                messageLocal.Add(Convert.ToString(probalitiesSymbol[keys[new Random().Next(keys.Count())]]));
            }
            return messageLocal;
        }

    }
}
