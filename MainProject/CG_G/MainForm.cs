﻿using System;
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
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			buttons.AddRange(new [] {button1, button2, button3});
		}

		private List<Button> buttons = new List<Button>(); 

		private void DisableAllButtons()
		{
			foreach (var b in buttons)
			{
				b.Enabled = false;
			}
		}

		private void EnableAllButtons()
		{
			foreach (var b in buttons)
			{
				b.Enabled = true;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var task1 = new Task1();
			task1.Show();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			var task2 = new Task3();
			task2.Show();
		}

		private void button4_Click(object sender, EventArgs e)
		{
			var task4 = new Task4();
			task4.Show();
		}
	}
}
