.DEFAULT_GOAL := build
DISKSIZE=16

boot/boot:
	$(MAKE) -C boot

bin/disk.raw: boot/boot
	-[[ ! -f "$@" ]] && dd if=/dev/zero of=$@ bs=1048576 count=$(DISKSIZE)
	dd if=boot/boot of=$@ bs=512 count=1 conv=notrunc

bin/fs.dll: tools/fs/bin/Debug/netcoreapp2.2/fs.dll
	-[[ ! -f "$@" ]] && ln -s ../tools/fs/bin/Debug/netcoreapp2.2/fs.dll $@

.PHONY: tools/fs/bin/Debug/netcoreapp2.2/fs.dll
tools/fs/bin/Debug/netcoreapp2.2/fs.dll:
	dotnet build tools/fs/fs.csproj

.PHONY: build
build: bin/disk.raw bin/fs.dll

.PHONY: run
run: build
	qemu-system-i386 --cpu pentium -m 64 -drive file=bin/disk.raw,if=ide,media=disk,format=raw

.PHONY: clean
clean:
	-rm bin/*
	$(MAKE) -C boot clean