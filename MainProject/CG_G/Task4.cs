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
	public partial class Task4 : Form
	{
		public Task4()
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
		
		

		private Bitmap DrawImage()
		{
			var bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
			Console.WriteLine(bmp.Width + " " + bmp.Height);
			double a, b, c;
			GetParameters(out a, out b, out c);
			var halfWidth = Math.Abs(c) + 10.0;
			DrawFunctionPlot(bmp, x => Math.Sin(a * x + b) / (x - c), -halfWidth, halfWidth, -5.0, 5.0);
			return bmp;
		}
	}
}
