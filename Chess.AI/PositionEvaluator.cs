using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.AI
{
	public class PositionEvaluator
	{
		public static int EvaluatePosition(Board board)
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
			// c) extra penalty if pawn cannot move 
			// >> Store pawns move for later, needed for evaluating pawn islands
			// 
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
				 

			int totalScore = 0;

			var attackBoardWhite = Check.GetAttackBoard(board, Colors.White);
			var attackBoardBlack = Check.GetAttackBoard(board, Colors.Black);

			int whiteBishops = 0;
			int blackBishops = 0;

			// List of all square a piece can attack
			var attacksFrom = new Dictionary<int, List<int>>();

			// list of all attackers
			//var attackedBy = new Dictionary<int, List<int>>();

			// list of all pawns and their moves
			var pawnMoves = new Dictionary<int, List<int>>();

			// ======================== Loop all pieces ========================

			for (int i = 0; i < 64; i++)
			{
				int score = 0;
				int color = board.Color(i);
				int piece = board.Piece(i);
				int val = board.State[i];

				if (piece == 0)
					continue;

				// 1. ==== All Pieces ====
				
				// a) Piece value
				score = GetPieceValue(piece);

				// b) Positional value
				score += ReadTable(GetPositionTable(piece), i, color);

				// c) Bonus for what it defends (pawns over queens)
				// d) Bonus for what it attacks (pawns over queens)
				int attackBonus = GetProtectionValue(piece);
				var attackBoard = Attacks.GetAttacks(board, i);
				attacksFrom[i] = attackBoard;


				for (int k = 0; k < attackBoard.Count; k++)
				{
					if (board.State[k] == 0)
						continue;

					// bonus for any targets; protecting allies, attacking enemies
					score += attackBonus;
				}

				// e) Extra penalty for hanging pieces (use Attack boards and count attacks vs defences)
				if (color == Colors.White && attackBoardWhite[i] < attackBoardBlack[i])
					score += HangingPiecePenalty;
				else if (color == Colors.Black && attackBoardBlack[i] < attackBoardWhite[i])
					score += HangingPiecePenalty;

				// f) Bonus for range of motion (number of moves available)
				score += (int)(attackBoard.Count * MovementBonusMultiplier);
				
				 
				if (piece == Pieces.Pawn)
				{
					// 2. ==== Pawns ====

					// a) Penalty for many pawns on the same channel
					if (i + 8 < 64 && board.State[i + 8] == val)
						score += PawnsOnSameFilePenalty;
					else if (i + 16 < 64 && board.State[i + 16] == val)
						score += PawnsOnSameFilePenalty;
					else if (i + 24 < 64 && board.State[i + 24] == val)
						score += PawnsOnSameFilePenalty;
					else if (i - 8 >= 0 && board.State[i - 8] == val)
						score += PawnsOnSameFilePenalty;
					else if (i - 16 >= 0 && board.State[i - 16] == val)
						score += PawnsOnSameFilePenalty;
					else if (i - 24 >= 0 && board.State[i - 24] == val)
						score += PawnsOnSameFilePenalty;

					// c) extra penalty if pawn cannot move
					var moves = Moves.GetMoves(board, i);
					if(moves.Count == 0)
						score += PawnCantMovePenalty;

					// >> Store pawns move for later, needed for evaluating pawn islands
					pawnMoves[i] = moves;
				}
				else if (piece == Pieces.Knight)
				{
					// 3. ==== Knights ====
					
					// a) Knights get bonus for forks
					int knightAttackCount = 0;
					foreach(var attack in attackBoard)
					{
						if(board.State[attack] == 0)
							continue;

						var attackedPiece = board.Piece(attack);
						if (board.Color(attack) != color && (attackedPiece != Pieces.Pawn && attackedPiece != Pieces.Knight))
							knightAttackCount++;
					}

					if (knightAttackCount > 1)
						score += KnightForksBonus;
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
						score += CastledBonus;
					else if (i > 4 && i < 8 && board.CastleKW == Moves.HasCastled)
						score += CastledBonus;
					if (i > 55 && i < 59 && board.CastleQB == Moves.HasCastled)
						score += CastledBonus;
					else if (i > 61 && board.CastleKB == Moves.HasCastled)
						score += CastledBonus;


					// b) extra bonus for range of motion (minimize checks and mates)
					var moves = Moves.GetMoves(board, i);
					score += (int)(moves.Count * KingMovementBonusMultiplier);
				}
				else if (piece == Pieces.Rook)
				{
					// 6. ==== Rooks ====

					// a) bonus for guarding back rank
					// Todo: Improve, can't be boxed in
					if (color == Colors.White && Board.Y(i) == 0)
						score += RookGuardsBackRankBonus;

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
						score += RookOnOpenFileBonus;
				}
				else if (piece == Pieces.Bishop)
				{
					// 7. ==== Bishops ====
					if (color == Colors.White)
						whiteBishops++;
					else
						blackBishops++;
				}

				if (color == Colors.White)
					totalScore += score;
				else
					totalScore -= score;
			}

			// 8. ==== Board Statistics ====
			
			// a) Endgame specific
			//		i) Knights are worth less
			//		ii) Bove Valuable in endgame
			//		iii) Rooks valuable in endgame
			//		iv) Change the kings positional evaluation
					
			// b) Penalty for moving without castling
			if (board.CastleKW == Moves.CannotCastle && board.CastleQW == Moves.CannotCastle)
				totalScore += CannotCastlePenalty;
			else if (board.CastleKB == Moves.CannotCastle && board.CastleQB == Moves.CannotCastle)
				totalScore -= CannotCastlePenalty;

			// c) Penalty for single pawn islands (I defend no pawns, I am defended by no pawns)
			// d) bonus for pawn chains
			// e) Bonus for not moving kingside pawns if castling possible or king behind them
			// f) Bonus for checks
			// g) infinite bonus for mate
			// h) Bonus for tempo (whose turn it is)
			// i) Don't bring out the queen early
			// j) Bonus if both bishops

			return totalScore;
		}
		
		public static int PieceValuePawn = 100;
		public static int PieceValueKnight = 300;
		public static int PieceValueishop = 320;
		public static int PieceValueRook = 500;
		public static int PieceValueQueen = 900;
		public static int PieceValueKing = 0;

		public static int ProtectionValuePawn = 50;
		public static int ProtectionValueKnight = 35;
		public static int ProtectionValueBishop = 25;
		public static int ProtectionValueRook = 10;
		public static int ProtectionValueQueen = 10;
		public static int ProtectionValueKing = 0;

		public static int HangingPiecePenalty = -50;
		public static float MovementBonusMultiplier = 2.5f;
		public static int PawnsOnSameFilePenalty = -30;
		public static int PawnCantMovePenalty = -40;
		public static int KnightForksBonus = 60;
		public static int CannotCastlePenalty = -60;
		public static float KingMovementBonusMultiplier = 10;
		public static int RookGuardsBackRankBonus = 10;
		public static int RookOnOpenFileBonus = 20;

		public static int CastledBonus = 30;

		public static short ReadTable(short[] table, int pos, int color)
		{
			int x = Board.X(pos);
			int y = Board.Y(pos);

			int location = (color == Colors.White) ? x + (7 - y) * 8 : x + y * 8;
			return table[location];
		}

		public static int GetPieceValue(int piece)
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

		public static int GetProtectionValue(int piece)
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

		public static short[] GetPositionTable(int piece)
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

		public static readonly short[] NullTable = new short[]
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

		public static readonly short[] PawnTable = new short[]
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

		public static readonly short[] KnightTable = new short[]
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

		public static readonly short[] BishopTable = new short[]
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

		public static readonly short[] KingTable = new short[]
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

		public static readonly short[] KingTableEndGame = new short[]
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
