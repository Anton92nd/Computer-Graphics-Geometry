using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_G
{
	class MainGraphics
	{
		[STAThread]
		static void Main(string[] args)
		{
			var form = new MainForm();
			form.ShowDialog();
		}
	}
}
