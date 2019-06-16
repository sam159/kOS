; SI = start of string
printstr:
    push ax
    mov ah, 0x0e            ; TTY
printstr_loop:
    lodsb                   ; Load string into al and increment si
    cmp al, byte 0x00       ; return if char is 0
    je printstr_ret
    int 0x10                ; print char
    jmp printstr_loop
printstr_ret:
    pop ax
    ret

printnl:
    push ax
    mov ah, 0x0e
    mov al, 0x0a            ; newline
    int 0x10
    mov al, 0x0d            ; carriage return
    int 0x10
    pop ax
    ret

; SI = start of data
; CX = length
printhex:
    pusha
    mov bx, 24
printhex_loop:
    test cx, cx              ; return when cx = 0
    jz printhex_ret
    mov dl, byte [si]        ; Get byte at si
    call printbyte
    inc si                   ; Increment data and counter
    dec cx
    dec bx
    jz printhex_nl
    jmp short printhex_loop
printhex_nl:
    mov bx, 24
    call printnl
    jmp short printhex_loop
printhex_ret:
    popa
    ret

; DX = data
printword:
    push dx
    ror dx, 8
    call printbyte
    ror dx, 8
    call printbyte
    pop dx
    ret

; DL = byte
printbyte:
    push bx
    push ax
    mov ah, 0x0e            ; TTY
    mov bx, dx
    and bx, 0x00F0
    shr bx, 4
    call printnibble
    mov bx, dx
    and bx, 0x000F
    call printnibble
    pop ax
    pop bx
    ret

printnibble:
    cmp bl, 0x09
    ja printnibble_GT9
    add bl, 0x30
    jmp printnibble_out
printnibble_GT9:
    add bl, 0x37
printnibble_out:
    mov al, bl
    int 0x10
    ret