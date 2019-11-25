using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Thésée
{
   public struct Point
   {
      public int X { get; set; }
      public int Y { get; set; }
      public Point(int x, int y)
      {
         X = x;
         Y = y;
      }
      public override bool Equals(object other) =>
         other is Point pt && Equals(pt);
      public bool Equals(Point p) =>
         X == p.X && Y == p.Y;
      public override int GetHashCode() =>
         base.GetHashCode(); // bof
      public static Point operator +(Point p0, Point p1) =>
         new Point(p0.X + p1.X, p0.Y + p1.Y);
      public static Point operator -(Point p) =>
         new Point(-p.X, -p.Y);
      public static bool operator ==(Point p0, Point p1) =>
         p0.Equals(p1);
      public static bool operator !=(Point p0, Point p1) =>
         !(p0 == p1);
   }
}
