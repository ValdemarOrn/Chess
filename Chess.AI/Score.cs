using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.AI
{
	public class Score
	{
		public int Mater;
		public int Position;
		public int AttackBonus;
		public int DefenseBonus;
		public int HangingPiecePenalty;
		public int MovementBonus;
		public int PawnsOnSameFilePenalty;
		public int PawnCantMovePenalty;
		public int KnightForksBonus;
		public int CastledBonus;
		public int KingMovementBonus;
		public int RookGuardsBackRank;
		public int RookOnOpenFileBonus;
		public int CannotCastlePenalty;
		public int PawnsUntouchedKingsideBonus;
		public int CheckPenalty;
		public int MatePenalty;
		public int TempoBonus;
		public int QueenMovedEarlyPenalty;
		public int BothBishopsBonus;

		public int TotalScore
		{
			get
			{
				return
					Mater +
					Position +
					AttackBonus +
					DefenseBonus +
					HangingPiecePenalty +
					MovementBonus +
					PawnsOnSameFilePenalty +
					PawnCantMovePenalty +
					KnightForksBonus +
					CastledBonus +
					KingMovementBonus +
					RookGuardsBackRank +
					RookOnOpenFileBonus +
					CannotCastlePenalty +
					PawnsUntouchedKingsideBonus +
					CheckPenalty +
					MatePenalty +
					TempoBonus +
					QueenMovedEarlyPenalty +
					BothBishopsBonus;
			}
		}

		public void Add(Score score)
		{
			Mater += score.Mater;
			Position += score.Position;
			AttackBonus += score.AttackBonus;
			DefenseBonus += score.DefenseBonus;
			HangingPiecePenalty += score.HangingPiecePenalty;
			MovementBonus += score.MovementBonus;
			PawnsOnSameFilePenalty += score.PawnsOnSameFilePenalty;
			PawnCantMovePenalty += score.PawnCantMovePenalty;
			KnightForksBonus += score.KnightForksBonus;
			CastledBonus += score.CastledBonus;
			KingMovementBonus += score.KingMovementBonus;
			RookGuardsBackRank += score.RookGuardsBackRank;
			RookOnOpenFileBonus += score.RookOnOpenFileBonus;
			CannotCastlePenalty += score.CannotCastlePenalty;
			PawnsUntouchedKingsideBonus += score.PawnsUntouchedKingsideBonus;
			CheckPenalty += score.CheckPenalty;
			MatePenalty += score.MatePenalty;
			TempoBonus += score.TempoBonus;
			QueenMovedEarlyPenalty += score.QueenMovedEarlyPenalty;
			BothBishopsBonus += score.BothBishopsBonus;
		}

		public string ToString()
		{
			string s = "";
			s += "Material: " + (Mater / 1000M) + "\n";
			s += "Position: " + (Position / 1000M) + "\n";
			s += "AttackBonus: " + (AttackBonus / 1000M) + "\n";
			s += "DefenseBonus: " + (DefenseBonus / 1000M) + "\n";
			s += "HangingPiecePenalty: " + (HangingPiecePenalty / 1000M) + "\n";
			s += "MovementBonus: " + (MovementBonus / 1000M) + "\n";
			s += "PawnsOnSameFilePenalty: " + (PawnsOnSameFilePenalty / 1000M) + "\n";
			s += "PawnCantMovePenalty: " + (PawnCantMovePenalty / 1000M) + "\n";
			s += "KnightForksBonus: " + (KnightForksBonus / 1000M) + "\n";
			s += "CastledBonus: " + (CastledBonus / 1000M) + "\n";
			s += "KingMovementBonus: " + (KingMovementBonus / 1000M) + "\n";
			s += "RookGuardsBackRank: " + (RookGuardsBackRank / 1000M) + "\n";
			s += "RookOnOpenFileBonus: " + (RookOnOpenFileBonus / 1000M) + "\n";
			s += "CannotCastlePenalty: " + (CannotCastlePenalty / 1000M) + "\n";
			s += "PawnsUntouchedKingsideBonus: " + (PawnsUntouchedKingsideBonus / 1000M) + "\n";
			s += "CheckPenalty: " + (CheckPenalty / 1000M) + "\n";
			s += "MatePenalty: " + (MatePenalty / 1000M) + "\n";
			s += "TempoBonus: " + (TempoBonus / 1000M) + "\n";
			s += "QueenMovedEarlyPenalty: " + (QueenMovedEarlyPenalty / 1000M) + "\n";
			s += "BothBishopsBonus: " + (BothBishopsBonus / 1000M) + "\n";
			s += "\n";
			s += "Score: " + (TotalScore / 1000M);

			return s;
			
		}
	}
}
