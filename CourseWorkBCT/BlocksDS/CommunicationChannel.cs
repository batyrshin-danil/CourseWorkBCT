using System;

namespace CourseWorkBCT.BlocksDS
{
    public class CommunicationChannel
    {
        public int TypeCommunicationChannel { get; private set; }

        public double TrahsferСoefficient { get; private set; }
        public double SpectralDensityNoisePower { get; private set; }
        public double Passband { get; private set; }
        public double PeakPower { get; private set; }
        public double AveragePower { get; private set; }
        public double AveragePowerNoise { get; private set; }
        public double PeakRatioSignalNoise { get; private set; }
        public double AverageRatioSignalNoise { get; private set; }
        public double ChannelCapacity { get; private set; }

        public string[] LatexFormuleTypeChannel { get; private set; } = new string[]
        {
            @"z(t)=\gamma u(t) + n(t) = s(t) + n(t)",
            @"z(t)=\gamma [u(t)cos(\theta) - \~{u}(t)sin(\theta)] + n(t) = s(t) + n(t)"
        };

        public CommunicationChannel(MessageSource messageSource, Modulator modulator)
        {
            Initialization(messageSource, modulator);
        }

        public double CalculationTrahsferСoefficient(int[] variationCourseWork)
        {
            return 1 / (variationCourseWork[0] + variationCourseWork[1] + 
                variationCourseWork[2] + variationCourseWork[3] + 1);
        }

        public double CalculationSpectralDensityNoisePower(int[] variationCourseWork, double typeModulation,
            double speedMessageSource)
        {
            double spectralDensity = 1 / (14 * speedMessageSource);
            double denominator = Math.Pow(
                2 + (variationCourseWork[2] + variationCourseWork[3]) % 4 + TypeCommunicationChannel, 
                typeModulation);

            spectralDensity *= 1 / denominator;

            return spectralDensity;
        }

        public double CalculationPassband(double widthLineFrequencies)
        {
            return widthLineFrequencies;
        }

        public double CalculationPeakPower(double peakPower)
        {
            return Math.Pow(TrahsferСoefficient, 2) * peakPower;
        }

        public double CalculationAveragePower(double averagePower)
        {
            return Math.Pow(TrahsferСoefficient, 2) * averagePower;
        }

        public double CalculationAveragePowerNoise(double passband, double spectralDensityNoisePower)
        {
            return passband * spectralDensityNoisePower;
        }

        public double CalculationPeakRatioSignalNoise(double peakPowerCommunicationChannel,
            double clockInterval, double spectralDensityNoisePower)
        {
            return (peakPowerCommunicationChannel * clockInterval) / spectralDensityNoisePower;
        }

        public double CalculationAverageRatioSignalNoise(double averagePowerCommunicationChannel,
            double clockInterval, double spectralDensityNoisePower)
        {
            return (averagePowerCommunicationChannel * clockInterval) / spectralDensityNoisePower;
        }

        public double CalculationChannelCapacity(double passband, double averagePowerCommunicationChannel,
            double averagePowerNoise)
        {
            return passband * Math.Log(1 + averagePowerCommunicationChannel / averagePowerNoise, 2);
        }

        public int ChoiceTypeChannel(int[] variationCouseWork, int typeModulation)
        {
            if (typeModulation == 0)
            {
                return 0;
            }
            return (variationCouseWork[0] + variationCouseWork[3]) % 2;
        }

        private void Initialization(MessageSource messageSource, Modulator modulator)
        {
            TypeCommunicationChannel = ChoiceTypeChannel(messageSource.VariationCourseWork, modulator.TypeModulation);
            TrahsferСoefficient = CalculationTrahsferСoefficient(messageSource.VariationCourseWork);

            SpectralDensityNoisePower = CalculationSpectralDensityNoisePower(messageSource.VariationCourseWork,
                modulator.TypeModulation, messageSource.Speed);

            Passband = CalculationPassband(modulator.WidthLineFrequencies);

            PeakPower = CalculationPeakPower(modulator.PeakPower);

            AveragePower = CalculationAveragePower(modulator.AveragePower);

            AveragePowerNoise = CalculationAveragePowerNoise(Passband, SpectralDensityNoisePower);

            PeakRatioSignalNoise = CalculationPeakRatioSignalNoise(PeakPower, modulator.ClockInterval, 
                SpectralDensityNoisePower);

            AverageRatioSignalNoise = CalculationAverageRatioSignalNoise(AveragePower, modulator.ClockInterval, 
                SpectralDensityNoisePower);

            ChannelCapacity = CalculationChannelCapacity(Passband, AveragePower, AveragePowerNoise);
        }
    }
}
