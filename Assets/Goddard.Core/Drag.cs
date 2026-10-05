using System;

namespace Goddard.Core
{
    /// <summary>
    /// Аэродинамическое сопротивление: квадратично по скорости,
    /// экспоненциальная атмосфера по высоте.
    /// </summary>
    public static class Drag
    {
        public static double D(double h, double v, SimParams p)
            => p.Dc * v * v * Math.Exp(-p.Hc * (h - p.H0) / p.H0);
    }
}