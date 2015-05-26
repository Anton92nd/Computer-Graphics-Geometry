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
	public partial class Task6 : Form
	{
		private readonly double RadiusLower, RadiusHigher, AxeLength;
		private readonly Point2 Lower, Higher;

		public Task6(double radiusLower, double radiusHigher, double axeLength, Point2 lower, Point2 higher)
		{
			RadiusLower = radiusLower;
			RadiusHigher = radiusHigher;
			AxeLength = axeLength;
			Lower = lower;
			Higher = higher;
			InitializeComponent();
		}
	}
}
