// using System;
// using System.Collections;
// using NUnit.Framework;
// using UnityEngine.TestTools;
//
// namespace Goddard.Core.Tests
// {
//     [TestFixture]
//     public class TsiolkovskyTests
//     {
//         [Test]
//         public void VacuumNoGravity_MatchesTsiolkovsky()
//         {
//             var p = new SimParams { EnableDrag = false, EnableGravity = false };
//             double[] y0 = { 0.0, 0.0, p.M0 };
//             var (T, Y) = Integrator.Solve(y0, tEnd: 50.0, steps: 5000,
//                 uOfT: _ => p.UMax, p);
//
//             int burnout = Array.FindLastIndex(Y, y => y[2] > p.Mf + 1e-9);
//             double vNumeric = Y[burnout][1];
//             double vAnalytic = p.C * Math.Log(p.M0 / p.Mf);
//
//             Assert.That(vNumeric, Is.EqualTo(vAnalytic).Within(0.5).Percent);
//
//         }
//
//     }
// }
