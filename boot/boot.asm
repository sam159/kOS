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
    
    mov [heads], dh

    mov al, ch
    mov ah, cl
    and ah, 0xC0
    shr ah, 6
    mov [cyls], ax

    mov al, cl
    and al, 0x3F
    mov [sectors], al

    mov dx, [cyls]
    call printword
    call printnl

    mov dx, 0
    mov dl, [sectors]
    call printword
    call printnl

    mov dx, 0
    mov dl, [heads]
    call printword
    call printnl

    jmp loopy
readerr:
    mov si, str_readerr
    call printstr
    mov dx, 0
    mov dl, ah
    call printbyte
    call printnl

loopy:
    jmp $


;global variables
diskid db 0x00
cyls dw 0x0000
sectors db 0x00
heads db 0x00

;strings
str_banner db 13, 10, 'kOS Booting', 13, 10, 0x00
str_readerr db 'Read Failed: ', 0x00


; padding and magic number
times 510 - ($-$$) db 0
dw 0xaa55
