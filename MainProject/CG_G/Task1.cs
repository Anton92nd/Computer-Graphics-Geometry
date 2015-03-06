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
	public partial class Task1 : Form
	{
		public Task1()
		{
			InitializeComponent();
		}

		private void Task1_Load(object sender, EventArgs e)
		{
		}

		private void GetParameters(out Point2 A, out Point2 B, out double a, out double b, out double c)
		{
			A = new Point2(double.Parse(textBox1.Text), double.Parse(textBox2.Text));
			B = new Point2(double.Parse(textBox3.Text), double.Parse(textBox4.Text));
			a = double.Parse(textBox5.Text);
			b = double.Parse(textBox6.Text);
			c = double.Parse(textBox7.Text);
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

		private List<Point2> GetIntersections(Point2 A, Point2 B, double a, double b, double c)
		{
			var result = new List<Point2>();
			var x1 = (-b * A.y - c) / a;
			if (DoubleLessOrEqual(A.x, x1) && DoubleLessOrEqual(x1, B.x))
			{
				result.Add(new Point2(x1, A.y));
			}
			var x2 = (-b * B.y - c) / a;
			if (DoubleLessOrEqual(A.x, x2) && DoubleLessOrEqual(x2, B.x))
			{
				result.Add(new Point2(x2, B.y));
			}
			var y1 = (-a * A.x - c) / b;
			if (DoubleLessOrEqual(A.y, y1) && DoubleLessOrEqual(y1, B.y))
			{
				result.Add(new Point2(A.x, y1));
			}
			var y2 = (-a * B.x - c) / b;
			if (DoubleLessOrEqual(A.y, y2) && DoubleLessOrEqual(y2, B.y))
			{
				result.Add(new Point2(B.x, y2));
			}
			return result;
		}

		private void DrawLine(Bitmap bmp, Point A, Point B)
		{
			var turn = Math.Abs(A.Y - B.Y) > Math.Abs(A.X - B.X);
			if (turn)
			{
				A = new Point(A.Y, A.X);
				B = new Point(B.Y, B.X);
			}
			if (A.X > B.X)
			{
				Point temp = A;
				A = B;
				B = temp;
			}
			int dx = B.X - A.X;
			int dy = Math.Abs(B.Y - A.Y);
			int error = dx / 2;
			int ystep = (A.Y < B.Y) ? 1 : -1;
			int y = A.Y;
			for (int x = A.X; x < B.X; x++)
			{
				int x1 = turn ? y : x, y1 = turn ? x : y;
				if (x1 > 0 && x1 < bmp.Width && y1 > 0 && y1 < bmp.Height)
				{
					bmp.SetPixel(x1, y1, Color.Blue);
				}
				error -= dy;
				if (error < 0)
				{
					y += ystep;
					error += dx;
				}
			}
		}

		private Bitmap DrawImage()
		{
			var bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
			Console.WriteLine(bmp.Width + " " + bmp.Height);
			var g = Graphics.FromImage(bmp);
			g.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height); Point2 A, B;
			double a, b, c;
			GetParameters(out A, out B, out a, out b, out c);
			if (DoubleEqual(a, 0.0) || DoubleEqual(b, 0.0))
			{
				if (DoubleEqual(a, 0.0) && DoubleEqual(b, 0.0))
				{
					return bmp;
				}
				if (DoubleEqual(a, 0.0))
				{
					double y = -c / b;
					if (DoubleLessOrEqual(y, A.y) || DoubleLessOrEqual(B.y, y))
					{
						return bmp;
					}
					int yy = Convert.ToInt32(bmp.Height * (y - A.y) / (B.y - A.y));
					for (int xx = 0; xx < bmp.Width; xx++)
					{
						bmp.SetPixel(xx, yy, Color.Blue);
					}
					return bmp;
				}
				double x = -c / a;
				if (DoubleLessOrEqual(x, A.x) || DoubleLessOrEqual(B.x, x))
				{
					return bmp;
				}
				int xxx = Convert.ToInt32(bmp.Width * (x - A.x) / (B.x - A.x));
				for (int yy = 0; yy < bmp.Height; yy++)
				{
					bmp.SetPixel(xxx, yy, Color.Black);
				}
				return bmp;
			}
			var intersections = GetIntersections(A, B, a, b, c);
			if (intersections.Count < 2)
			{
				return bmp;
			}
			var A1 = new Point(Convert.ToInt32(bmp.Width * (intersections[0].x - A.x) / (B.x - A.x)),
				Convert.ToInt32(bmp.Height * (B.y - intersections[0].y) / (B.y - A.y)));
			var B1 = new Point(Convert.ToInt32(bmp.Width * (intersections[1].x - A.x) / (B.x - A.x)),
				Convert.ToInt32(bmp.Height * (B.y - intersections[1].y) / (B.y - A.y)));
			DrawLine(bmp, A1, B1);
			return bmp;
		}

		private void DrawDiagonal(Bitmap bmp, Point A, Point B)
		{
			throw new NotImplementedException();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			panel1.Visible = false;
			pictureBox1.Dock = DockStyle.Fill;
			pictureBox1.Visible = true;
			pictureBox1.Image = DrawImage();
		}

		private void Task1_ResizeEnd(object sender, EventArgs e)
		{
			pictureBox1.Image = DrawImage();
		}
	}
}
