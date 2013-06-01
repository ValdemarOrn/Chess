using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chess.Base;

namespace Chess.Lib.BitboardEditor
{
	public partial class BoardControlPanel : UserControl
	{
		IntPtr boardStr;
		bool EnableStateChange;

		public unsafe BoardControlPanel()
		{
			InitializeComponent();
			EnableStateChange = true;
			boardControl1.LabeledTiles = true;
			boardControl1.UpdateCallback = RefreshBoard;
			boardStr = (IntPtr)Board.Create();
			boardControl1.BoardStruct = boardStr;
			Board.Init((BoardStruct*)boardStr, 0);
		}

		unsafe ~BoardControlPanel()
		{
			Board.Delete((BoardStruct*)boardStr);
		}

		private void buttonInit_Click(object sender, EventArgs e)
		{
			EnableStateChange = false;
			boardControl1.State = new Chess.Base.Board(true).State;
			radioButtonWhitePlayer.Checked = true;
			checkBoxCastlingBK.Checked = true;
			checkBoxCastlingBQ.Checked = true;
			checkBoxCastlingWK.Checked = true;
			checkBoxCastlingWQ.Checked = true;
			EnableStateChange = true;

			RefreshBoard();
			UpdateFEN();
		}

		private unsafe void RefreshBoard()
		{
			try
			{
				EnableStateChange = false;
				Board.Init((BoardStruct*)boardStr, 0);
				for (int i = 0; i < boardControl1.State.Length; i++)
				{
					var state = boardControl1.State[i];
					Board.SetPiece((BoardStruct*)boardStr, i, state & 0x0F, state & 0xF0);
				}

				int castling = 0;
				if (checkBoxCastlingBK.Checked)
					castling |= Board.CASTLE_BK;
				if (checkBoxCastlingBQ.Checked)
					castling |= Board.CASTLE_BQ;
				if (checkBoxCastlingWK.Checked)
					castling |= Board.CASTLE_WK;
				if (checkBoxCastlingWQ.Checked)
					castling |= Board.CASTLE_WQ;

				((BoardStruct*)boardStr)->Castle = (byte)castling;

				if (radioButtonWhitePlayer.Checked)
					((BoardStruct*)boardStr)->PlayerTurn = Board.COLOR_WHITE;
				else
					((BoardStruct*)boardStr)->PlayerTurn = Board.COLOR_BLACK;

				EnableStateChange = true;
				UpdateFEN();

				textBoxBits0.Text = String.Format("0x{0:X}", ((BoardStruct*)boardStr)->Boards[0]);
				textBoxBits1.Text = String.Format("0x{0:X}", ((BoardStruct*)boardStr)->Boards[1]);
				textBoxBits2.Text = String.Format("0x{0:X}", ((BoardStruct*)boardStr)->Boards[2]);
				textBoxBits3.Text = String.Format("0x{0:X}", ((BoardStruct*)boardStr)->Boards[3]);
				textBoxBits4.Text = String.Format("0x{0:X}", ((BoardStruct*)boardStr)->Boards[4]);
				textBoxBits5.Text = String.Format("0x{0:X}", ((BoardStruct*)boardStr)->Boards[5]);
				textBoxBits6.Text = String.Format("0x{0:X}", ((BoardStruct*)boardStr)->Boards[6]);
				textBoxBits7.Text = String.Format("0x{0:X}", ((BoardStruct*)boardStr)->Boards[7]);
			}
			catch(Exception)
			{
				
			}
		}

		private unsafe void UpdateFEN()
		{
			if (textBoxFEN.Focused)
				return;

			byte* str = stackalloc byte[100];
			Board.ToFEN((BoardStruct*)boardStr, str);
			string fenString = Helpers.GetString(str);
			textBoxFEN.Text = fenString;
		}

		private void StateChanged(object sender, EventArgs e)
		{
			try
			{
				if (!EnableStateChange)
					return;

				RefreshBoard();
				UpdateFEN();
			}
			catch (Exception) { }
		}

		private void buttonClear_Click(object sender, EventArgs e)
		{
			EnableStateChange = false;
			boardControl1.State = new int[64];
			radioButtonWhitePlayer.Checked = true;
			checkBoxCastlingBK.Checked = true;
			checkBoxCastlingBQ.Checked = true;
			checkBoxCastlingWK.Checked = true;
			checkBoxCastlingWQ.Checked = true;
			EnableStateChange = true;

			RefreshBoard();
			UpdateFEN();
		}

		private void boardControl1_Click(object sender, EventArgs e)
		{
			boardControl1.Focus();
		}

		private void buttonLoadFEN_Click(object sender, EventArgs e)
		{
			try
			{
				var newBoard = Notation.ReadFEN(textBoxFEN.Text);
				boardControl1.State = newBoard.State;
				checkBoxCastlingBK.Checked = newBoard.CanCastleKBlack;
				checkBoxCastlingBQ.Checked = newBoard.CanCastleQBlack;
				checkBoxCastlingWK.Checked = newBoard.CanCastleKWhite;
				checkBoxCastlingWQ.Checked = newBoard.CanCastleQBlack;

				RefreshBoard();
			}
			catch (Exception) { }
		}

	}
}
