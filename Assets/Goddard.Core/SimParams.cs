
using System;

namespace Goddard.Core
{
    /// <summary>
    /// Параметры постановки COPS, безразмерная нормировка.
    /// </summary>
    public record SimParams
    {
        public double H0 = 1.0;
        public double G0 = 1.0;
        public double Hc = 500.0;
        public double Vc = 620.0;
        public double M0 = 3.0; // стартовая масса
        public double Mf = 1.0; // сухая масса
        public double UMax = 3.5; // max расход
        
        public bool EnableDrag = true;
        public bool EnableGravity = true;
        
        /// <summary>
        /// Эффективная скорость истечения c.
        /// </summary>
        public double C => 0.5 * Math.Sqrt(G0 * H0) * Vc;
        
        /// <summary>
        /// Коэффициент сопротивления Dc.
        /// </summary>
        public double Dc => 0.5 * Vc * M0 / G0;
    }
}
