[org 0x7c00]

mov [diskid], dl    ; save boot disk id
jmp Main

%include "print.asm"
%include "disk.asm"

Main:
    mov si, banner
    call printstr
    
    jmp $

;global variables
diskid db 0x00

;strings
banner db 13, 10, 'kOS Booting', 13, 10, 0x00

; padding and magic number
times 510 - ($-$$) db 0
dw 0xaa55
