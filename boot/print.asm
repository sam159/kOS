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
; DX = length
printhex:
    pusha
    xor cx,cx               ; Clear counter
    mov ah, 0x0e            ; TTY
printhex_loop:
    cmp cx, dx              ; return when counter = DX
    je printhex_ret
    mov bx, [si]            ; Get char at si
    and bx, 0x00F0          ; Output the first half of the byte
    shr bx, 4
    add bx, hexmap          ; Add the start address of the lookup table
    mov al, byte [bx]       ; Get the char in the lookup table
    int 0x10                ; Print hex char
    mov bx, [si]            ; Do same for second hex char of byte
    and bx, 0x000F
    add bx, hexmap
    mov al, byte [bx]
    int 0x10
    inc si                  ; Increment data and counter
    inc cx
    jmp printhex_loop
printhex_ret:
    popa
    ret

hexmap db 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46