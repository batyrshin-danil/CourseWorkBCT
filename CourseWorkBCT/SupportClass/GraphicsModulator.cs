using System.Drawing;

using CourseWorkBCT.BlocksDS;
using ZedGraph;
using System.Linq;
using System;

namespace CourseWorkBCT.SupportClass
{
    public static class GraphicsModulator
    {
        private static string AddrFolder = 
            Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\";

        public static void DrawGraphics(Modulator modulator)
        {
            DrawGraphicsModulation(modulator);
            DrawGraphicsSpectrum(modulator);
        }

        private static void DrawGraphicsModulation(Modulator modulator)
        {
            var vectorsPlotModulation = modulator.vectorsPlotModulation;

            string defaultSignal = "Исходный сигнал";
            string modulationSignal = "Модулированный сигнал";

            int minAxisX = 0;
            int maxAxisX = 6;

            CurveList curveItems = new CurveList();

            GraphPane paneSignal = new GraphPane();
            SettingGraph(paneSignal, minAxisX, maxAxisX);

            curveItems.Add(
                CreateCurve(
                    vectorsPlotModulation.TimeCounts, vectorsPlotModulation.SignalCounts, defaultSignal));

            SaveGraphics(DrawCurveGraph(paneSignal, curveItems), defaultSignal, ".png");

            curveItems.Clear();
            paneSignal.CurveList.Clear();

            curveItems.Add(
                CreateCurve(
                    vectorsPlotModulation.TimeCounts, vectorsPlotModulation.ModSignalCounts, modulationSignal));

            SaveGraphics(DrawCurveGraph(paneSignal, curveItems), modulationSignal, ".png");           
        }

        private static void DrawGraphicsSpectrum(Modulator modulator)
        {
            var vectorsPlotSpectrum = modulator.vectorsPlotSpectrum;

            string defaultSpectrum = "Спектр исходного сигнала";
            string modulationSpectrum = "Спектр модулированного сигнала";

            int minAxisX = - (int) modulator.CarrierFrequency;
            int maxAxisX = (int) modulator.CarrierFrequency;

            double maxAmplitude = vectorsPlotSpectrum.ModSpectrumCounts.Max();

            double leftCarrierFrequency = modulator.CarrierFrequency - modulator.DeviationFrequency;
            double rightCarrierFrequency = modulator.CarrierFrequency + modulator.DeviationFrequency;

            CurveList curveItems = new CurveList();

            GraphPane paneSpectrum = new GraphPane();

            SettingGraph(paneSpectrum, minAxisX, maxAxisX);

            curveItems.Add(
                CreateCurve(
                    vectorsPlotSpectrum.FrequencyCounts, vectorsPlotSpectrum.SpectrumCounts, defaultSpectrum));

            SaveGraphics(DrawCurveGraph(paneSpectrum, curveItems), defaultSpectrum, ".png");

            curveItems.Clear();
            paneSpectrum.CurveList.Clear();

            if (modulator.TypeModulation == 1)
            {
                curveItems.Add(
                    CreateCurve(
                        new double[] { leftCarrierFrequency, leftCarrierFrequency }, 
                        new double[] { 0, maxAmplitude},
                        "f\u2080 - \u0394f",
                        SymbolType.HDash));
                curveItems.Add(
                    CreateCurve(
                        new double[] { rightCarrierFrequency, rightCarrierFrequency },
                        new double[] { 0, maxAmplitude },
                        "f\u2080 + \u0394f",
                        SymbolType.HDash));
            }
            else
            {
                curveItems.Add(
                    CreateCurve(
                        new double[] { modulator.CarrierFrequency, modulator.CarrierFrequency },
                        new double[] { 0, maxAmplitude },
                        "f\u2080",
                        SymbolType.HDash));
            }

            SettingCurve(curveItems, Color.Orange);
            SettingGraph(paneSpectrum, 0, maxAxisX * 2);

            curveItems.Add(
                CreateCurve(
                    vectorsPlotSpectrum.ModFrequencyCounts, vectorsPlotSpectrum.ModSpectrumCounts, defaultSpectrum));

            SaveGraphics(DrawCurveGraph(paneSpectrum, curveItems), modulationSpectrum, ".png");
        }

        private static CurveItem CreateCurve(double[] x, double[] y, string label, 
            SymbolType symbolType = SymbolType.None)
        {
            GraphPane graphPane = new GraphPane();

            LineItem lineItem = graphPane.AddCurve(label, CreatePoints(x, y), Color.Blue, symbolType);
            lineItem.Line.Width = 0.5f;

            return lineItem;
        }

        private static PointPairList CreatePoints(double[] x, double[] y)
        {
            PointPairList pointPairs = new PointPairList();

            for (int i = 0; i < x.Length; i++)
            {
                pointPairs.Add(x[i], y[i]);
            }

            return pointPairs;
        }

        private static void SettingCurve(CurveList curve, Color color)
        {
            for(int i = 0; i < curve.Count; i++)
            {
                curve[i].Color = color;
            }
        }

        private static GraphPane DrawCurveGraph(GraphPane graphPane, CurveList curveItems)
        {
            graphPane.CurveList = curveItems;
            graphPane.AxisChange();

            return graphPane;
        }

        private static void SettingGraph(GraphPane graphPane, int minAxisX, int maxAxisX)
        {
            graphPane.Rect = new RectangleF(0,0,1820,780);

            graphPane.XAxis.MajorGrid.IsVisible = true;
            graphPane.YAxis.MajorGrid.IsVisible = true;
            graphPane.YAxis.MinorGrid.IsVisible = true;
            graphPane.XAxis.MinorGrid.IsVisible = true;

            graphPane.XAxis.MajorGrid.DashOn = 4;
            graphPane.XAxis.MajorGrid.DashOff = 2;

            graphPane.YAxis.MajorGrid.DashOn = 4;
            graphPane.YAxis.MajorGrid.DashOff = 2;

            graphPane.YAxis.MinorGrid.DashOn = 1;
            graphPane.YAxis.MinorGrid.DashOff = 2;

            graphPane.XAxis.MinorGrid.DashOn = 1;
            graphPane.XAxis.MinorGrid.DashOff = 2;

            graphPane.XAxis.Scale.Min = minAxisX;
            graphPane.XAxis.Scale.Max = maxAxisX;
        }

        private static void SaveGraphics(GraphPane graphPane, string name, string formatImage)
        {
            Bitmap bitmap = new Bitmap(graphPane.GetImage());
            bitmap.Save(AddrFolder + name + formatImage);
        }

    }
}
