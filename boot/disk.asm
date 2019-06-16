
;DL = disk
disk_select:
    pusha
    mov [disk_curid], dl
    mov ah, 0x08
    int 0x13
    jc disk_err

    mov [disk_heads], dh

    mov al, ch
    mov ah, cl
    and ah, 0xC0
    shr ah, 6
    mov [disk_cyls], ax

    mov al, cl
    and al, 0x3F
    mov [disk_sectors], al

    popa
    ret

; DX:AX = LBA
; CX = sectors to read
; ES:BX = buffer
disk_read:
    pusha
disk_read_continue:
    mov di, 3               ; 3 attempts to read sector
disk_read_retry:
    pusha

    div word [disk_sectors] ; divide DX:AX by number of sectors
    ; ax = LBA / Sectors
    ; dx = LBA % Sectors = sector on cylinder - 1

    ;CX =       ---CH--- ---CL---
    ;cylinder : 76543210 98
    ;sector   :            543210
    mov cx, dx
    inc cx

    xor dx,dx               ; clear dx
    div word [disk_heads]
    ; ax = (LBA / Sectors) / Heads = cylinder
    ; dx = (LBA / Sectors) % Heads = head

    mov ch, al
    shl ah, 6
    or cl, ah

    mov dh, dl
    mov dl, [disk_curid]

    mov ah, 0x02 ; Read function
    mov al, 0x01 ; Read 1 sector

    int 0x13
    jnc disk_read_sector_ok

    xor ah, ah
    int 0x13

    popa
    dec di      ; decrement attempts
    jnz disk_read_retry
    jmp disk_err

disk_read_sector_ok:
    popa
    dec cx
    jz disk_read_done

    add bx, 512
    add ax, 1
    adc dx, 0
    jmp disk_read_continue

disk_read_done:
    popa
    ret

disk_err:
    mov si, str_readerr
    call printstr
    mov dx, 0
    mov dl, ah
    call printbyte
    call printnl
    jmp $

;variables 
disk_curid db 0x00
disk_errcode db 0x00
disk_cyls dw 0x0000
disk_sectors db 0x00
disk_heads db 0x00