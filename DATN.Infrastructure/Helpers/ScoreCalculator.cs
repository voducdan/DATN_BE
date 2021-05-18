using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Infrastructure.Helpers
{
    public static class ScoreCalculator
    {
        public static double sigmoid(double x, int offset, int scale)
        {
            return (x + offset) / (scale + Math.Abs(x + offset));
        }

        public static float calcScore(float n, float p, float y)
        {
            float num = y - n * p;
            double denom = Math.Sqrt(n * p * (1 - p));
            denom = denom == 0 ? Math.Pow(10, -10) : denom;
            double z = num / denom;
            double result = 0.2 * ScoreCalculator.sigmoid(z, -80, 50) + 0.2 * ScoreCalculator.sigmoid(
                z, -30, 30) + 0.2 * ScoreCalculator.sigmoid(z, 0, 30) + 0.2 * ScoreCalculator.sigmoid(
                    z, 30, 30) + 0.2 * ScoreCalculator.sigmoid(z, 80, 50);
            float score = (float)(Math.Round(result * 1e5) / 1e5);
            return score;
        }

        public static float findRelatedScore(int s1Cnt, int s2Cnt,int s1S2Cnt ,int dataSize)
        {
            float p = s2Cnt / dataSize;
            return ScoreCalculator.calcScore(s1Cnt, p, s1S2Cnt);
        }
    }
}
