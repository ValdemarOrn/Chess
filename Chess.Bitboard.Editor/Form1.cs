using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chess.Bitboard.Editor
{
	public partial class Form1 : Form
	{
		ulong _state;
		public ulong State
		{
			get { return _state; }
			set
			{ 
				_state = value;
				if (boardControl1 != null)
					boardControl1.State = value;

				SetTexboxes();
			}
		}

		ulong[] States;
		int Index;

		public Form1()
		{
			InitializeComponent();
			boardControl1.UpdateCallback = BoardUpdated;

			States = new ulong[10];
			Index = 0;
			State = (ulong)0;
		}

		public void BoardUpdated()
		{
			if (boardControl1 == null || State == boardControl1.State)
				return;

			State = boardControl1.State;
			SetTexboxes();
		}

		void SetTexboxes()
		{
			if (State == (ulong)0)
			{
				textBoxHex.Text = "";
				textBoxDec.Text = "";
				textBoxList.Text = "";
			}
			else
			{
				textBoxHex.Text = "0x" + String.Format("{0:X}", State);
				textBoxDec.Text = String.Format("{0:G}", State);

				var list = new List<int>();
				for (int i = 0; i < 64; i++)
				{
					if (Bitboard.Get(State, i))
						list.Add(i);
				}

				textBoxList.Text = list.Select(k => k.ToString()).Aggregate((x, y) => x + "," + y);
			}

			string bitsHigh = "";
			for (int i = 63; i > 31; i--)
			{
				if ((i+1) % 8 == 0 && i != 63)
					bitsHigh += ".";

				if (Bitboard.Get(State, i))
					bitsHigh += "1";
				else
					bitsHigh += "0";
			}
			labelBitsUpper.Text = bitsHigh;


			string bitsLow = "";
			for (int i = 31; i >= 0; i--)
			{
				if ((i + 1) % 8 == 0 && i != 31)
					bitsLow += ".";

				if (Bitboard.Get(State, i))
					bitsLow += "1";
				else
					bitsLow += "0";
			}
			labelBitsLower.Text = bitsLow;
		}

		private void textBoxHex_TextChanged(object sender, EventArgs e)
		{
			try
			{
				ulong val = Convert.ToUInt64(textBoxHex.Text, 16);
				State = val;
			}
			catch (Exception)
			{
			}
		}

		private void textBoxDec_TextChanged(object sender, EventArgs e)
		{
			try
			{
				ulong val = Convert.ToUInt64(textBoxDec.Text, 10);
				State = val;
			}
			catch (Exception)
			{
			}
		}


		private void buttonClear_Click(object sender, EventArgs e)
		{
			State = 0;
		}

		private void buttonInvert_Click(object sender, EventArgs e)
		{
			State = ~State;
		}

		private void buttonNext_Click(object sender, EventArgs e)
		{
			States[Index] = State;
			Index = (Index + 1) % States.Length;
			State = States[Index];
			textBoxPos.Text = Index.ToString();
		}

		private void buttonPrev_Click(object sender, EventArgs e)
		{
			States[Index] = State;
			Index = (States.Length + Index - 1) % States.Length;
			State = States[Index];
			textBoxPos.Text = Index.ToString();
		}

		private void buttonShiftUp_Click(object sender, EventArgs e)
		{
			State = State << 8;
		}

		private void buttonShiftDown_Click(object sender, EventArgs e)
		{
			State = State >> 8;
		}

		private void buttonShiftRight_Click(object sender, EventArgs e)
		{
			State = State << 1;
		}

		private void buttonShiftLeft_Click(object sender, EventArgs e)
		{
			State = State >> 1;
		}

		

		


	}
}
