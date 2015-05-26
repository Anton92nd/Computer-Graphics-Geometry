using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_G
{
	public class Point2
	{
		public readonly double x, y;

		public Point2()
		{
			x = y = 0;
		}

		public Point2(double x, double y)
		{
			this.x = x;
			this.y = y;
		}

		public static Point2 operator +(Point2 a, Point2 b)
		{
			return new Point2(a.x + b.x, a.y + b.y);
		}

		public static Point2 operator -(Point2 a, Point2 b)
		{
			return new Point2(a.x - b.x, a.y - b.y);
		}

		public static double operator *(Point2 a, Point2 b)
		{
			return a.x*b.y - b.x*a.y;
		}

		public static double operator %(Point2 a, Point2 b)
		{
			return a.x*b.x + a.y*b.y;
		}

		public static Point2 operator *(Point2 v, double k)
		{
			return new Point2(v.x*k, v.y*k);
		}

		public static Point2 operator /(Point2 v, double k)
		{
			return new Point2(v.x/k, v.y/k);
		}

		public double Length()
		{
			return Math.Sqrt(x*x + y*y);
		}
	}
}
