.DEFAULT_GOAL := all
ASM=nasm
ASM_FLAGS=-f bin -O0

SOURCE=boot.asm

../bin/boot: $(wildcard *.asm) 
	$(ASM) $(ASM_FLAGS) -o $@ $(SOURCE)

.PHONY: all
all: ../bin/boot