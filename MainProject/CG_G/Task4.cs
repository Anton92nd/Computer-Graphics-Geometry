using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CG_G
{
	public partial class Task4 : Form
	{
		public Task4()
		{
			InitializeComponent();
		}

		private List<Point2> polygon = new List<Point2>();
		private Point2 O, h, w;
		private List<Point2[]> rectangle;

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

		private void ReadDataFromFile()
		{
			var lines = File.ReadAllLines(textBox1.Text).ToList();
			var n = int.Parse(lines[0]);
			lines = lines.Skip(1).ToList();
			polygon = lines.Take(n)
				.Select(s =>
				{
					var xy = s.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries).Select(Double.Parse).ToList();
					return new Point2(xy[0], xy[1]);
				}).ToList();
			var remains = lines.Skip(n).Aggregate("", (sum, s) => sum + " " + s);
			var lst = remains.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries).Select(Double.Parse).ToList();
			polygon.Add(polygon[0]);
			O = new Point2(lst[0], lst[1]);
			h = new Point2(lst[2], lst[3]);
			w = new Point2(lst[4], lst[5]);
			rectangle = new List<Point2[]>
			{
				new[] {O, O + h},
				new[] {O, O + w},
				new[] {O + w, O + w + h},
				new[] {O + h, O + w + h}
			}; 
		}

		private bool PointInside(Point2 v)
		{
			return DoubleLessOrEqual(0, (v - O) % h) && DoubleLessOrEqual((v - O) % h, h % h)
				&& DoubleLessOrEqual(0, (v - O) % w) && DoubleLessOrEqual((v - O) % w, w % w);
		}

		private bool Intersection(Point2 A, Point2 B, Point2 C, Point2 D, out Point2 result)
		{
			if (DoubleLessOrEqual(((B - A)*(D - A))*((B - A)*(C - A)), 0) &&
			    DoubleLessOrEqual(((D - C)*(A - C))*((D - C)*(B - C)), 0))
			{
				double sADC = Math.Abs((D - A)*(C - A));
				double sBDC = Math.Abs((D - B)*(C - B));
				result = A + ((B - A)*(sADC/(sADC + sBDC)));
				return true;
			}
			result = null;
			return false;
		}

		private Point2 GetIntersection(Point2 A, Point2 B)
		{
			foreach (var segment in rectangle)
			{
				Point2 result;
				if (Intersection(A, B, segment[0], segment[1], out result))
				{
					return result;
				}
			}
			return null;
		}

		List<Point2[]> FindInsideSegments()
		{
			var result = new List<Point2[]>();
			var lastInside = PointInside(polygon[0]);
			for (var i = 0; i < polygon.Count - 1; i++)
			{
				var nextInside = PointInside(polygon[i + 1]);
				if (nextInside && lastInside)
				{
					result.Add(new [] {polygon[i], polygon[i + 1] });
				}
				if (nextInside != lastInside)
				{
					var intersectPoint = GetIntersection(polygon[i], polygon[i + 1]);
					result.Add(lastInside ? new [] {polygon[i], intersectPoint} : new[] {intersectPoint, polygon[i + 1]});
				}
				lastInside = nextInside;
			}
			return result;
		}

		private Point ConvertToScreen(Point2 pointInRectange, int screenWidth, int screenHeight)
		{
			return new Point(Convert.ToInt32(pointInRectange.x / w.Length() * screenWidth), 
				screenHeight - Convert.ToInt32(pointInRectange.y / h.Length() * screenHeight));
		}

		private void DrawAxes(Bitmap bmp, Graphics g)
		{
			for (var x = 0;; x++)
			{
				var xScreen = Convert.ToInt32(x/w.Length()*bmp.Width);
				if (xScreen >= bmp.Width)
				{
					break;
				}
				g.DrawLine(Pens.Black, xScreen, bmp.Height - 4, xScreen, bmp.Height - 1);
			}
			for (var y = 0; ; y++)
			{
				var yScreen = bmp.Height - 1 - Convert.ToInt32(y / h.Length() * bmp.Height);
				if (yScreen < 0)
				{
					break;
				}
				g.DrawLine(Pens.Black, 0, yScreen, 3, yScreen);
			}
		}

		private Bitmap DrawImage()
		{
			var bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
			ReadDataFromFile();
			var g = Graphics.FromImage(bmp);
			g.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);
			DrawAxes(bmp, g);
			var segments = FindInsideSegments();
			var newOx = w/w.Length();
			var newOy = h/h.Length();
			var segmentsInRectange = segments.Select(s => new[]
				{
					new Point2((s[0] - O)%newOx, (s[0] - O)%newOy),
					new Point2((s[1] - O)%newOx, (s[1] - O)%newOy)
				}).Select(s => new[]
				{
					ConvertToScreen(s[0], bmp.Width, bmp.Height),
					ConvertToScreen(s[1], bmp.Width, bmp.Height)
				}).ToList();
			foreach (var segment in segmentsInRectange)
			{
				g.DrawLine(Pens.Blue, segment[0], segment[1]);
			}
			return bmp;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			var openDialog = new OpenFileDialog();
			if (openDialog.ShowDialog() == DialogResult.OK)
			{
				button1.Enabled = true;
				textBox1.Text = openDialog.FileName;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			panel1.Visible = false;
			pictureBox1.Dock = DockStyle.Fill;
			pictureBox1.Visible = true;
			pictureBox1.Image = DrawImage();
		}

		private void Task4_ResizeEnd(object sender, EventArgs e)
		{
			pictureBox1.Image = DrawImage();
		}
	}
}
