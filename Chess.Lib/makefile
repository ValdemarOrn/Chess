
CC=g++

CFLAGS=-Wall -fkeep-inline-functions -fpic -m64
LFLAGS=-fpic -fkeep-inline-functions -m64

ifeq ($(mode), debug)
	CFLAGS += -g
else
	CFLAGS += -O0
endif

OUTPUT=Chess.Lib.so
	
OBJ=Moves/Bishop.o \
	Moves/King.o \
	Moves/Knight.o \
	Moves/Pawn.o \
	Moves/Rook.o \
	Bitboard.o \
	Board.o \
	Eval.o \
	EvalData.o \
	Manager.o \
	Move.o \
	Moves.o \
	Order.o \
	Perft.o \
	Search.o \
	SEE.o \
	TTable.o \
	Zobrist.o \
	ZobristKeys.o

install: all
	@echo === Installing shared library to /usr/lib/ ===
	mv bin/$(OUTPUT) /usr/lib/$(OUTPUT)
	
all: release
	@echo === Starting linker ===
	mkdir -p bin
	$(CC) -shared $(LFLAGS) $(OBJ) -o bin/$(OUTPUT)
	@echo === File Linked ===

release: $(OBJ)
	@echo === All objects compiled ===

%.o : %.cpp
	$(CC) -c $(CFLAGS) $< -o $@

clean:
	@echo === Cleaning Files ===
	rm -rf *.o
	rm -rf Moves/*.o
	rm -rf bin/*.so
	rm -rf bin/*.dll