using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace CG_G
{
	class Point3
	{
		public readonly double x, y, z;

		public Point3()
		{
			x = y = 0;
		}

		public Point3(double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public static Point3 operator +(Point3 a, Point3 b)
		{
			return new Point3(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		public static Point3 operator -(Point3 a, Point3 b)
		{
			return new Point3(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		public static Point3 operator *(Point3 a, Point3 b)
		{
			return new Point3(
				a.y * b.z - a.z * b.y,
				a.z * b.x - a.x * b.z,
				a.x * b.y - a.y * b.x
				);
		}

		public static double operator %(Point3 a, Point3 b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}

		public static Point3 operator *(Point3 v, double k)
		{
			return new Point3(v.x * k, v.y * k, v.z * k);
		}

		public static Point3 operator /(Point3 v, double k)
		{
			return new Point3(v.x / k, v.y / k, v.z / k);
		}

		public double Length()
		{
			return Math.Sqrt(x * x + y * y + z * z);
		}

		public Point3 Rotate(Point3 axe, double angle)
		{
			var turned = this*axe;
			return (this * Math.Cos(angle)) + (turned * Math.Sin(angle));
		}
	}
}
