# kOS Filesystem
Version 1.0

Unless stated all numbers are unsigned with little-endian byte order.

## General Layout

512 byte sectors

| Content     | Size                           |
| ----------- | ------------------------------ |
| Header      | 1 sector                       |
| Indexes     | 64 bytes for 8 per sector      |
| Data Bitmap | 1 sector per 4096 data sectors |
| Data        | 1+ sectors                     |

## Header

| Byte Offset | Size      | Content                                              |
| ----------- |---------- | ---------------------------------------------------- |
| 0           | 6 bytes   | Magic identifier. ASCII 'kOS.FS' (6b 4f 53 2e 46 53) |
| 6           | 1 byte    | Major Version                                        |
| 7           | 1 byte    | Minor Version                                        |
| 8           | 4 bytes   | Size of file system in sectors                       |
| 12          | 4 bytes   | index count                                          |
| 16          | 4 bytes   | Index sector count (index count / 8)                 |
| 20          | 4 bytes   | Bitmap sector count                                  |
| 24          | 4 bytes   | Size of bitmap in bytes                              |
| 28          | 4 bytes   | index ID of bootloader file                          |
| 32          | 408 bytes | Reserved / Zero                                      |

Number of data sectors can be inferred from sector count - index sectors - bitmap sectors.

## Index Node

64 Bytes in length. 8 per 512 byte sector.

| Byte Offset | Size     | Content                |
| ----------- | -------- | ---------------------- |
| 0           | 4 bytes  | ID                     |
| 4           | 2 bytes  | Flags (see below)      |
| 6           | 4 bytes  | Parent ID              |
| 10          | 2 bytes  | Device ID              |
| 12          | 4 bytes  | Total Length of data   |
| 16          | 4 bytes  | Index of data sector   |
| 20          | 4 bytes  | Number of data sectors |
| 24          | 8 bytes  | Reserved / Zero        |
| 32          | 32 bytes | Name, nul terminated   |

### Flags

| Value | Name       | Notes                                                                       |
| ----- | ---------- | ----------------------------------------------------------------------------|
| 1     | Directory  | Only a directory or non-contigious file may have children                   |
| 2     | Valid      | Unless set index can be considered unused                                   |
| 4     | Device     | Is a special file/directory identified by the Device ID field               |
| 8     | HasData    | Unless set ignore data sector index and size                                |
| 16    | Contigious | Content of file is in one chunk, if not child indexes contain the remainder |

Bootloader must be a file and contigious.

## Bitmap

The data bitmap uses one bit per sector to represent if a sector is used by an index.

It is organised left to right ignoring byte order, that is the first sector will be bit 7 of the first byte (76543210).

## Data sectors

The data sectors only contain data, no header/meta data is present.