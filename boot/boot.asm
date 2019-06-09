[org 0x7c00]

mov [diskid], dl    ; save boot disk id
jmp Main

%include "print.asm"
%include "disk.asm"

Main:
    mov si, str_banner
    call printstr

    mov ah, 0x08
    mov dl, [diskid]
    int 0x13
    jc readerr

    push dx
    mov dx, cx
    call printword
    call printnl
    pop dx
    call printword
    call printnl

    jmp loopy
readerr:
    mov si, str_readerr
    call printstr
    mov [temp], ah
    mov si, temp
    mov dx, 1
    call printhex
    call printnl

loopy:
    jmp $


;global variables
diskid db 0x00
temp db 0x00

;strings
str_banner db 13, 10, 'kOS Booting', 13, 10, 0x00
str_readerr db 'Read Failed ', 0x00


; padding and magic number
times 510 - ($-$$) db 0
dw 0xaa55
