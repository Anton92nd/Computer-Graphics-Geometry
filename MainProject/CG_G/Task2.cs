using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CG_G
{
	public partial class Task2 : Form
	{
		public Task2()
		{
			InitializeComponent();
		}

		private const double eps = 1e-9;

		public bool DoubleEqual(double a, double b)
		{
			return Math.Abs(a - b) < eps;
		}

		public bool DoubleLess(double a, double b)
		{
			return a < b && !DoubleEqual(a, b);
		}

		public bool DoubleLessOrEqual(double a, double b)
		{
			return a < b || DoubleEqual(a, b);
		}

		private double ConvertFromScreenToPlot(int xScreen, double x1, double x2, int screenSize)
		{
			return x1 + (x2 - x1) * xScreen / screenSize;
		}

		private int ConvertFromPlotToScreen(double x, double x1, double x2, int screenSize)
		{
			return Convert.ToInt32(screenSize * (x - x1) / (x2 - x1));
		}

		private void DrawOXY(Bitmap bmp, Graphics g, double x1, double x2, double y1, double y2)
		{
			g.DrawLine(Pens.Black, 0, bmp.Height / 2, bmp.Width, bmp.Height / 2);
			g.DrawLine(Pens.Black, bmp.Width / 2, 0, bmp.Width / 2, bmp.Height);
			var screenOX = bmp.Height / 2;
			var screenOY = bmp.Width / 2;
			for (int xPlot = 1; ; xPlot++)
			{
				var xScreen = ConvertFromPlotToScreen(xPlot, x1, x2, bmp.Width);
				if (xScreen >= bmp.Width)
				{
					break;
				}
				g.DrawLine(Pens.Black, xScreen, screenOX - 2, xScreen, screenOX + 2);
				xScreen = ConvertFromPlotToScreen(-xPlot, x1, x2, bmp.Width);
				if (xScreen <= 0)
				{
					break;
				}
				g.DrawLine(Pens.Black, xScreen, screenOX - 2, xScreen, screenOX + 2);
			}
			for (int yPlot = 1; ; yPlot++)
			{
				var yScreen = ConvertFromPlotToScreen(yPlot, y1, y2, bmp.Height);
				if (yScreen >= bmp.Height)
				{
					break;
				}
				g.DrawLine(Pens.Black, screenOY - 2, yScreen, screenOY + 2, yScreen);
				yScreen = ConvertFromPlotToScreen(-yPlot, y1, y2, bmp.Height);
				if (yScreen <= 0)
				{
					break;
				}
				g.DrawLine(Pens.Black, screenOY - 2, yScreen, screenOY + 2, yScreen);
			}
		}

		private void GetParameters(out double a, out double b)
		{
			a = double.Parse(textBox1.Text);
			b = double.Parse(textBox2.Text);
		}

		private Bitmap DrawImage()
		{
			var bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
			Console.WriteLine(bmp.Width + " " + bmp.Height);
			double a, b;
			GetParameters(out a, out b);
			double c = Math.Sqrt(a*a + b*b);
			double x1 = -3*c, x2 = 3*c, y1 = -3*c, y2 = 3*c;
			var g = Graphics.FromImage(bmp);
			g.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);
			DrawOXY(bmp, g, x1, x2, y1, y2);
			var screenOX = bmp.Height/2;
			var screenOY = bmp.Width/2;
			for (var xScreen = screenOY; xScreen < bmp.Width; xScreen++)
			{
				var xPlot = ConvertFromScreenToPlot(xScreen, x1, x2, bmp.Width);
				var yPlot = b*Math.Sqrt(1 + xPlot*xPlot/(a*a));
				var yScreen = bmp.Height - ConvertFromPlotToScreen(yPlot, y1, y2, bmp.Height);
				if (yScreen <= 0)
					break;
				var v = new Point(xScreen - screenOY, yScreen - screenOX);
				bmp.SetPixel(screenOY + v.X, screenOX + v.Y, Color.Blue);
				bmp.SetPixel(screenOY + v.X, screenOX - v.Y, Color.Blue);
				bmp.SetPixel(screenOY - v.X, screenOX + v.Y, Color.Blue);
				bmp.SetPixel(screenOY - v.X, screenOX - v.Y, Color.Blue);
			}
			return bmp;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			panel1.Visible = false;
			pictureBox2.Dock = DockStyle.Fill;
			pictureBox2.Visible = true;
			pictureBox2.Image = DrawImage();
		}

		private void Task2_ResizeEnd(object sender, EventArgs e)
		{
			pictureBox2.Image = DrawImage();
		}
	}
}
