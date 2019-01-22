using System;
using System.Collections.Generic;
using CourseWorkBCT.SupportClass;

namespace CourseWorkBCT.BlocksDS
{
    public class Modulator
    {
        public int TypeModulation { get; private set; }
        public Dictionary<string, double> Parameters { get; private set; }

        public (double[] SignalCounts, double[] TimeCounts, double[] ModSignalCounts) VectorsPlotModulation;
        public (double[] SpectrumCounts, double[] FrequencyCounts, double[] ModSignalCounts) VectorsPlotSpectrum;

        public Modulator(SourceCoder sourceCoder, ChannelEncoder channelEncoder)
        {
            StartBlock(sourceCoder, channelEncoder);
        }

        public double AmplitudeSignal(int[] studentVariation)
        {
            return studentVariation[0] + studentVariation[1] + studentVariation[2] + studentVariation[3] + 1;
        }

        public double SpeedModulation(double encoderOutputSpeed)
        {
            return encoderOutputSpeed;
        }

        public double CarrierFrequency(double speedModulation)
        {
            return 5 * speedModulation;
        }

        public double ClockInterval(double speedModulation)
        {
            return 1 / speedModulation;
        }

        public double DeviationFrequencies(double clockInterval)
        {
            return 1 / (2 * clockInterval);
        }

        public double WidthLineFrequencies(double deviationFrequencies, double clockInterval)
        {
            if (TypeModulation == 1)
            {
                return (2 / clockInterval) + (2 * deviationFrequencies);
            }

            return 2 / clockInterval;
        }

        public double SpectralEfficiency(double widthLineFrequencies, double encoderOutputSpeed)
        {
            return encoderOutputSpeed / widthLineFrequencies;
        }

        public double ZeroTransferPower(double clockInterval)
        {
            double P0 = Integration.IntegrationSimpson(ModulationSelection(), 0, 0, clockInterval);

            return 1 / clockInterval * Math.Pow(P0, 2);
        }

        public double OneTransferPower(double clockInterval)
        {
            double P1 = Integration.IntegrationSimpson(ModulationSelection(), 1, 0, clockInterval);

            return 1 / clockInterval * Math.Pow(P1, 2);
        }

        public double AveragePower(double zeroTransferPower, double oneTransferPower, double p0, double p1)
        {
            return p0 * zeroTransferPower + p1 * oneTransferPower;
        }

        public double PeakPower(double zeroTransferPower, double oneTransferPower)
        {
            return Math.Max(zeroTransferPower, oneTransferPower);
        }

        public double PeakFactor(double averagePower, double peakPower)
        {
            return peakPower / averagePower;
        }

        private void StartBlock(SourceCoder sourceCoder, ChannelEncoder channelEncoder)
        {
            Parameters = new Dictionary<string, double>();

            ChoiseTypeModulation(sourceCoder.messageSource.variationCourseWork);

            Parameters.Add("U0", AmplitudeSignal(sourceCoder.messageSource.variationCourseWork));
            Parameters.Add("V0", SpeedModulation(channelEncoder.Parameters["VKk"]));
            Parameters.Add("f0", CarrierFrequency(Parameters["V0"]));
            Parameters.Add("T0", ClockInterval(Parameters["V0"]));
            Parameters.Add("deltaf", DeviationFrequencies(Parameters["T0"]));
            Parameters.Add("deltaF", WidthLineFrequencies(Parameters["deltaf"], Parameters["T0"]));
            Parameters.Add("nu", SpectralEfficiency(Parameters["deltaF"], channelEncoder.Parameters["VKk"]));
            Parameters.Add("PowerZero", ZeroTransferPower(Parameters["T0"]));
            Parameters.Add("PowerOne", OneTransferPower(Parameters["T0"]));
            Parameters.Add("PowerAverage", AveragePower(
                Parameters["PowerZero"],
                Parameters["PowerOne"],
                sourceCoder.Parameters["PZero"],
                sourceCoder.Parameters["POne"]));
            Parameters.Add("PeakPower", PeakPower(Parameters["PowerZero"], Parameters["PowerOne"]));
            Parameters.Add("PeakFactor", PeakFactor(Parameters["PowerAverage"], Parameters["PeakPower"]));

            MessageRange(MessagePreparation(string.Join("",channelEncoder.Message).ToCharArray()));
            Console.WriteLine("s");
            Modulation();
        }

        private void ChoiseTypeModulation(int[] varible)
        {
            TypeModulation = (varible[1] * varible[2] * varible[3]) % 3; 
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

        private double SignalAM(double c, double t)
        {
            return (Parameters["U0"] / 2) * (1 + c) * Math.Cos(2 * Math.PI * Parameters["f0"] * t);
        }

        private double SignalFM(double c, double t)
        {
            return Parameters["U0"] * c * Math.Cos(2 * Math.PI * Parameters["f0"] * t);
        }

        private double SignalSM(double c, double t)
        {
            double x = (Parameters["U0"] / 2) * (1 - c) * 
                Math.Cos(2 * Math.PI * (Parameters["f0"] - Parameters["deltaf"]) * t);

            x += (Parameters["U0"] / 2) * (1 + c) *
                Math.Cos(2 * Math.PI * (Parameters["f0"] + Parameters["deltaf"]) * t);

            return x; 
        }

        private double SpectrumAMFM(double f)
        {
            return G(f - Parameters["f0"]);
        }

        private double SpectrumCHM(double f)
        {
            return G(f - Parameters["f0"] + Parameters["deltaf"]) + G(f - Parameters["f0"] - Parameters["deltaf"]);
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

        private double ModulationSelection(double c, double t)
        {
            switch (TypeModulation)
            {
                case 0:
                    return SignalFM(c, t);
                case 1:
                    return SignalSM(c, t);
                case 2:
                    return SignalAM(c, t);
            }

            return 0;
        }

        private Func<double,double,double> ModulationSelection()
        {
            switch (TypeModulation)
            {
                case 0:
                    return SignalFM;
                case 1:
                    return SignalSM;
                case 2:
                    return SignalAM;
            }

            return null;
        }

        private double Impulse(double symbolIndex, double pulseDuration, double t)
        {
            return (t >= pulseDuration * symbolIndex) && (t < pulseDuration * ++symbolIndex) ? 1 : 0;
        }

        private double G(double f)
        {
            if (f == 0)
            {
                return Math.Pow(Parameters["U0"], 2) * Parameters["T0"];
            }
            return Math.Pow(Parameters["U0"], 2) * Parameters["T0"] * Sinc(Math.PI * f * Parameters["T0"]);
        }

        private double Sinc(double x)
        {
            return Math.Pow(Math.Sin(x), 2) / Math.Pow(x,2);
        }
    }
}
