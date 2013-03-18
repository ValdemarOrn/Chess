using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Chess.Lib.BitboardEditor
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Manager.InitLibrary();
			PieceBitmaps.Dir = "..\\..\\..\\Pieces\\";

			Application.Run(new Main());
		}
	}
}
