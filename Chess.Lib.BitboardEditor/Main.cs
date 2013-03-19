using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chess.Lib.BitboardEditor
{
	public partial class Main : Form
	{
		public Main()
		{
			InitializeComponent();
			KeyPreview = true;
			KeyPress += Main_KeyPress;
		}

		void Main_KeyPress(object sender, KeyPressEventArgs e)
		{
			if(boardControlPanel1.boardControl1.Focused)
				boardControlPanel1.boardControl1.HandleKeyPress(e.KeyChar);
		}

	}
}
