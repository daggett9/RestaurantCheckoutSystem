namespace Utils.Extensions
{
    public static class FloatExtensions
    {
        public static bool AreFloatsEqual(this float a, float b, float epsilon = 0.0001f)
        {
            return Math.Abs(a - b) < epsilon;
        }
    }
}