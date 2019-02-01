using System;

namespace CourseWorkBCT.SupportClass
{
    static class Integration
    {
        public static double h = 0.0001;

        public static double IntegrationSimpson(Func<double, double, double> fun, double c, double a, double b)
        {
            double resultIntegration = 0;

            double n = (b - a) / h;

            resultIntegration = h * (fun(c, a) + fun(c, b)) / 6.0;

            for (int i = 1; i <= n; i++)
            {
                resultIntegration = resultIntegration + 4.0 / 6.0 * h * fun(c, a + h * (i - 0.5));
            }

            for (int i = 1; i <= n - 1; i++)
            {
                resultIntegration = resultIntegration + 2.0 / 6.0 * h * fun(c, a + h * i);
            }

            return resultIntegration;
        }

        public static double IntegrationSimpson(Func<double, double> fun, double a, double b)
        {
            double resultIntegration = 0;

            double n = (b - a) / h;

            resultIntegration = h * (fun(a) + fun(b)) / 6.0;

            for (int i = 1; i <= n; i++)
            {
                resultIntegration = resultIntegration + 4.0 / 6.0 * h * fun(a + h * (i - 0.5));
            }

            for (int i = 1; i <= n - 1; i++)
            {
                resultIntegration = resultIntegration + 2.0 / 6.0 * h * fun(a + h * i);
            }

            return resultIntegration;
        }
    }
}
