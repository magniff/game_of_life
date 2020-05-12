using System;
namespace functions
{
    static class Functions
    {
        public static R apply<T, R>(T value, Func<T, R> mapping) {
            return mapping(value);
        }
        public static int factorial(int value)
        {
            return value == 1 ? 1 : factorial(value - 1) * value;
        }
    }
}