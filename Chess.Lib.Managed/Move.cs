using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Chess.Lib
{
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
	public unsafe struct Move
	{
		public byte From;
		public byte To;

		public byte PlayerColor;
		public byte PlayerPiece;
		public byte CapturePiece;
		public byte CaptureTile; // location of capture piece. Only differs from "To" in en passant attacks
		public byte Promotion;
		public byte Castle;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
	public struct MoveSmall
	{
		public byte From;
		public byte To;
		public byte Piece;
		public byte Promotion;
		public int Score;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
	public unsafe struct MoveHistory
	{
		public Move HistoryMove;

		public ulong PrevAttacksWhite;
		public ulong PrevAttacksBlack;
		public ulong PrevHash;

		public byte PrevEnPassantTile;
		public byte PrevFiftyMoveRulePlies;
		public byte PrevCastleState;
	}
}
