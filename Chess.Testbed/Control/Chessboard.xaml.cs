using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Chess.Base;
using Chess.Testbed.Views;

namespace Chess.Testbed.Control
{
	/// <summary>
	/// Interaction logic for Chessboard.xaml
	/// </summary>
	public partial class Chessboard : UserControlBase
	{
		public static readonly DependencyProperty DarkTileBrushProperty =
			DependencyProperty.Register("DarkTileBrush", typeof(Brush), typeof(Chessboard), new PropertyMetadata(Brushes.DarkGray));

		public static readonly DependencyProperty LightTileBrushProperty =
			DependencyProperty.Register("LightTileBrush", typeof(Brush), typeof(Chessboard), new PropertyMetadata(Brushes.White));

		public static readonly DependencyProperty BoardProperty =
			DependencyProperty.Register("Board", typeof(Board), typeof(Chessboard), new PropertyMetadata(null));

		public static readonly DependencyProperty FontBrushProperty =
			DependencyProperty.Register("FontBrush", typeof(Brush), typeof(Chessboard), new PropertyMetadata(Brushes.Black));

		public static readonly DependencyProperty BoardEdgeBrushProperty =
			DependencyProperty.Register("BoardEdgeBrush", typeof(Brush), typeof(Chessboard), new PropertyMetadata(Brushes.LightGray));

		public static readonly DependencyProperty InvertBoardProperty =
			DependencyProperty.Register("InvertBoard", typeof(bool), typeof(Chessboard), new PropertyMetadata(false));


		public Chessboard()
		{
			InitializeComponent();
			PaintTiles();

			var prop = DependencyPropertyDescriptor.FromProperty(BoardProperty, this.GetType());
			prop.AddValueChanged(this, (s, e) => Repaint());

			prop = DependencyPropertyDescriptor.FromProperty(DarkTileBrushProperty, this.GetType());
			prop.AddValueChanged(this, (s, e) => PaintTiles());
			prop = DependencyPropertyDescriptor.FromProperty(LightTileBrushProperty, this.GetType());
			prop.AddValueChanged(this, (s, e) => PaintTiles());

			prop = DependencyPropertyDescriptor.FromProperty(InvertBoardProperty, this.GetType());
			prop.AddValueChanged(this, (s, e) =>
			{
				NotifyChanged(() => File);
				NotifyChanged(() => Rank);
			});
		}

		public Board Board
		{
			get { return (Board)GetValue(BoardProperty); }
			set { SetValue(BoardProperty, value); }
		}

		public Brush DarkTileBrush
		{
			get { return (Brush)GetValue(DarkTileBrushProperty); }
			set { SetValue(DarkTileBrushProperty, value); }
		}


		public Brush LightTileBrush
		{
			get { return (Brush)GetValue(LightTileBrushProperty); }
			set { SetValue(LightTileBrushProperty, value); }
		}

		public Brush BoardEdgeBrush
		{
			get { return (Brush)GetValue(BoardEdgeBrushProperty); }
			set { SetValue(BoardEdgeBrushProperty, value); }
		}

		public Brush FontBrush
		{
			get { return (Brush)GetValue(FontBrushProperty); }
			set { SetValue(FontBrushProperty, value); }
		}

		public bool InvertBoard
		{
			get { return (bool)GetValue(InvertBoardProperty); }
			set { SetValue(InvertBoardProperty, value); }
		}

		public string[] File
		{
			get
			{
				return !InvertBoard
					? "ABCDEFGH".Select(x => x.ToString()).ToArray()
					: "HGFEDCBA".Select(x => x.ToString()).ToArray();
			}
		}

		public string[] Rank
		{
			get
			{
				return !InvertBoard
					? "12345678".Select(x => x.ToString()).ToArray()
					: "87654321".Select(x => x.ToString()).ToArray();
			}
		}

		private void PaintTiles()
		{
			CanvasBackground.Children.Clear();
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					var brush = ((i + j) % 2 == 0) ? LightTileBrush : DarkTileBrush;
					var rect = new Rectangle {Width = 100, Height = 100, Fill = brush};
					CanvasBackground.Children.Add(rect);
					Canvas.SetLeft(rect, i * 100);
					Canvas.SetTop(rect, j * 100);
				}
			}
		}

		private void Repaint()
		{
			CanvasPieces.Children.Clear();

			if (Board.State.Length != 64)
				throw new Exception("Board must have 64 tiles");

			for (int i = 0; i < 64; i++)
			{
				if (Board.State[i] == 0)
					continue;

				var x = i % 8;
				var y = 7 - (i / 8);

				if (InvertBoard)
				{
					x = 7 - x;
					y = 7 - y;
				}

				var piece = GetPiece(Board.State[i]);
				if (piece == null)
					continue;
				piece.Width = 100;
				piece.Height = 100;
				CanvasPieces.Children.Add(piece);
				Canvas.SetLeft(piece, x * 100);
				Canvas.SetTop(piece, y * 100);
			}
		}

		private UserControl GetPiece(int tile)
		{
			switch(tile)
			{
				case (int)Base.Piece.Pawn | (int)Base.Color.White:
					return new Pieces.WhitePawn();
				case (int)Base.Piece.Knight | (int)Base.Color.White:
					return new Pieces.WhiteKnight();
				case (int)Base.Piece.Bishop | (int)Base.Color.White:
					return new Pieces.WhiteBishop();
				case (int)Base.Piece.Rook | (int)Base.Color.White:
					return new Pieces.WhiteRook();
				case (int)Base.Piece.Queen | (int)Base.Color.White:
					return new Pieces.WhiteQueen();
				case (int)Base.Piece.King | (int)Base.Color.White:
					return new Pieces.WhiteKing();

				case (int)Base.Piece.Pawn | (int)Base.Color.Black:
					return new Pieces.BlackPawn();
				case (int)Base.Piece.Knight | (int)Base.Color.Black:
					return new Pieces.BlackKnight();
				case (int)Base.Piece.Bishop | (int)Base.Color.Black:
					return new Pieces.BlackBishop();
				case (int)Base.Piece.Rook | (int)Base.Color.Black:
					return new Pieces.BlackRook();
				case (int)Base.Piece.Queen | (int)Base.Color.Black:
					return new Pieces.BlackQueen();
				case (int)Base.Piece.King | (int)Base.Color.Black:
					return new Pieces.BlackKing();

				default:
					return null;
			}
		}
	}
}
