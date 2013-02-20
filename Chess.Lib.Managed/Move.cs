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
		public byte CapturePiece;
		public byte CaptureTile; // only differs from "To" in en passant attacks
		public byte Promotion;
		public byte CheckmateState;
		public byte Castle;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
	public unsafe struct MoveHistory
	{
		public Move Move;

		public byte PrevAttacksWhite;
		public byte PrevAttacksBlack;
		public byte PrevHash;

		public byte PrevEnPassantTile;
		public byte PrevFiftyMoveRulePlies;
		public byte PrevCastleState;
		public byte PrevCheckmateState;

		public byte Unused; // make it multiple of 4 bytes
	}
}
