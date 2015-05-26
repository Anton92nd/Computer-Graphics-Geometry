using System;
using System.Drawing;
using System.Windows.Forms;

namespace CG_G
{
	public partial class Task6 : Form
	{
		private const double LeftX = -8.0, RightX = 8.0, LowY = -8.0, TopY = 8.0;
		private readonly double RadiusLower, RadiusHigher, AxeLength;
		private readonly Point2 Lower, Higher;
		private Point3 lowerRadius;

		public Task6(double radiusLower, double radiusHigher, double axeLength, Point2 lower, Point2 higher)
		{
			RadiusLower = radiusLower;
			RadiusHigher = radiusHigher;
			AxeLength = axeLength;
			Lower = lower;
			Higher = higher;
			InitializeComponent();
			pictureBox1.Image = DrawPicture();
		}

		private void DrawAxes(Graphics g, int size)
		{
			var oX = Convert.ToInt32((0 - LowY) / (TopY - LowY) * size);
			var oY = Convert.ToInt32((0 - LeftX) / (RightX - LeftX) * size);
			g.FillRectangle(Brushes.White, 0, 0, size, size);
			g.DrawLine(Pens.Black, oY, oX, oY, 0);
			g.DrawLine(Pens.Black, oY, oX, size, oX);
			g.DrawLine(Pens.Black, oY, oX, oY - (size - oX), size);
		}

		private Point2 ConvertToPlane(Point3 point)
		{
			return new Point2(-point.x / (2 * Math.Sqrt(2)) + point.y,
							point.x / (2 * Math.Sqrt(2)) - point.z);
		}

		private Point ConvertToScreen(int size, Point2 point)
		{
			return new Point(
				Convert.ToInt32((point.x - LeftX) / (RightX - LeftX) * size),
				Convert.ToInt32((point.y - LowY) / (TopY - LowY) * size));
		}

		private void DrawLowerCylinder(Bitmap bmp, Graphics g)
		{
			var pen = Pens.Green;
			var size = bmp.Height;
			const double angle = Math.PI / 2.0;
			const int anglePartsCount = 7;
			const int heightPartsCount = 10;
			var A = new Point3(AxeLength, 0, 0);
			var B = new Point3(0, 0, 0);
			var v = new Point3(0, RadiusLower, 0);
			var axe = new Point3(-1, 0, 0);
			for (var i = 0; i <= anglePartsCount; i++)
			{
				var cur = v.Rotate(axe, (angle / anglePartsCount) * i);
				var A1 = A + cur;
				var B1 = B + cur;
				var planeA1 = ConvertToPlane(A1);
				var planeB1 = ConvertToPlane(B1);
				var screenA1 = ConvertToScreen(size, planeA1);
				var screenB1 = ConvertToScreen(size, planeB1);
				g.DrawLine(pen, screenA1, screenB1);
			}
			var screenRadius = ConvertToScreen(size, ConvertToPlane(v)).X - size / 2;
			for (var i = 0; i <= heightPartsCount; i++)
			{
				var C = A + ((B - A) / heightPartsCount * i);
				var screenC = ConvertToScreen(size, ConvertToPlane(C));
				g.DrawArc(pen, (float)(screenC.X - screenRadius), (float)(screenC.Y - screenRadius), (float)screenRadius * 2, (float)screenRadius * 2, -90F, (float)(angle * 180.0 / Math.PI));
			}
		}

		private void DrawHigherCylinder(Bitmap bmp, Graphics g)
		{
			var pen = Pens.Red;
			var lowerRad = new Point2(lowerRadius.y, lowerRadius.z);
			var higherRad = lowerRad.Resize(RadiusHigher);
			var deltaRad = new Point3(0, lowerRad.x, lowerRad.y) - new Point3(0, higherRad.x, higherRad.y);
			var deltaX = Lower.x - Higher.x;
			var A = new Point3(AxeLength + deltaX, 0, 0) + deltaRad;
			var B = new Point3(deltaX, 0, 0) + deltaRad;
			higherRad = higherRad.Rotate(-highAngle);
			const int anglePartsCount = 9;
			const int heightPartsCount = 10;
			const double angle = Math.PI / 2.0;
			var size = bmp.Height;
			for (var i = 0; i <= anglePartsCount; i++)
			{
				var currentV = higherRad.Rotate(i*(angle/anglePartsCount));
				var A1 = A + new Point3(0, currentV.x, currentV.y);
				var B1 = B + new Point3(0, currentV.x, currentV.y);
				var planeA1 = ConvertToPlane(A1);
				var planeB1 = ConvertToPlane(B1);
				var screenA1 = ConvertToScreen(size, planeA1);
				var screenB1 = ConvertToScreen(size, planeB1);
				g.DrawLine(pen, screenA1, screenB1);
			}
			var v = new Point3(0, RadiusHigher, 0);
			var screenRadius = ConvertToScreen(size, ConvertToPlane(v)).X - size / 2;
			var startAngle = -ToDegrees(Math.Atan2(higherRad.y, higherRad.x));
			for (var i = 0; i <= heightPartsCount; i++)
			{
				var C = A + ((B - A) / heightPartsCount * i);
				var screenC = ConvertToScreen(size, ConvertToPlane(C));
				g.DrawArc(pen, (float)(screenC.X - screenRadius), (float)(screenC.Y - screenRadius), (float)screenRadius * 2, (float)screenRadius * 2, startAngle, ToDegrees(-angle));
			}
		}

		private float ToDegrees(double angleInRadians)
		{
			return Convert.ToSingle(angleInRadians*180.0/Math.PI);
		}

		private Bitmap DrawPicture()
		{
			CalculatePointsOnCylinders();
			var bmp = new Bitmap(pictureBox1.Width, pictureBox1.Width);
			var g = Graphics.FromImage(bmp);
			DrawAxes(g, bmp.Height);
			DrawLowerCylinder(bmp, g);
			DrawHigherCylinder(bmp, g);
			return bmp;
		}

		private double lowAngle, highAngle;

		private void CalculatePointsOnCylinders()
		{
			lowAngle = Math.Acos(Lower.y/RadiusLower);
			lowerRadius = new Point3(0, Lower.y, RadiusLower*Math.Sin(lowAngle));
			highAngle = Math.Acos(Higher.y/RadiusHigher);
		}
	}
}
