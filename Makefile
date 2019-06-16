.DEFAULT_GOAL := build
DISK_SECTORS=2879
DISK_INDEX_PC=10
ROOTFS_FILES := $(shell find rootfs/ -type f)
TOOLS_FS_FILES := $(shell find tools/fs -type f -name "*.cs")
 
boot/boot: boot/*.asm
	$(MAKE) -C boot

bin/disk.raw: boot/boot bin/disk.part
	cat boot/boot bin/disk.part > $@

bin/fs.dll: tools/fs/bin/Debug/netcoreapp2.2/fs.dll
	[[ -f "$@" ]] || ln -s ../tools/fs/bin/Debug/netcoreapp2.2/fs.dll $@

bin/disk.part: bin/fs.dll $(ROOTFS_FILES)
	dotnet bin/fs.dll $@ $(DISK_SECTORS) $(DISK_INDEX_PC) rootfs/

tools/fs/bin/Debug/netcoreapp2.2/fs.dll: $(TOOLS_FS_FILES)
	dotnet build tools/fs/fs.csproj

.PHONY: build
build: bin/disk.raw bin/fs.dll

.PHONY: run
run: build
	qemu-system-i386 --cpu pentium -m 64 -drive file=bin/disk.raw,if=floppy,media=disk,format=raw -boot a

.PHONY: clean
clean:
	rm bin/*
	dotnet clean tools/fs/fs.csproj
	$(MAKE) -C boot clean