[org 0x7c00]

mov [diskid], dl    ; save boot disk id
jmp Main

%include "print.asm"
%include "disk.asm"

Main:
    mov si, str_banner
    call printstr

    mov dl, [diskid]
    call disk_select

    mov ax, 0x0050
    mov es, ax

    mov dx, 0
    mov ax, 1
    mov cx, 1
    mov bx, 0
    call disk_read

    mov si, 0x0500
    mov cx, 512
    call printhex

    jmp loopy

loopy:
    jmp $


;global variables
diskid db 0x00

;strings
str_banner db 13, 10, 'kOS Booting', 13, 10, 0x00
str_readerr db 'Disk Error: ', 0x00


; padding and magic number
times 510 - ($-$$) db 0
dw 0xaa55
