using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base
{
	public partial class Zobrist
	{
		/// <summary>
		/// Accessor methods to get Zobrist keys
		/// </summary>
		public static class Key
		{
			public static ulong PlayerTurn(Color color) 
			{ 
				return Keys[IndexPlayerTurn, (int)color]; 
			}

			public static ulong Castling(HashSet<Castling> castlingRights) 
			{
				int castling = castlingRights.Select(x => (int)x).Aggregate((total, x) => total | x);
				return Keys[IndexCastling, castling];
			}

			public static ulong Castling(params Castling[] castlingRights)
			{
				int castling = castlingRights.Select(x => (int)x).Aggregate((total, x) => total | x);
				return Keys[IndexCastling, castling];
			}

			public static ulong EnPassant(int square)
			{
				return Keys[IndexEnPassant, square];
			}

			public static ulong Piece(Color color, Piece piece, int square)
			{
				return Keys[Index[Colors.Val(color, piece)], square];
			}

			public static ulong Piece(int pieceAndColor, int square)
			{
				return Keys[Index[pieceAndColor], square];
			}
		}
	
		const int IndexPlayerTurn = 12;
		const int IndexCastling = 13;
		const int IndexEnPassant = 14;

		static byte[] Index;

		static Zobrist()
		{
			SetKeys();
			Init();
		}

		/// <summary>
		/// sets up the zobrist key indexes
		/// </summary>
		static void Init()
		{
			Index = new byte[256];

			// define indexes for pieces
			Index[Colors.Val(Color.White, Piece.Pawn)] = 0;
			Index[Colors.Val(Color.White, Piece.Knight)] = 1;
			Index[Colors.Val(Color.White, Piece.Bishop)] = 2;
			Index[Colors.Val(Color.White, Piece.Rook)] = 3;
			Index[Colors.Val(Color.White, Piece.Queen)] = 4;
			Index[Colors.Val(Color.White, Piece.King)] = 5;

			Index[Colors.Val(Color.Black, Piece.Pawn)] = 6;
			Index[Colors.Val(Color.Black, Piece.Knight)] = 7;
			Index[Colors.Val(Color.Black, Piece.Bishop)] = 8;
			Index[Colors.Val(Color.Black, Piece.Rook)] = 9;
			Index[Colors.Val(Color.Black, Piece.Queen)] = 10;
			Index[Colors.Val(Color.Black, Piece.King)] = 11;

			// No piece or color, indexes empty space
			Index[0] = 15;
			Index[(int)Piece.Pawn] = 15;
			Index[(int)Piece.Knight] = 15;
			Index[(int)Piece.Bishop] = 15;
			Index[(int)Piece.Rook] = 15;
			Index[(int)Piece.Queen] = 15;
			Index[(int)Piece.King] = 15;
			Index[(int)Color.White] = 15;
			Index[(int)Color.Black] = 15;

			// zero out the key for the "no en-passant" square.
			Keys[IndexEnPassant, 0] = 0;
		}

		public static ulong Calculate(Board board)
		{
			ulong hash = 0;

			for (int i = 0; i < 64; i++)
			{
				var color = board.GetColor(i);
				if (color == Color.None)
					continue;

				var piece = board.GetPiece(i);

				hash = hash ^ Keys[Index[Colors.Val(color, piece)], i];
			}

			int castling = board.CastlingRights.Select(x => (int)x).Aggregate((total, x) => total | x);

			hash = hash ^ Keys[IndexPlayerTurn, (int)board.PlayerTurn];
			hash = hash ^ Keys[IndexCastling, castling];
			hash = hash ^ Keys[IndexEnPassant, board.EnPassantTile];

			return hash;
		}
	}
}
