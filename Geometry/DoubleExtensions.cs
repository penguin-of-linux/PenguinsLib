namespace Geometry
{
    public static class DoubleExtensions
    {
        public static bool Less(this double a, double b, double eps = 0.001)
        {
            return b - a > eps;
        }
        public static bool Greater(this double a, double b, double eps = 0.001)
        {
            return b.Less(a, eps);
        }
        public static bool Equal(this double a, double b, double eps = 0.001)
        {
            return !a.Less(b, eps) && !a.Greater(b, eps);
        }
        public static bool LessOrEqual(this double a, double b, double eps = 0.001)
        {
            return a.Less(b, eps) || a.Equal(b, eps);
        }
        public static bool GreaterOrEqual(this double a, double b, double eps = 0.001)
        {
            return a.Greater(b, eps) || a.Equal(b, eps);
        }
    }
}