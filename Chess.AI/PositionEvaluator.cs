using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Chess.AI.Tests")]

namespace Chess.AI
{
	

	public class PositionEvaluator
	{
		public static int ScoreDiff(Tuple<Score, Score> score)
		{
			return score.Item1.TotalScore - score.Item2.TotalScore;
		}

		public static Tuple<Score, Score> EvaluatePosition(Board board)
		{
			// 1. ==== All Pieces ====
			//
			// a) Piece value
			// b) Positional value
			// c) Bonus for what it defends (pawns over queens)
			// d) Bonus for what it attacks (pawns over queens)
			// e) Extra penalty for hanging pieces (use Attack boards and count attacks vs defences)
			// f) Bonus for range of motion (number of moves available)
			//
			//
			// 2. ==== Pawns ====
			//
			// a) Penalty for many pawns on the same channel
			// b) extra penalty if pawn cannot move 
			// c) penalty for passed pawns
			// 
			// 3. ==== Knights ====
			// 
			// a) Knights get bonus for forks
			// 
			// 
			// 4. ==== Queens ====
			// 
			// a) 
			// 
			// 
			// 5. ==== Kings ====
			// 
			// a) Bonus for being castled
			// b) extra bonus for range of motion (minimize checks and mates)
			// 
			// 
			// 6. ==== Rooks ====
			// 
			// a) bonus for guarding back rank 
			// b) bonus for guarding an open file
			// 
			// 
			// 7. ==== Bishops ====
			// 
			// a) 
			// 
			// 8. ==== Board Statistics ====
			// 
			// a) Endgame specific
			//		i) Knights are worth less
			//		ii) Bove Valuable in endgame
			//		iii) Rooks valuable in endgame
			//		iv) Change the kings positional evaluation
			//
			// b) Penalty for moving without castling
			// c) Penalty for single pawn islands (I defend no pawns, I am defended by no pawns)
			// d) bonus for pawn chains
			// e) Bonus for not moving kingside pawns if castling possible or king behind them
			// f) Bonus for checks
			// g) infinite bonus for mate
			// h) Bonus for tempo (whose turn it is)
			// i) Don't bring out the queen early
			// j) Bonus if both bishops
			// k) Static exchange evaluation



			// ===================== Get starting information =====================

			Score ScoreWhite = new Score();
			Score ScoreBlack = new Score();

			int totalPieceCount = 0; // total number of pieces
			int pwCount = 0; // white pawn count
			int pbCount = 0; // black pawn count
			int nwCount = 0; // white knight count
			int nbCount = 0; // black knight count
			int bwCount = 0; // white bishop count
			int bbCount = 0; // black bishop count
			int rwCount = 0; // white rook count
			int rbCount = 0; // black rook count
			int qwCount = 0; // white queen count
			int qbCount = 0; // black queen count

			// Note: Arrays are unusually large because we need to handle promotions!
			int[] pawnsWhite = new int[8];
			int[] pawnsBlack = new int[8];
			int[] knightsWhite = new int[4];
			int[] knightsBlack = new int[4];
			int[] bishopsWhite = new int[4];
			int[] bishopsBlack = new int[4];
			int[] rooksWhite = new int[4];
			int[] rooksBlack = new int[4];
			int[] queensWhite = new int[4];
			int[] queensBlack = new int[4];
			int kingWhite = -1;
			int kingBlack = -1;

			var attacks = new int[64][];

			var attackBoardWhite = new int[64];
			var attackBoardBlack = new int[64];

			#region find pieces and attacks
			
			// find all attacks
			for (int i = 0; i < 64; i++)
			{
				if (board.State[i] == 0)
					continue;

				int piece = board.Piece(i);

				// find all the moves this piece can make
				var moves = Attacks.GetAttacks(board, i);
				attacks[i] = moves;

				// keep a record of every pieces position
				if (board.Color(i) == Colors.White)
				{
					// mark them on the attack board
					foreach (int move in moves)
						attackBoardWhite[move]++;

					// Record all pieces by type
					switch(piece)
					{
						case(Pieces.Pawn):
							pawnsWhite[pwCount] = i;
							pwCount++;
							break;
						case (Pieces.Bishop):
							bishopsWhite[bwCount] = i;
							bwCount++;
							break;
						case (Pieces.Knight):
							knightsWhite[nwCount] = i;
							nwCount++;
							break;
						case (Pieces.Rook):
							rooksWhite[rwCount] = i;
							rwCount++;
							break;
						case (Pieces.Queen):
							queensWhite[qwCount] = i;
							qwCount++;
							break;
						case (Pieces.King):
							kingWhite = i;
							break;
					}
				}
				else
				{
					// mark them on the attack board
					foreach (int move in moves)
						attackBoardBlack[move]++;

					// Record all pieces by type
					switch (piece)
					{
						case (Pieces.Pawn):
							pawnsBlack[pbCount] = i;
							pbCount++;
							break;
						case (Pieces.Bishop):
							bishopsBlack[bbCount] = i;
							bbCount++;
							break;
						case (Pieces.Knight):
							knightsBlack[nbCount] = i;
							nbCount++;
							break;
						case (Pieces.Rook):
							rooksBlack[rbCount] = i;
							rbCount++;
							break;
						case (Pieces.Queen):
							queensBlack[qbCount] = i;
							qbCount++;
							break;
						case (Pieces.King):
							kingBlack = i;
							break;
					}
				}

				totalPieceCount++;
			}

			#endregion

			// list of all attackers
			// Note: Data can be had from the loop aboce with minimal overhead
			//var attackedBy = new Dictionary<int, List<int>>();


			// ======================== Loop all pieces ========================

			for (int i = 0; i < 64; i++)
			{
				var score = new Score();
				int color = board.Color(i);
				int piece = board.Piece(i);
				int val = board.State[i];

				if (piece == 0)
					continue;

				// 1. ==== All Pieces ====
				
				// a) Piece value
				score.Mater = GetPieceValue(piece);

				// b) Positional value
				score.Position += ReadTable(GetPositionTable(piece), i, color);

				// c) Bonus for what it defends (pawns over queens)
				// d) Bonus for what it attacks (pawns over queens)
				int attackBonus = GetProtectionValue(piece);
				var attacksByPiece = attacks[i];


				for (int k = 0; k < attacksByPiece.Length; k++)
				{
					int attack = attacksByPiece[k];

					if (board.State[attack] == 0)
						continue;

					// bonus for any targets; protecting allies, attacking enemies
					if (board.Color(attack) == color)
						score.DefenseBonus += (int)(attackBonus * DefenceBonusMultiplier);
					else
						score.AttackBonus += (int)(attackBonus * AttackBonusMultiplier);
				}

				// e) Extra penalty for hanging pieces (use Attack boards and count attacks vs defences)
				if (color == Colors.White && board.PlayerTurn != color && attackBoardWhite[i] == 0 && attackBoardBlack[i] > 0)
					score.HangingPiecePenalty += HangingPiecePenalty;
				else if (color == Colors.Black && board.PlayerTurn != color && attackBoardBlack[i] == 0 && attackBoardWhite[i] > 0)
					score.HangingPiecePenalty += HangingPiecePenalty;

				// f) Bonus for range of motion (number of moves available)
				score.MovementBonus += (int)(attacksByPiece.Length * MovementBonusMultiplier);

				#region Piece specific evaluations
				
				if (piece == Pieces.Pawn)
				{
					// 2. ==== Pawns ====

					// a) Penalty for many pawns on the same channel
					if (i + 8 < 64 && board.State[i + 8] == val)
						score.PawnsOnSameFilePenalty += PawnsOnSameFilePenalty;
					else if (i + 16 < 64 && board.State[i + 16] == val)
						score.PawnsOnSameFilePenalty += PawnsOnSameFilePenalty;
					else if (i + 24 < 64 && board.State[i + 24] == val)
						score.PawnsOnSameFilePenalty += PawnsOnSameFilePenalty;
					else if (i + 32 < 64 && board.State[i + 32] == val)
						score.PawnsOnSameFilePenalty += PawnsOnSameFilePenalty;
					else if (i - 8 >= 0 && board.State[i - 8] == val)
						score.PawnsOnSameFilePenalty += PawnsOnSameFilePenalty;
					else if (i - 16 >= 0 && board.State[i - 16] == val)
						score.PawnsOnSameFilePenalty += PawnsOnSameFilePenalty;
					else if (i - 24 >= 0 && board.State[i - 24] == val)
						score.PawnsOnSameFilePenalty += PawnsOnSameFilePenalty;
					else if (i - 32 >= 0 && board.State[i - 32] == val)
						score.PawnsOnSameFilePenalty += PawnsOnSameFilePenalty;

					// b) extra penalty if pawn cannot move
					var moves = Moves.GetMoves(board, i);
					if (moves.Length == 0)
						score.PawnCantMovePenalty += PawnCantMovePenalty;

					// c) penalty for passed pawns
					// Todo: passed pawns
				}
				else if (piece == Pieces.Knight)
				{
					// 3. ==== Knights ====
					
					// a) Knights get bonus for forks
					int knightAttackCount = 0;
					foreach (var attack in attacksByPiece)
					{
						if(board.State[attack] == 0)
							continue;

						var attackedPiece = board.Piece(attack);
						if (board.Color(attack) != color && (attackedPiece != Pieces.Pawn && attackedPiece != Pieces.Knight))
							knightAttackCount++;
					}

					if (knightAttackCount > 1)
						score.KnightForksBonus += KnightForksBonus;
				}
				else if (piece == Pieces.Queen)
				{
					// 4. ==== Queens ====

				}
				else if (piece == Pieces.King)
				{
					// 5. ==== Kings ====
			
					// a) Bonus for being castled
					if (i < 3 && board.CastleQW == Moves.HasCastled)
						score.CastledBonus += CastledBonus;
					else if (i > 4 && i < 8 && board.CastleKW == Moves.HasCastled)
						score.CastledBonus += CastledBonus;
					if (i > 55 && i < 59 && board.CastleQB == Moves.HasCastled)
						score.CastledBonus += CastledBonus;
					else if (i > 61 && board.CastleKB == Moves.HasCastled)
						score.CastledBonus += CastledBonus;


					// b) extra bonus for range of motion (minimize checks and mates)
					var moves = Moves.GetMoves(board, i);
					score.KingMovementBonus += (int)(moves.Length * KingMovementBonusMultiplier);
				}
				else if (piece == Pieces.Rook)
				{
					// 6. ==== Rooks ====

					// a) bonus for guarding back rank
					// Todo: Improve rook eval, can't be boxed in
					if (color == Colors.White && Board.Y(i) == 0)
						score.RookGuardsBackRank += RookGuardsBackRankBonus;
					if (color == Colors.Black && Board.Y(i) == 7)
						score.RookGuardsBackRank += RookGuardsBackRankBonus;

					// b) bonus for guarding an open file
					bool open = true;

					int t = i + 8;
					// look at tiles above (up to rank 7)
					while (t < 56 && open)
					{
						open = (board.State[t] == 0);
						t += 8;
					}

					t = i - 8;
					// look at tiles below
					while (t >= 0 && open)
					{
						open = (board.State[t] == 0);
						t -= 8;
					}

					if (open)
						score.RookOnOpenFileBonus += RookOnOpenFileBonus;
				}
				else if (piece == Pieces.Bishop)
				{
					// 7. ==== Bishops ====
				}

				#endregion

				if (color == Colors.White)
					ScoreWhite.Add(score);
				else
					ScoreBlack.Add(score);
			}

			// 8. ==== Board Statistics ====
			
			// a) Endgame specific
			//		i) Knights are worth less
			//		ii) Bove Valuable in endgame
			//		iii) Rooks valuable in endgame
			//		iv) Change the kings positional evaluation
					
			// b) Penalty for moving without castling
			if (board.CastleKW == Moves.CannotCastle && board.CastleQW == Moves.CannotCastle)
				ScoreWhite.CannotCastlePenalty += CannotCastlePenalty;
			else if (board.CastleKB == Moves.CannotCastle && board.CastleQB == Moves.CannotCastle)
				ScoreBlack.CannotCastlePenalty += CannotCastlePenalty;

			// c) Penalty for single pawn islands (I defend no pawns, I am defended by no pawns)
			// d) bonus for pawn chains

			// e) Bonus for not moving kingside pawns if castling possible or king behind them
			int whitePawn = Pieces.Pawn | Colors.White;
			int blackPawn = Pieces.Pawn | Colors.Black;
			if (board.State[13] == whitePawn && board.State[14] == whitePawn && board.State[15] == whitePawn)
				ScoreWhite.PawnsUntouchedKingsideBonus = PawnsUntouchedKingsideBonus;
			if (board.State[53] == blackPawn && board.State[54] == blackPawn && board.State[55] == blackPawn)
				ScoreBlack.PawnsUntouchedKingsideBonus = PawnsUntouchedKingsideBonus;

			// f) Bonus for checks
			// g) infinite bonus for mate
			if (attackBoardBlack[kingWhite] > 0)
			{
				ScoreWhite.CheckPenalty = CheckPenalty;

				// If check mate
				if (Check.IsCheckMate(board, Colors.White))
					ScoreWhite.MatePenalty = MatePenalty;
			}
			if (attackBoardWhite[kingBlack] > 0)
			{
				ScoreBlack.CheckPenalty = CheckPenalty;

				if (Check.IsCheckMate(board, Colors.Black))
					ScoreBlack.MatePenalty = MatePenalty;
			}

			// h) Bonus for tempo (whose turn it is)
			
			if (board.PlayerTurn == Colors.White)
				ScoreWhite.TempoBonus = TempoBonus;
			else
				ScoreBlack.TempoBonus = TempoBonus;

			// i) Don't bring out the queen early
			if (board.MoveCount < 6 && queensWhite[0] != 3)
				ScoreWhite.QueenMovedEarlyPenalty = QueenMovedEarlyPenalty;
			if (board.MoveCount < 6 && queensBlack[0] != 59)
				ScoreBlack.QueenMovedEarlyPenalty = QueenMovedEarlyPenalty;

			// j) Bonus if both bishops

			if (bwCount == 2)
				ScoreWhite.BothBishopsBonus = BothBishopsBonus;
			if (bbCount == 2)
				ScoreBlack.BothBishopsBonus = BothBishopsBonus;

			// k) Static exchange evaluation

			// Todo: SEE

			return new Tuple<Score, Score>(ScoreWhite, ScoreBlack);
		}
		
		internal static int PieceValuePawn = 1000;
		internal static int PieceValueKnight = 3000;
		internal static int PieceValueishop = 3200;
		internal static int PieceValueRook = 5000;
		internal static int PieceValueQueen = 9000;
		internal static int PieceValueKing = 0;

		internal static int ProtectionValuePawn = 50;
		internal static int ProtectionValueKnight = 35;
		internal static int ProtectionValueBishop = 25;
		internal static int ProtectionValueRook = 10;
		internal static int ProtectionValueQueen = 10;
		internal static int ProtectionValueKing = 0;

		internal static float DefenceBonusMultiplier = 2.0f;
		internal static float AttackBonusMultiplier = 2.0f;

		internal static int HangingPiecePenalty = -500;
		internal static float MovementBonusMultiplier = 25.0f;
		internal static int PawnsOnSameFilePenalty = -30;
		internal static int PawnCantMovePenalty = -40;
		internal static int KnightForksBonus = 60;
		internal static int CannotCastlePenalty = -60;
		internal static float KingMovementBonusMultiplier = 10;
		internal static int RookGuardsBackRankBonus = 100;
		internal static int RookOnOpenFileBonus = 20;
		internal static int PawnsUntouchedKingsideBonus = 200;
		internal static int CheckPenalty = -500;
		internal static int MatePenalty = -1000000000;
		internal static int TempoBonus = 100;
		internal static int QueenMovedEarlyPenalty = -100;
		internal static int BothBishopsBonus = 50;

		internal static int CastledBonus = 300;

		internal static int ReadTable(short[] table, int pos, int color)
		{
			int x = Board.X(pos);
			int y = Board.Y(pos);

			int location = (color == Colors.White) ? x + (7 - y) * 8 : x + y * 8;
			return table[location]*10;
		}

		internal static int GetPieceValue(int piece)
		{
			switch (piece)
			{
				case (Pieces.Pawn):
					return PieceValuePawn;
				case (Pieces.Knight):
					return PieceValueKnight;
				case (Pieces.Bishop):
					return PieceValueishop;
				case (Pieces.Rook):
					return PieceValueRook;
				case (Pieces.Queen):
					return PieceValueQueen;
				case (Pieces.King):
					return PieceValueKing;
				default:
					return 0;
			}
		}

		internal static int GetProtectionValue(int piece)
		{
			switch (piece)
			{
				case (Pieces.Pawn):
					return ProtectionValuePawn;
				case (Pieces.Knight):
					return ProtectionValueKnight;
				case (Pieces.Bishop):
					return ProtectionValueBishop;
				case (Pieces.Rook):
					return ProtectionValueRook;
				case (Pieces.Queen):
					return ProtectionValueQueen;
				case (Pieces.King):
					return ProtectionValueKing;
				default:
					return 0;
			}
		}

		internal static short[] GetPositionTable(int piece)
		{
			switch(piece)
			{
				case(Pieces.Pawn):
					return PawnTable;
				case (Pieces.Knight):
					return KnightTable;
				case (Pieces.Bishop):
					return BishopTable;
				case (Pieces.King):
					return KingTable;
				default:
					return NullTable;
			}
		}

		internal static readonly short[] NullTable = new short[]
		{
		  0,  0,  0,  0,  0,  0,  0,  0,
		  0,  0,  0,  0,  0,  0,  0,  0,
		  0,  0,  0,  0,  0,  0,  0,  0,
		  0,  0,  0,  0,  0,  0,  0,  0,
		  0,  0,  0,  0,  0,  0,  0,  0,
		  0,  0,  0,  0,  0,  0,  0,  0,
		  0,  0,  0,  0,  0,  0,  0,  0,
		  0,  0,  0,  0,  0,  0,  0,  0
		};

		internal static readonly short[] PawnTable = new short[]
		{
		  0,  0,  0,  0,  0,  0,  0,  0,
		 50, 50, 50, 50, 50, 50, 50, 50,
		 10, 10, 20, 30, 30, 20, 10, 10,
		  5,  5, 10, 27, 27, 10,  5,  5,
		 -5,  0,  0, 25, 25,  0,  0, -5,
		 -15,-5,-10,  0,  0,-10, -5,-15,
		 -5, 10, 10,-25,-25, 10, 10, -5,
		  0,  0,  0,  0,  0,  0,  0,  0
		};

		internal static readonly short[] KnightTable = new short[]
		{
		 -50,-40,-30,-30,-30,-30,-40,-50,
		 -40,-20,  0,  0,  0,  0,-20,-40,
		 -30,  0, 10, 15, 15, 10,  0,-30,
		 -30,  5, 15, 20, 20, 15,  5,-30,
		 -30,  0, 15, 20, 20, 15,  0,-30,
		 -30,  5, 10, 15, 15, 10,  5,-30,
		 -40,-20,  0,  5,  5,  0,-20,-40,
		 -50,-40,-20,-30,-30,-20,-40,-50,
		};

		internal static readonly short[] BishopTable = new short[]
		{
		 -20,-10,-10,-10,-10,-10,-10,-20,
		 -10,  0,  0,  0,  0,  0,  0,-10,
		 -10,  0,  5, 10, 10,  5,  0,-10,
		 -10,  5,  5, 10, 10,  5,  5,-10,
		 -10,  0, 10, 10, 10, 10,  0,-10,
		 -10, 10, 10, 10, 10, 10, 10,-10,
		 -10,  5,  0,  0,  0,  0,  5,-10,
		 -20,-10,-40,-10,-10,-40,-10,-20,
		};

		internal static readonly short[] KingTable = new short[]
		{
		  -30, -40, -40, -50, -50, -40, -40, -30,
		  -30, -40, -40, -50, -50, -40, -40, -30,
		  -30, -40, -40, -50, -50, -40, -40, -30,
		  -30, -40, -40, -50, -50, -40, -40, -30,
		  -20, -30, -30, -40, -40, -30, -30, -20,
		  -10, -20, -20, -20, -20, -20, -20, -10, 
		   20,  20,   0,   0,   0,   0,  20,  20,
		   20,  30,  10,   0,   0,  10,  30,  20
		};

		internal static readonly short[] KingTableEndGame = new short[]
		{
		 -50,-40,-30,-20,-20,-30,-40,-50,
		 -30,-20,-10,  0,  0,-10,-20,-30,
		 -30,-10, 20, 30, 30, 20,-10,-30,
		 -30,-10, 30, 40, 40, 30,-10,-30,
		 -30,-10, 30, 40, 40, 30,-10,-30,
		 -30,-10, 20, 30, 30, 20,-10,-30,
		 -30,-30,  0,  0,  0,  0,-30,-30,
		 -50,-30,-30,-30,-30,-30,-30,-50
		};
	}
}
