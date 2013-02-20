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
		public uint MoveCount;

		public byte From;
		public byte To;
		
		public byte Color;
		public byte Capture;
		public byte CaptureTile; // only differs from "To" in en passant attacks
		public byte Promotion;
		public byte CheckmateState;
		public byte Castle;

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
