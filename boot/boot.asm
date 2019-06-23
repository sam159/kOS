[org 0x7c00]

mov [boot_diskid], dl    ; save boot disk id
jmp Main

%include "print.asm"
%include "disk.asm"
%include "fs.asm"

Main:
    call cls
    mov si, str_banner
    call printstr
    call printnl

    mov dl, [boot_diskid]
    call disk_select

    mov si, str_loading
    call printstr

    mov ax, 0x0050
    mov es, ax
    mov bx, 0
    call fs_load_bootloader

    mov si, str_ok
    call printstr
    call printnl

    jmp loopy

loopy:
    jmp $

boot_diskid db 0x00

;strings
str_banner db 'kOS', 0x00
str_loading db 'Reading Bootloader... ', 0x00
str_ok db 'OK', 0x00
str_readerr db 'E!DSK', 0x00
str_fs_sigmismatch db 'E!FSSIG', 0x00

; padding and magic number
times 510 - ($-$$) db 0
dw 0xaa55
