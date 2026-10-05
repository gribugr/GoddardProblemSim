using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.OdeSolvers;

namespace Goddard.Core
{
    /// <summary>
    /// Интегрирует ОДУ задачи Годдарда методом Рунге–Кутты 4-го порядка
    /// (Math.NET, фиксированный шаг). Наивная версия v1: один вызов решателя
    /// на весь интервал, без контроля инвариантов между шагами.
    ///
    /// Известный дефект (демонстрируется тестами MassInvariantTests):
    /// ограничение m >= Mf проверяется только в правой части Dynamics.F —
    /// на шаге, пересекающем сухую массу, прирост «перепрыгивает» границу,
    /// после чего тяга отключается и масса навсегда остаётся чуть ниже Mf.
    /// Исправление — пошаговое интегрирование с проекцией (версия v2).
    /// </summary>
    public static class Integrator
    {
        /// <param name="y0">Начальное состояние (h, v, m), безразмерные единицы COPS.</param>
        /// <param name="tEnd">Конец интервала; интегрирование всегда от t = 0.</param>
        /// <param name="steps">
        /// Число узлов выходной сетки. По документации Math.NET это размер
        /// выходного массива («the larger, the finer»);
        /// шаг dt = tEnd / (steps - 1).
        /// </param>
        /// <param name="uOfT">Закон управления: расход массы u(t), 0 &lt;= u &lt;= p.UMax.</param>
        /// <param name="p">Параметры постановки.</param>
        /// <returns>
        /// Кортеж из двух массивов равной длины:
        /// T — равномерная сетка времён (T[i] = i·dt);
        /// Y — состояния в узлах, Y[i] = (h, v, m) в момент T[i].
        /// Внимание: инвариант Y[i][2] &gt;= p.Mf здесь НЕ гарантирован.
        /// </returns>
        public static (double[] T, double[][] Y) Solve(
            double[] y0, double tEnd, int steps,
            Func<double, double> uOfT, SimParams p)
        {
            // Конвертации double[] <-> Vector<double> неизбежны и сделаны намеренно:
            // Dynamics.F работает на «чистых» double[] — ядро модели не зависит
            // от типов Math.NET в сигнатуре; решатель же требует Vector<double>.
            // Адаптер между мирами — локальная функция F.
            Vector<double> F(double t, Vector<double> y) =>
                Vector<double>.Build.DenseOfArray(
                    Dynamics.F(t, y.ToArray(), uOfT(t), p));

            // Один вызов на весь интервал: решатель возвращает Vector<double>[] —
            // массив состояний по узлам сетки, первый элемент — y0.
            Vector<double>[] solVectors = RungeKutta.FourthOrder(
                Vector<double>.Build.DenseOfArray(y0), 0.0, tEnd, steps, F);

            // Наружу отдаём double[][]: потребителям (Unity, тесты)
            // не нужно знать про типы Math.NET.
            double[][] Y = solVectors.Select(v => v.ToArray()).ToArray();

            // Сетку строим по фактической длине Y, а не по steps: контракт
            // «T и Y одинаковой длины» не зависит от деталей реализации библиотеки.
            double dt = tEnd / (steps - 1.0);
            double[] T = Enumerable.Range(0, Y.Length)
                                   .Select(i => i * dt)
                                   .ToArray();

            return (T, Y);
        }
    }
}