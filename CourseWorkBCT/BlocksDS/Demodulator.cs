using System;
using System.Collections.Generic;
using System.Text;
using CourseWorkBCT.SupportClass;

namespace CourseWorkBCT.BlocksDS
{
    public class Demodulator
    {
        private static int countErrors = 4;

        private int typeCommunicationChannel;      
        private List<int> errorsPosition = new List<int>(){0,0,0,0};

        private string[] informationFolderReceptionMethod = new string[] 
        {
            // TODO 
            // Вбить адреса папок с файлами для каждого типа модуляции.
        };

        public double ProbabilityError { get; private set; }
        public List<string> Message { get; private set; }

        public string InformationFolderReceptionMethod
        {
            get
            {
                return informationFolderReceptionMethod[typeCommunicationChannel];
            }
        }

        public Demodulator()
        {

        }

        public Demodulator(
            ChannelEncoder channelEncoder, Modulator modulator, CommunicationChannel communicationChannel)
        {
            Initialization(channelEncoder, modulator, communicationChannel);
        }

        private void Initialization(ChannelEncoder channelEncoder, Modulator modulator, CommunicationChannel communicationChannel)
        {
            typeCommunicationChannel = communicationChannel.TypeCommunicationChannel;
            ProbabilityError = CalculationProbabilityError(
                  communicationChannel.TypeCommunicationChannel, modulator.TypeModulation, communicationChannel.PeakRatioSignalNoise);
            Message = SimulationErrors(channelEncoder.Message);         
        }

        public double CalculationProbabilityError(
            int typeCommunicationChannel, int typeModulation, double peakRatioSignalNoise)
        {
            return ChoiceFormulaProbabilityError(typeCommunicationChannel, typeModulation, peakRatioSignalNoise);
        }
        
        private double ChoiceFormulaProbabilityError(
            int typeCommunicationChannel, int typeModulation, double peakRatioSignalNoise)
        {
            double probabilityError = 0;

            if(typeCommunicationChannel == 0)
            {
                probabilityError = ChoiceFormulaCoherentReception(typeModulation, peakRatioSignalNoise);
            }

            probabilityError = ChoiceFormulaIncoherentReception(typeModulation, peakRatioSignalNoise);

            return probabilityError;
        }

        private double ChoiceFormulaCoherentReception(int typeModulation, double peakRatioSignalNoise)
        {
            if(typeModulation == 0)
            {
                peakRatioSignalNoise *= 2;
            }
            else if(typeModulation == 2)
            {
                peakRatioSignalNoise /= 2;
            }

            return QFunction.Q(Math.Sqrt(peakRatioSignalNoise));
        }

        private double ChoiceFormulaIncoherentReception(int typeModulation, double peakRatioSignalNoise)
        {
            if (typeModulation == 1)
            {
                peakRatioSignalNoise /= 4;
            }
            else if (typeModulation == 2)
            {
                peakRatioSignalNoise /= 2;
            }

            return 0.5 * Math.Exp(-peakRatioSignalNoise);
        }

        private List<string> SimulationErrors(List<string> messageChannelEncoder)
        {
            ChoiceRandomErrorsPosition();

            List<string> errorMessage = messageChannelEncoder;
            int countErrorBlocks = 3;
            StringBuilder blockMessage;

            for  (int i = 0;i < countErrorBlocks; i++)
            {
                blockMessage = new StringBuilder(messageChannelEncoder[i]);

                errorMessage[i] = InvertBit(errorsPosition[i + 1], blockMessage);
                if (i == 3)
                {
                    errorMessage[i] = InvertBit(errorsPosition[i + 1], blockMessage);
                }               
            }

            return errorMessage;
        }

        private string InvertBit(int errorPosition, StringBuilder errorBit)
        {
            errorBit[errorPosition] = errorBit[errorPosition] == '1' ? '0' : '1';
            return errorBit.ToString();
        }

        private void ChoiceRandomErrorsPosition()
        {
            Random randomPosition = new Random();

            for(int i = 0;i < countErrors; i++)
            {
                int position = 0;
                while (true)
                {
                    position = randomPosition.Next(0, 7);
                    if (errorsPosition.Contains(position))
                    {
                        continue;
                    }
                    errorsPosition[i] = position;
                    break;
                }
            }
        }
    }
}
