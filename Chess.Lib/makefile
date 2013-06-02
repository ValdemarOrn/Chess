
CC=g++
CFLAGS=-c -Wall -m64
LFLAGS=-shared -fpic -m64

OUTPUT=Chess.Lib.so

SRC=Moves/Bishop.cpp \
	Moves/King.cpp \
	Moves/Knight.cpp \
	Moves/Pawn.cpp \
	Moves/Rook.cpp \
	Bitboard.cpp \
	Board.cpp \
	Eval.cpp \
	EvalData.cpp \
	Manager.cpp \
	Move.cpp \
	Moves.cpp \
	Order.cpp \
	Perft.cpp \
	Search.cpp \
	SEE.cpp \
	TTable.cpp \
	Zobrist.cpp \
	ZobristKeys.cpp
	
all: Chess.Lib

Chess.Lib: Objects
	mkdir -p bin
	$(CC) $(LFLAGS) $(SRC) -o bin/$(OUTPUT)

Objects:
	$(CC) $(CFLAGS) $(SRC)
	
clean:
	@echo "Cleaning..."
	rm -rf *.o
	rm -rf Moves/*.o