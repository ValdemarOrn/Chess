# I am a comment, and I want to say that the variable CC will be
# the compiler to use.
CC=g++
# Hey!, I am comment number 2. I want to say that CFLAGS will be the
# options I'll pass to the compiler.
CFLAGS=-c -Wall -m64

OUTPUT=Chess.Lib.dll

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
	$(CC) -shared $(SRC) -o bin/$(OUTPUT)

Objects:
	$(CC) $(CFLAGS) $(SRC)
	
clean:
	@echo "Cleaning..."
	rm -rf *.o
	rm -rf Moves/*.o