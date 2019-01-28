using System;
using System.Collections.Generic;
using CourseWorkBCT.SupportClass;

namespace CourseWorkBCT.BlocksDS
{
    public class Modulator
    {
        public int TypeModulation { get; private set; }

        public (double[] SignalCounts, double[] TimeCounts, double[] ModSignalCounts) VectorsPlotModulation;
        public (double[] SpectrumCounts, double[] FrequencyCounts, double[] ModSignalCounts) VectorsPlotSpectrum;

        public double AmplitudeSignal { get; private set; }
        public double SpeedModulation { get; private set; }
        public double CarrierFrequency { get; private set; }
        public double ClockInterval { get; private set; }
        public double DeviationFrequency { get; private set; }
        public double WidthLineFrequencies { get; private set; }
        public double SpectralEfficiency { get; private set; }
        public double ZeroTransferPower { get; private set; }
        public double OneTransferPower { get; private set; }
        public double AveragePower { get; private set; }
        public double PeakPower { get; private set; }
        public double PeakFactor { get; private set; }

        public SourceCoder SourceCoder { get; private set; }

        public Modulator(SourceCoder sourceCoder, ChannelEncoder channelEncoder)
        {
            SourceCoder = sourceCoder;
            Initialization(sourceCoder, channelEncoder);
        }

        public double CalculationAmplitudeSignal(int[] studentVariation)
        {
            return studentVariation[0] + studentVariation[1] + studentVariation[2] + studentVariation[3] + 1;
        }

        public double CalculationSpeedModulation(double encoderOutputSpeed)
        {
            return encoderOutputSpeed;
        }

        public double CalculationCarrierFrequency(double speedModulation)
        {
            return 5 * speedModulation;
        }

        public double CalculationClockInterval(double speedModulation)
        {
            return 1 / speedModulation;
        }

        public double CalculationDeviationFrequeny(double clockInterval)
        {
            return 1 / (2 * clockInterval);
        }

        public double CalculationWidthLineFrequencies(double deviationFrequencies, double clockInterval)
        {
            if (TypeModulation == 1)
            {
                return (2 / clockInterval) + (2 * deviationFrequencies);
            }

            return 2 / clockInterval;
        }

        public double CalculationSpectralEfficiency(
            double widthLineFrequencies, double averageSpeedBitsChannelEncoder)
        {
            return averageSpeedBitsChannelEncoder / widthLineFrequencies;
        }

        public double CalculationZeroTransferPower(double clockInterval)
        {
            double P0 = Integration.IntegrationSimpson(ModulationSelection(), 0, 0, clockInterval);

            return 1 / clockInterval * Math.Pow(P0, 2);
        }

        public double CalculationOneTransferPower(double clockInterval)
        {
            double P1 = Integration.IntegrationSimpson(ModulationSelection(), 1, 0, clockInterval);

            return 1 / clockInterval * Math.Pow(P1, 2);
        }

        public double CalculationAveragePower(
            double zeroTransferPower, double oneTransferPower, 
            double probabilityZero, double probabilityOne)
        {
            return probabilityZero * zeroTransferPower + probabilityOne * oneTransferPower;
        }

        public double CalculationPeakPower(double zeroTransferPower, double oneTransferPower)
        {
            return Math.Max(zeroTransferPower, oneTransferPower);
        }

        public double CalculationPeakFactor(double averagePower, double peakPower)
        {
            return peakPower / averagePower;
        }

        public int ChoiceTypeModulation(int[] variationCourseWork)
        {
            return (variationCourseWork[1] * variationCourseWork[2] * variationCourseWork[3]) % 3;
        }

        private void Initialization(SourceCoder sourceCoder, ChannelEncoder channelEncoder)
        {
            TypeModulation = ChoiceTypeModulation(sourceCoder.messageSource.VariationCourseWork);

            AmplitudeSignal = CalculationAmplitudeSignal(sourceCoder.messageSource.VariationCourseWork);
            SpeedModulation = CalculationSpeedModulation(channelEncoder.AverageSpeedBits);
            CarrierFrequency = CalculationCarrierFrequency(SpeedModulation);
            ClockInterval = CalculationClockInterval(SpeedModulation);
            DeviationFrequency = CalculationDeviationFrequeny(ClockInterval);
            WidthLineFrequencies = CalculationWidthLineFrequencies(DeviationFrequency, ClockInterval);
            SpectralEfficiency = CalculationSpectralEfficiency(WidthLineFrequencies, channelEncoder.AverageSpeedBits);
            ZeroTransferPower = CalculationZeroTransferPower(ClockInterval);
            OneTransferPower = CalculationOneTransferPower(ClockInterval);

            AveragePower = CalculationAveragePower(ZeroTransferPower, OneTransferPower, 
                sourceCoder.ProbabilityOne, sourceCoder.ProbabilityZero);

            PeakPower = CalculationPeakPower(ZeroTransferPower, OneTransferPower);
            PeakFactor = CalculationPeakFactor(AveragePower, PeakPower);

            MessageRange(MessagePreparation(string.Join("",channelEncoder.Message).ToCharArray()));
            Modulation();
        }

        private int[] MessagePreparation(char[] message)
        {
            int[] messageLocal = new int[message.Length];

            for (int i = 0;i < message.Length; i++)
            {
                if (message[i] == '0')
                    messageLocal[i] = -1;
                else
                    messageLocal[i] = 1;
            }

            return messageLocal;
        }

        private void MessageRange(int[] message)
        {
            int messageLength = message.Length;
            int pulseDuration = 1;
            int simulationInterval = messageLength * pulseDuration;

            int numberSamples = Convert.ToInt32(simulationInterval / 0.01);

            VectorsPlotModulation.SignalCounts = new double[numberSamples];
            VectorsPlotModulation.TimeCounts = new double[numberSamples];

            for(int i = 0;i < numberSamples;i++)
            {
                double sum = 0;
                VectorsPlotModulation.TimeCounts[i] = i * 0.01;
                
                for (int j = 0;j < messageLength; j++)
                {
                    sum += message[j] * Impulse(j, pulseDuration, VectorsPlotModulation.TimeCounts[i]);
                }

                VectorsPlotModulation.SignalCounts[i] = sum; 
            }
        }

        private double SignalAM(double countSignal, double countTime)
        {
            return (AmplitudeSignal / 2) * (1 + countSignal) * Math.Cos(2 * Math.PI * CarrierFrequency * countTime);
        }

        private double SignalPM(double countSignal, double countTime)
        {
            return AmplitudeSignal * countSignal * Math.Cos(2 * Math.PI * CarrierFrequency * countTime);
        }

        private double SignalFM(double countSignal, double countTime)
        {
            double signal = (AmplitudeSignal / 2) * (1 - countSignal) * 
                Math.Cos(2 * Math.PI * (CarrierFrequency - DeviationFrequency) * countTime);

            signal += (AmplitudeSignal / 2) * (1 + countSignal) *
                Math.Cos(2 * Math.PI * (CarrierFrequency + DeviationFrequency) * countTime);

            return signal; 
        }

        private double SpectrumAMFM(double frequency)
        {
            return G(frequency - CarrierFrequency);
        }

        private double SpectrumCHM(double frequency)
        {
            return G(
                frequency - CarrierFrequency + DeviationFrequency) + 
                G(frequency - CarrierFrequency - DeviationFrequency);
        }

        private void Modulation()
        {
            VectorsPlotModulation.ModSignalCounts = new double[VectorsPlotModulation.SignalCounts.Length];
            Console.WriteLine(VectorsPlotModulation.SignalCounts.Length);
            for(int i = 0;i < VectorsPlotModulation.SignalCounts.Length; i++)
            {
                Console.WriteLine(i);
                VectorsPlotModulation.ModSignalCounts[i] = ModulationSelection(
                    VectorsPlotModulation.SignalCounts[i], VectorsPlotModulation.TimeCounts[i]);
            }
        }

        private double ModulationSelection(double countSignal, double countTime)
        {
            switch (TypeModulation)
            {
                case 0:
                    return SignalPM(countSignal, countTime);
                case 1:
                    return SignalFM(countSignal, countTime);
                case 2:
                    return SignalAM(countSignal, countTime);
            }

            return 0;
        }

        private Func<double,double,double> ModulationSelection()
        {
            switch (TypeModulation)
            {
                case 0:
                    return SignalPM;
                case 1:
                    return SignalFM;
                case 2:
                    return SignalAM;
            }

            return null;
        }

        private double Impulse(double symbolIndex, double pulseDuration, double countTime)
        {
            return (countTime >= pulseDuration * symbolIndex) && (countTime < pulseDuration * ++symbolIndex) ? 1 : 0;
        }

        private double G(double frequency)
        {
            if (frequency == 0)
            {
                return Math.Pow(AmplitudeSignal, 2) * ClockInterval;
            }
            return Math.Pow(AmplitudeSignal, 2) * ClockInterval * 
                Sinc(Math.PI * frequency * ClockInterval);
        }

        private double Sinc(double x)
        {
            return Math.Pow(Math.Sin(x), 2) / Math.Pow(x,2);
        }
    }
}
