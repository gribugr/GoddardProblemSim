// using NUnit.Framework;
//
// namespace Goddard.Core.Tests
// {
//     [TestFixture]
//     public class MassInvariantTests
//     {
//         [Test]
//         public void Mass_IsMonotonicallyNonIncreasing()
//         {
//             var p = new SimParams();
//             double[] y0 = { p.H0, 0.0, p.M0 };
//             var (T, Y) = Integrator.Solve(y0, tEnd: 50.0, steps: 2000,
//                 uOfT: _ => p.UMax, p);
//
//             for (int i = 1; i < Y.Length; i++)
//                 Assert.That(Y[i][2], Is.LessThanOrEqualTo(Y[i - 1][2] + 1e-12),
//                     $"масса выросла на шаге {i}");
//         }
//
//         [Test]
//         public void Mass_NeverBelowDryMass()
//         {
//             var p = new SimParams();
//             double[] y0 = { p.H0, 0.0, p.M0 };
//             var (T, Y) = Integrator.Solve(y0, tEnd: 60.0, steps: 3000,
//                 uOfT: _ => p.UMax, p);
//
//             foreach (var y in Y)
//                 Assert.That(y[2], Is.GreaterThanOrEqualTo(p.Mf - 1e-12));
//         }
//
//         [Test]
//         public void FullBurn_ReachesExactlyDryMass()
//         {
//             var p = new SimParams();
//             double[] y0 = { p.H0, 0.0, p.M0 };
//             var (T, Y) = Integrator.Solve(y0, tEnd: 60.0, steps: 3000,
//                 uOfT: _ => p.UMax, p);
//
//             Assert.That(Y[^1][2], Is.EqualTo(p.Mf).Within(1e-9),
//                 "при постоянном UMax топливо должно выгореть до сухой массы");
//         }
//     }
// }