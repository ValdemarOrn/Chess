using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Chess.Lib.EGTB
{
	public class State
	{
		public ulong Hash;
		public ulong NextState;
		public byte DepthToMate;
		public byte PlayerTurn;
		public byte[] Pieces;
		public byte[] PieceLocation;

		public State()
		{ }

		public State(State copy)
		{
			Hash = copy.Hash;
			NextState = copy.NextState;
			DepthToMate = copy.DepthToMate;
			PlayerTurn = copy.PlayerTurn;
			Pieces = new byte[] { copy.Pieces[0], copy.Pieces[1], copy.Pieces[2], copy.Pieces[3] };
			PieceLocation = new byte[] { copy.PieceLocation[0], copy.PieceLocation[1], copy.PieceLocation[2], copy.PieceLocation[3] };
		}
	}

	unsafe class Program
	{
		const byte MAX = 255;
		const byte STALEMATE = 254;

		static void Main(string[] args)
		{
			Zob.Load();
			Chess.Lib.Manager.InitLibrary();

			var p = new Program();
			p.Execute();
		}

		public Dictionary<ulong, State> States;

		public void Execute()
		{ 
			var pieces = new byte[3];
			pieces[0] = Board.COLOR_WHITE | Board.PIECE_KING;
			pieces[1] = Board.COLOR_WHITE | Board.PIECE_ROOK;
			pieces[2] = Board.COLOR_BLACK | Board.PIECE_KING;
			States = new Dictionary<ulong, State>();

			GenerateMates(pieces);

			// which ply we are looking for (how many moves to mate?)
			int searchPly = 1;

			// the player whose turn it is
			byte color = Board.COLOR_WHITE;

			// the set of states we are currently working with
			// we send this into the Crunch method, and that method tries to find
			// any states that can lead to the input states. It then returns a list of
			// those states and it becomes our new stateSet
			List<State> stateSet = States.Where(x => x.Value.DepthToMate == 0).Select(x => x.Value).ToList();

			while (true)
			{
				// Todo: Can I re-use the old state set instead of selecting a new set?
				//stateSet = States.Where(x => x.Value.DepthToMate == searchPly - 1).Select(x => x.Value).ToList();

				if (color == Board.COLOR_WHITE)
				{
					Console.WriteLine("Ply Depth " + searchPly + ", Running White");
					stateSet = CalculateWhite(stateSet);
				}
				else
				{
					Console.WriteLine("Ply Depth " + searchPly + ", Running Black");
					stateSet = CalculateBlack(stateSet);
					// Todo: Can I *not* save the stateSet for the black moves in the States dictionary?
				}

				searchPly++;

				// Todo: Can I stop looking after I get a stateSet with zero new states?
				stateSet = AddStates(stateSet);
				if (searchPly > 50)
					break;

				Console.WriteLine("States added: " + stateSet.Count);
				color = (color == Board.COLOR_WHITE) ? (byte)Board.COLOR_BLACK : (byte)Board.COLOR_WHITE;
			}

			Console.WriteLine("Complete, total states: " + States.Count);
			Verify();
			WriteToFile();
			

			while(true)
			{
				try { QueryStates(pieces); }
				catch(Exception e) { Console.WriteLine("An error occured: ", e.Message); }
			}
		}

		private void Verify()
		{
			var mates = States.Count(x => x.Value.DepthToMate == 0);
			var stale = States.Count(x => x.Value.DepthToMate == STALEMATE);
			var d1 = States.Count(x => x.Value.DepthToMate == 1);
			var d2 = States.Count(x => x.Value.DepthToMate == 2);
			var d3 = States.Count(x => x.Value.DepthToMate == 3);

			foreach(var kvp in States)
			{
				var state = kvp.Value;
				var dom = state.DepthToMate;
				var next = state.NextState;

				if (dom == 0 || dom == STALEMATE)
					continue;

				if(!States.ContainsKey(next))
					throw new Exception("Table does not contain next state");

				var nextState = States[state.NextState];
				if(dom <= nextState.DepthToMate)
					throw new Exception("Next state is not closer to mate");

			}
		}

		private void WriteToFile()
		{
			StringBuilder sb = new StringBuilder();
			var ordered = States.Where(x => x.Value.PlayerTurn == Board.COLOR_WHITE).OrderBy(x => x.Key).Select(x => x.Value).ToArray();
			var fs = System.IO.File.OpenWrite(@"c:\KR-K.txt");
			foreach(var state in ordered)
			{
				fs.Write(BitConverter.GetBytes(state.Hash), 0, 8);
				fs.Write(BitConverter.GetBytes(state.NextState), 0, 8);
				fs.WriteByte(state.DepthToMate);
			}
			fs.Close();
		}

		public bool QueryStates(byte[] pieces)
		{
			Console.Write("Query Position (wK, wQ, bK):");
			var line = Console.ReadLine();
			if (line == "quit" || line == "exit")
				return false;

			var locations = line.Split(',').Select(x => Convert.ToByte(x.Trim())).ToArray();
			var stateTemp = new State();
			stateTemp.PieceLocation = locations;
			stateTemp.Pieces = pieces;
			stateTemp.PlayerTurn = Board.COLOR_WHITE;
			var hash = CalcHash(stateTemp);

			State state = null;

			var d = States.FirstOrDefault(x => x.Value.PieceLocation[0] == locations[0] && x.Value.PieceLocation[1] == locations[1] && x.Value.PieceLocation[2] == locations[2]);

			if (States.ContainsKey(hash))
				state = States[hash];

			Console.WriteLine("Hash: " + state.Hash);
			Console.WriteLine("Depth to Mate: " + state.DepthToMate);
			Console.WriteLine("Position: " + state.PieceLocation[0] + ", " + state.PieceLocation[1] + ", " + state.PieceLocation[2]);

			if (States.ContainsKey(state.NextState))
			{
				var next = States[state.NextState];
				Console.WriteLine("Next Position: " + next.PieceLocation[0] + ", " + next.PieceLocation[1] + ", " + next.PieceLocation[2]);
			}

			Console.WriteLine("");
			return true;
		}
		
		public List<State> CalculateBlack(List<State> stateSet)
		{
			// find only moves where black MUST move to a state in the stateSet
			
			var newStates = new List<State>();
			var b = Board.Create();

			foreach (var state in stateSet)
			{
				var originalHash = state.Hash;

				state.PlayerTurn = Board.COLOR_BLACK;
				state.Hash = CalcHash(state);
				SetupBoard(b, state, Board.COLOR_BLACK);
				var moves = GetMovesReverse(b, Board.COLOR_BLACK);

				for (int i = 0; i < moves.Length; i++)
				{
					var from = moves[i][0];
					var to = moves[i][1];

					// set player to black and set the position to the "precursor" state
					state.PlayerTurn = Board.COLOR_BLACK;
					MovePieceState(state, to, from);
					SetupBoard(b, state, Board.COLOR_BLACK);
					b->CurrentMove = 0;
					if(AreKingsTouching(b))
					{
						// revert the state back to the original state
						state.PlayerTurn = Board.COLOR_WHITE;
						MovePieceState(state, from, to);
						continue;
					}

					// get moves from precursor state to any other available states
					var moveBitboard = Moves.GetMoves(b, from);
					var movesFromPreviousPos = Bitboard.Bitboard_BitList(moveBitboard);

					// We see if there is a way for black to get out of a forced mate by looking
					// for any moves that escape it. If white has a forced mate, the input state (original state)
					// is supposedly the best move black could make.
					// IF black can make moves tho states that aren't already explored, then they lead to a better
					// state for black, thus he can escape
					bool canEscape = false;

					for(int j=0; j < movesFromPreviousPos.Length; j++)
					{
						var backTo = movesFromPreviousPos[j];
						var valid = Board.Make(b, from, backTo);
						if (valid)
						{
							Board.Unmake(b);

							// Check if this is a capture. We can't "un-capture" pieces so this would be a way out
							// for black.
							if (Bitboard.Get(b->Boards[Board.BOARD_WHITE], backTo))
							{
								canEscape = true;
								break;
							}

							// find the hash for the next possible state
							state.PlayerTurn = Board.COLOR_WHITE;
							MovePieceState(state, from, backTo);
							var nextHash = state.Hash;
							state.PlayerTurn = Board.COLOR_BLACK;
							MovePieceState(state, backTo, from);
							
							// check if that is a known position
							if(!States.ContainsKey(nextHash))
							{
								canEscape = true;
								break;
							}
						}
					}

					// If black can not escape the then moving to the original state is the best option black has.
					if (!canEscape)
					{
						var newState = new State(state);
						newState.NextState = originalHash;
						newState.DepthToMate++;
						newStates.Add(newState);
					}

					// revert the state back to the original state
					state.PlayerTurn = Board.COLOR_WHITE;
					MovePieceState(state, from, to);
				}

				// We messed with the state... alot. 
				// Make sure there isn't a bug in the code and we just corrupted something :)
				if (originalHash != state.Hash)
					throw new Exception("State was altered");
			}

			Board.Delete(b);
			return newStates;
		}

		public List<State> CalculateWhite(List<State> stateSet)
		{
			// find all states where white can transition from a state into one of the input states in stateSet			

			var newStates = new List<State>();
			var b = Board.Create();

			foreach(var state in stateSet)
			{
				var originalHash = state.Hash;

				state.PlayerTurn = Board.COLOR_WHITE;
				state.Hash = CalcHash(state);
				SetupBoard(b, state, Board.COLOR_WHITE);
				var moves = GetMovesReverse(b, Board.COLOR_WHITE);

				for (int i = 0; i < moves.Length; i++)
				{
					var from = moves[i][0];
					var to = moves[i][1];

					// set player to white and set the position to the "precursor" state
					state.PlayerTurn = Board.COLOR_WHITE;
					MovePieceState(state, to, from);
					SetupBoard(b, state, Board.COLOR_WHITE);
					b->CurrentMove = 0;

					// we must see the the position we are moving from already had black in check
					// Such a position could never actually occur, since can't enter check by himself
					bool wasChecked = Board.IsChecked(b, Board.COLOR_BLACK) == 1;

					if (!wasChecked)
					{
						var valid = Board.Make(b, from, to);

						if (valid)
						{
							var newState = new State(state);
							newState.NextState = originalHash;
							newState.DepthToMate++;
							newStates.Add(newState);
						}
					}

					// revert the state back to the original state
					state.PlayerTurn = Board.COLOR_BLACK;
					MovePieceState(state, from, to);
				}

				if (originalHash != state.Hash)
					throw new Exception("State was altered");
			}

			Board.Delete(b);
			return newStates;
		}

		/// <summary>
		/// Checks for the invalid state where kings are attacking each other
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		private bool AreKingsTouching(BoardStruct* b)
		{
			var whiteKing = Bitboard.ForwardBit(b->Boards[Board.BOARD_WHITE] & b->Boards[Board.BOARD_KINGS]);
			var blackKingBoard = b->Boards[Board.BOARD_BLACK] & b->Boards[Board.BOARD_KINGS];

			var attacks = Moves.GetAttacks(b, whiteKing);
			return ((blackKingBoard & attacks) > 0);
		}

		/*private bool AreKingsTouching(State s)
		{
			var x1 = s.PieceLocation[0] % 8;
			var y1 = s.PieceLocation[0] >> 3;

			var x2 = s.PieceLocation[2] % 8;
			var y2 = s.PieceLocation[2] >> 3;

			return (Math.Abs(x1 - x2) <= 1 && Math.Abs(y1 - y2) <= 1);
		}*/

		/// <summary>
		/// Gets all moves that a piece can make that will RESULT IN the given position.
		/// All pieces can reverse their own move, so backwards move are the same as forwards
		/// except for pawns, they only move one way.
		/// </summary>
		/// <param name="b"></param>
		/// <param name="playerColor"></param>
		/// <returns></returns>
		private unsafe byte[][] GetMovesReverse(BoardStruct* b, byte playerColor)
		{
			var moves = new List<byte[]>();

			byte[] locations = null;

			if (playerColor == Board.COLOR_WHITE)
				locations = Bitboard.Bitboard_BitList(b->Boards[Board.BOARD_WHITE]);
			else
				locations = Bitboard.Bitboard_BitList(b->Boards[Board.BOARD_BLACK]);

			for (byte i = 0; i < locations.Length; i++)
			{
				var to = locations[i];
				var pieceMoves = Moves.GetMoves(b, to);

				// filter out all captures.. remember, this is in reverse!
				pieceMoves = pieceMoves & ~(b->Boards[Board.BOARD_WHITE] | b->Boards[Board.BOARD_BLACK]);

				var sources = Bitboard.Bitboard_BitList(pieceMoves);

				for (int j = 0; j < sources.Length; j++)
					moves.Add(new byte[] { sources[j], to });
			}

			return moves.ToArray();
		}

		#region Mate Generation

		/// <summary>
		/// Checks a position and determines if black is mated
		/// </summary>
		/// <param name="state"></param>
		/// <param name="b"></param>
		public void CheckIfMate(State state, BoardStruct* b)
		{
			SetupBoard(b, state, Board.COLOR_BLACK);

			var from = state.PieceLocation[2];
			var moveBoard = Moves.GetMoves(b, from);
			var moves = Bitboard.Bitboard_BitList(moveBoard);

			// make sure the black king can't directly attack the white king, that's an illegal position
			if (AreKingsTouching(b))
				return;

			bool isMate = true;

			for (int j = 0; j < moves.Length; j++)
			{
				var valid = Board.Make(b, from, moves[j]);

				if (valid)
				{
					Board.Unmake(b);
					isMate = false;
					break;
				}
			}

			if (isMate)
			{
				if (Board.IsChecked(b, Board.COLOR_BLACK) == 1)
					state.DepthToMate = 0;
				else
					state.DepthToMate = STALEMATE;
			}


		}

		/// <summary>
		/// Generates all possible states, with black to move.
		/// Used to find all mated positions
		/// </summary>
		/// <param name="pieces"></param>
		public void GenerateMates(byte[] pieces)
		{
			List<State> states = null;

			if (pieces.Length == 3)
				states = Generate3PieceMates(pieces);
			else if (pieces.Length == 4)
				states = Generate4PieceMates(pieces);
			else
				throw new Exception("This generator only supports 3 and 4 pieces tablebases");

			int empty = states.Count(x => x.Pieces == null || x.Hash == 0 || x.DepthToMate == 255);
			Assert(empty == 0, "State generation failed!");

			AddStates(states);
		}

		private List<State> Generate3PieceMates(byte[] pieces)
		{
			var b = Board.Create();
			var states = new List<State>();

			byte p0 = pieces[0];
			byte p1 = pieces[1];
			byte p2 = pieces[2];

			int i = 0;

			for (byte l0 = 0; l0 < 64; l0++)
				for (byte l1 = 0; l1 < 64; l1++)
					for (byte l2 = 0; l2 < 64; l2++)
						if (l0 != l1 && l0 != l2 && l1 != l2)
						{
							var state = new State();
							state.Pieces = new byte[4];
							state.PieceLocation = new byte[4];
							state.PlayerTurn = Board.COLOR_BLACK;

							// set pieces
							state.Pieces[0] = p0;
							state.PieceLocation[0] = l0;
							state.Pieces[1] = p1;
							state.PieceLocation[1] = l1;
							state.Pieces[2] = p2;
							state.PieceLocation[2] = l2;

							state.DepthToMate = MAX; // set to max
							state.Hash = CalcHash(state);
							CheckIfMate(state, b);

							if (state.DepthToMate != 255)
								states.Add(state);

							i++;
						}

			Assert(i == 64 * 63 * 62);

			Board.Delete(b);
			var mateCount = states.Count(x => x.DepthToMate == 0);
			var stalemateCount = states.Count(x => x.DepthToMate == STALEMATE);

			return states;
		}

		private List<State> Generate4PieceMates(byte[] pieces)
		{
			var states = new List<State>();
			return states;
		}

		#endregion

		#region Helpers

		public void MovePieceState(State state, byte moveFrom, byte moveTo)
		{
			if (state.PieceLocation[0] == moveFrom)
				state.PieceLocation[0] = moveTo;
			if (state.PieceLocation[1] == moveFrom)
				state.PieceLocation[1] = moveTo;
			if (state.PieceLocation[2] == moveFrom)
				state.PieceLocation[2] = moveTo;

			state.Hash = CalcHash(state);
		}

		/// <summary>
		/// Adds states the master States dictionary. Checks for hash collisions
		/// </summary>
		/// <param name="newStates"></param>
		public List<State> AddStates(List<State> newStates)
		{
			var statesAdded = new Dictionary<ulong, State>();

			for (int i = 0; i < newStates.Count; i++)
			{
				var newState = newStates[i];

				if (!IsValidState(newState))
					continue;

				// check for collisions, or if state is already included
				if (States.ContainsKey(newState.Hash))
				{
					var existingState = States[newState.Hash];

					bool equal = Compare(existingState, newState);
					if (!equal)
						throw new Exception("Hash collision!");

					// don't overwrite if the existing state has a shorter depth to mate
					if (existingState.DepthToMate < newState.DepthToMate)
						continue;
				}

				States[newState.Hash] = newState;
				statesAdded[newState.Hash] = newState;
			}

			return statesAdded.Select(x => x.Value).ToList();
		}

		BoardStruct* bs = Board.Create();

		private bool IsValidState(State state)
		{
			SetupBoard(bs, state, state.PlayerTurn);
			if(AreKingsTouching(bs))
				return false;

			var oppositePlayer = (state.PlayerTurn == Board.COLOR_WHITE) ? Board.COLOR_BLACK : Board.COLOR_WHITE;

			if(Board.IsChecked(bs, oppositePlayer) == 1)
				return false;

			return true;
		}

		private bool Compare(State state1, State state2)
		{
			var a = state1.PieceLocation[0] == state2.PieceLocation[0];
			var b = state1.PieceLocation[1] == state2.PieceLocation[1];
			var c = state1.PieceLocation[2] == state2.PieceLocation[2];
			var d = state1.PlayerTurn == state2.PlayerTurn;

			return a & b & c & d;
		}
		
		private unsafe void SetupBoard(BoardStruct* b, State state, byte playerTurn)
		{
			b->Boards[0] = 0;
			b->Boards[1] = 0;
			b->Boards[2] = 0;
			b->Boards[3] = 0;
			b->Boards[4] = 0;
			b->Boards[5] = 0;
			b->Boards[6] = 0;
			b->Boards[7] = 0;
			b->Castle = 0;
			b->EnPassantTile = 0;
			b->Hash = 0;
			Board.SetPiece(b, state.PieceLocation[0], state.Pieces[0] & 0x0F, state.Pieces[0] & 0xF0);
			Board.SetPiece(b, state.PieceLocation[1], state.Pieces[1] & 0x0F, state.Pieces[1] & 0xF0);
			Board.SetPiece(b, state.PieceLocation[2], state.Pieces[2] & 0x0F, state.Pieces[2] & 0xF0);
			b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);
			b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);

			b->PlayerTurn = playerTurn;
			b->Hash = b->Hash ^ Zob.Keys[Zobrist.ZOBRIST_SIDE, b->PlayerTurn];

			Assert(b->Hash == state.Hash, "Hashes do not match");
		}

		private ulong CalcHash(State state)
		{
			ulong hash = 0;
			hash ^= Zob.Keys[Zob.Index[state.Pieces[0]], state.PieceLocation[0]];
			hash ^= Zob.Keys[Zob.Index[state.Pieces[1]], state.PieceLocation[1]]; 
			hash ^= Zob.Keys[Zob.Index[state.Pieces[2]], state.PieceLocation[2]];
			hash ^= Zob.Keys[Zobrist.ZOBRIST_SIDE, state.PlayerTurn];
			return hash;
		}

		/// <summary>
		/// Calculate hash of position after a move
		/// </summary>
		/// <param name="state"></param>
		/// <param name="moveFrom"></param>
		/// <param name="moveTo"></param>
		/// <returns></returns>
		private ulong CalcHash(State state, byte moveFrom, byte moveTo)
		{
			if (state.PieceLocation[0] == moveFrom)
				state.PieceLocation[0] = moveTo;
			if (state.PieceLocation[1] == moveFrom)
				state.PieceLocation[1] = moveTo;
			if (state.PieceLocation[2] == moveFrom)
				state.PieceLocation[2] = moveTo;

			var hash = CalcHash(state);

			if (state.PieceLocation[0] == moveTo)
				state.PieceLocation[0] = moveFrom;
			if (state.PieceLocation[1] == moveTo)
				state.PieceLocation[1] = moveFrom;
			if (state.PieceLocation[2] == moveTo)
				state.PieceLocation[2] = moveFrom;

			return hash;
		}
		
		public void Assert(bool constraint, string message = "Assert failed")
		{
			if (constraint == false)
				throw new Exception(message);
		}

		#endregion
	}
}
