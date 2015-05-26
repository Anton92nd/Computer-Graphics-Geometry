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

		private const double leftX = -8.0, rightX = 8.0, lowY = -11, topY = 5;

		private void Task5_Load(object sender, EventArgs e)
		{
			var size = Math.Min(pictureBox1.Height, pictureBox1.Width);
			pictureBox1.Image = DrawImage(size);
		}

		private void DrawAxes(Graphics g, int size)
		{
			var oX = Convert.ToInt32((0-lowY)/(topY - lowY)*size);
			var oY = Convert.ToInt32((0 - leftX)/(rightX - leftX)*size);
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
			const double radius = 3.0;
			const int anglePartsCount = 7;
			const int heightPartsCount = 10;
			var A = new Point3(12, 0, 0);
			var B = new Point3(0, 0, 0);
			var v = new Point3(0, radius, 0);
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
			const double radius = 5.0;
			const int anglePartsCount = 9;
			const int heightPartsCount = 10;
			var A = new Point3(12, 0, 6);
			var B = new Point3(0, 0, 6);
			var v = new Point3(0, radius, 0);
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
				Convert.ToInt32((point.x - leftX) / (rightX - leftX) * size), 
				Convert.ToInt32((point.y - lowY) / (topY - lowY) * size));
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var task6 = new Task6();
			task6.Show();
		}
	}
}
