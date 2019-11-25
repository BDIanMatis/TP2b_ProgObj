using System;
using static System.Math;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Thésée
{
   static class Algos
   {
      public static double Distance(this Point p0, Point p1) =>
         Sqrt(Pow(p0.X - p1.X, 2) + Pow(p0.Y - p1.Y, 2));
      public static bool EstEntre<T>(T val, T a, T b)
         where T : IComparable =>
            a.CompareTo(val) <= 0 && val.CompareTo(b) <= 0;
      public static void Permuter<T>(ref T a, ref T b)
      {
         T temp = a;
         a = b;
         b = temp;
      }
   }
}
