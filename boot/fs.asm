
; es:bx = destination of bootloader
fs_load_bootloader:
    pusha

    ; Read Header
    mov dx, 0
    mov ax, 1
    mov cx, 1
    call disk_read

    ;Check signature
    call fs_sig_check

    ;get bootloader location
    mov esi, 2                    ; bootsector + fs header
    add esi, dword [es:bx+16]     ; index sector count
    add esi, dword [es:bx+20]     ; bitmap sector count
    add esi, dword [es:bx+32]     ; bootloader sector (within data section)
    ; move esi into dx:ax
    mov ax, si
    ror esi, 16
    mov dx, si

    ;get bootloader length in sectors
    mov esi, dword [es:bx+36]
    mov cx, si
    
    ;Load bootloader
    call disk_read
    popa
    ret

fs_sig_check:
    pusha
    mov cx, 0
    mov si, bx
    mov di, fs_signature_v10
fs_sig_check_next:
    mov ax, cx
    mov bl, [es:si]
    cmp bl, [di]
    jne fs_error
    inc cx
    inc si
    inc di
    cmp cx, 7
    jne fs_sig_check_next
fs_sig_check_ok:
    popa
    ret

fs_error:
    mov si, str_fs_sigmismatch
    call printstr
    call printnl
    mov si, 0x0500
    mov cx, 24
    call printhex
    jmp $

fs_signature_v10 db 0x6b, 0x4f, 0x53, 0x2e, 0x46, 0x53, 0x01 ; kOS.FS, major version = 1