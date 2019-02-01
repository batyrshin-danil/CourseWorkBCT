using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkBCT.SupportClass
{
    public static class QFunction
    {
        public static double Q(double hightLimit)
        {
            return 1 / 2 * (1 - Erf(hightLimit / Math.Sqrt(2)));
        }

        private static double Erf(double hightLimit)
        {
            return 2 / Math.Sqrt(Math.PI) * Integration.IntegrationSimpson(ErfIntegralExpression, 0, hightLimit);
        }

        private static double ErfIntegralExpression(double t)
        {
            return Math.Exp(Math.Pow(-t, 2));
        }
    }
}
