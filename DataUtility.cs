using System;

namespace Population
{
    static class DataUtility
    {
        static private Random rnd = new Random();

        static public double GaussianSample(double mean, double stdDev)
        {
            double u1, u2;

            u1 = 1.0 - rnd.NextDouble();
            u2 = 1.0 - rnd.NextDouble();

            // Box-Muller transformation
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            return mean + randStdNormal * stdDev;
        }

        static public double GetRandIntervalStartPosition(double intervalSize, double rangeSize)
        {
            return rnd.NextDouble() * (rangeSize - intervalSize);
        }
    }
}
