using System;

namespace Eml.Extensions
{
    public static class FloatingPointExtensions
    {
        public static bool IsNonZero(this double value, int decimalPlaceValue = 5)
        {
            return !value.IsZero(decimalPlaceValue);
        }

        public static bool IsNonZero(this float value, int decimalPlaceValue = 5)
        {
            return !value.IsZero(decimalPlaceValue);
        }

        public static bool IsNonZero(this decimal value, int decimalPlaceValue = 5)
        {
            return !value.IsZero(decimalPlaceValue);
        }

        public static bool IsZero(this double value, int decimalPlaceValue = 5)
        {
            return value < Math.Pow(10, -decimalPlaceValue);
        }

        public static bool IsZero(this float value, int decimalPlaceValue = 5)
        {
            return value < (float)Math.Pow(10, -decimalPlaceValue);
        }

        public static bool IsZero(this decimal value, int decimalPlaceValue = 5)
        {
            return value < (decimal)Math.Pow(10, -decimalPlaceValue);
        }
    }
}