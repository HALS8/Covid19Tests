using System;

namespace Covid19.Tests
{
    public class HelperMethods
    {
        private static readonly Random _r = new Random();
        private static readonly DateTime _today = DateTime.Today;

        public static int RandYear(int bracket)
        {
            return bracket switch
            {
                1 => _r.Next(_today.Year - 17, _today.Year - 13),
                2 => _r.Next(_today.Year - 34, _today.Year - 18),
                3 => _r.Next(_today.Year - 49, _today.Year - 35),
                4 => _r.Next(_today.Year - 64, _today.Year - 50),
                5 => _r.Next(_today.Year - 79, _today.Year - 65),
                6 => _r.Next(_today.Year - 120, _today.Year - 80),
                _ => _today.Year,
            };
        }

        public static int RandMonth()
        {
            return _r.Next(1, _today.Month);
        }

        public static int RandDay(int month)
        {
            return month < _today.Month ? _r.Next(1, 28) : _today.Day;
        }
    }
}
