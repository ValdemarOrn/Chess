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
		public uint PlyCount;

		public byte From;
		public byte To;
		
		public byte Color;
		public byte Capture;
		public byte CaptureTile; // only differs from "To" in en passant attacks
		public byte Promotion;
		public byte CheckmateState;
		
		public byte PreviousChecmateState;
		public byte PreviousCastleState;
		public byte PreviousEnPassantTile;
	}
}
