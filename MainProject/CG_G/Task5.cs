using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CG_G
{
	public partial class Task5 : Form
	{
		public Task5()
		{
			InitializeComponent();
			pictureBox1.Dock = DockStyle.Fill;
		}

		private const double LeftX = -8.0, RightX = 8.0, LowY = -11, TopY = 5;
		private const double RadiusLower = 3.0, RadiusHigher = 5.0, AxeLength = 12.0;

		private void Task5_Load(object sender, EventArgs e)
		{
			var size = Math.Min(pictureBox1.Height, pictureBox1.Width);
			pictureBox1.Image = DrawImage(size);
		}

		private void DrawAxes(Graphics g, int size)
		{
			var oX = Convert.ToInt32((0-LowY)/(TopY - LowY)*size);
			var oY = Convert.ToInt32((0 - LeftX)/(RightX - LeftX)*size);
			g.FillRectangle(Brushes.White, 0, 0, size, size);
			g.DrawLine(Pens.Black, oY, oX, oY, 0);
			g.DrawLine(Pens.Black, oY, oX, size, oX);
			g.DrawLine(Pens.Black, oY, oX, oY - (size - oX), size);
		}

		private Bitmap DrawImage(int size)
		{
			var bmp = new Bitmap(size, size);
			var g = Graphics.FromImage(bmp);
			DrawAxes(g, size);
			DrawLowerCylinder(bmp, g);
			DrawHigherCylinder(bmp, g);
			return bmp;
		}

		private void DrawLowerCylinder(Bitmap bmp, Graphics g)
		{
			var pen = Pens.Green;
			var size = bmp.Height;
			const double angle = Math.PI/2.0;
			const int anglePartsCount = 7;
			const int heightPartsCount = 10;
			var A = new Point3(AxeLength, 0, 0);
			var B = new Point3(0, 0, 0);
			var v = new Point3(0, RadiusLower, 0);
			var axe = new Point3(-1, 0, 0);
			for (var i = 0; i <= anglePartsCount; i++)
			{
				var cur = v.Rotate(axe, angle/anglePartsCount*i);
				var A1 = A + cur;
				var B1 = B + cur;
				var planeA1 = ConvertToPlane(A1);
				var planeB1 = ConvertToPlane(B1);
				var screenA1 = ConvertToScreen(size, planeA1);
				var screenB1 = ConvertToScreen(size, planeB1);
				g.DrawLine(pen, screenA1, screenB1);
			}
			var screenRadius = ConvertToScreen(size, ConvertToPlane(v)).X - size/2;
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
			var size = bmp.Height;
			const double angle = Math.PI / 2.0;
			const int anglePartsCount = 9;
			const int heightPartsCount = 10;
			var A = new Point3(AxeLength, 0, 6);
			var B = new Point3(0, 0, 6);
			var v = new Point3(0, RadiusHigher, 0);
			var axe = new Point3(-1, 0, 0);
			for (var i = 0; i <= anglePartsCount; i++)
			{
				var cur = v.Rotate(axe, angle / anglePartsCount * i);
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

		private void button1_Click(object sender, EventArgs e)
		{
			label7.Text = "";
			double xl, yl, xh, yh;
			if (!double.TryParse(textBox1.Text, out xh) || xh < 0.0 || xh > AxeLength)
			{
				label7.Text = @"Неверное описание x-координаты верхнего цилиндра";
				return;
			}
			if (!double.TryParse(textBox2.Text, out yh) || yh < 0.0 || yh > RadiusHigher)
			{
				label7.Text = @"Неверное описание y-координаты верхнего цилиндра";
				return;
			}
			if (!double.TryParse(textBox3.Text, out xl) || xl < 0.0 || xl > AxeLength)
			{
				label7.Text = @"Неверное описание x-координаты нижнего цилиндра";
				return;
			}
			if (!double.TryParse(textBox4.Text, out yl) || yl < 0.0 || yl > RadiusLower)
			{
				label7.Text = @"Неверное описание y-координаты нижнего цилиндра";
				return;
			}
			var task6 = new Task6(RadiusLower, RadiusHigher, AxeLength, new Point2(xl, yl), new Point2(xh, yh));
			task6.Show();
		}
	}
}
