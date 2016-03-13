using System;

namespace FiniteElements
{
    class GaussIntegrator
    {
        private static int nodesAmount = 5;
        private static double[] nodes =
        {
            -(1 / 3d) * Math.Sqrt(5 + 2 * Math.Sqrt(10 / 7d)),
            -(1 / 3d) * Math.Sqrt(5 - 2 * Math.Sqrt(10 / 7d)),
            0,
            (1 / 3d) * Math.Sqrt(5 - 2 * Math.Sqrt(10 / 7d)),
            (1 / 3d) * Math.Sqrt(5 + 2 * Math.Sqrt(10 / 7d))
        };
        private static double[] weights =
        {
            (322 - 13 * Math.Sqrt(70)) / 900d,
            (322 + 13 * Math.Sqrt(70)) / 900d,
            128 / 225d,
            (322 + 13 * Math.Sqrt(70)) / 900d,
            (322 - 13 * Math.Sqrt(70)) / 900d
        };

        public static double Integrate(double a, double b, Func<double, double, double> f, double x1)
        {
            double temp = 0;

            for (int i = 0; i < nodesAmount; i++)
            {
                temp += weights[i] * f(0.5 * ((a + b) + (b - a) * nodes[i]), x1);
            }
            temp *= (0.5 * (b - a));

            return temp;
        }
    }
}
