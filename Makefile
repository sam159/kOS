.DEFAULT_GOAL := all
DISKSIZE=16

bin/boot:
	$(MAKE) -C boot

bin/disk.raw: bin/boot
	[[ ! -f "$@" ]] && dd if=/dev/zero of=$@ bs=1048576 count=$(DISKSIZE)
	dd if=bin/boot of=$@ bs=512 count=1 conv=notrunc

.PHONY: all
all: bin/disk.raw

.PHONY: run
run: all
	qemu-system-i386 --cpu pentium -m 64 -drive file=bin/disk.raw,if=ide,media=disk,format=raw

.PHONY: clean
clean:
	rm bin/*