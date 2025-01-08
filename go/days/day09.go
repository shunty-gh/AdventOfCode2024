package days

import (
	aoc "aoc2024/aocutils"
	"fmt"
	"os"
	"slices"
)

// Probably a lot more complicated than it needs to be

type fsBlock struct {
	fileId     int
	index      int
	len        int
	next, prev *fsBlock
}

func (fb *fsBlock) String() string {
	ni, pi := " nil", " nil"
	if fb.prev != nil {
		pi = fmt.Sprintf("%4d", fb.prev.index)
	}
	if fb.next != nil {
		ni = fmt.Sprintf("%4d", fb.next.index)
	}
	return fmt.Sprintf("{%2d, %4d, %d N:%s; P:%s}", fb.fileId, fb.index, fb.len, ni, pi)
}

func (fb *fsBlock) checksum() int64 {
	if fb.fileId < 0 {
		return 0
	}
	var result int64 = int64(fb.fileId) * int64(fb.index)
	for nx := fb.next; nx != nil; nx = nx.next {
		result += int64(nx.fileId) * int64(nx.index)
	}

	for prv := fb.prev; prv != nil; prv = prv.prev {
		result += int64(prv.fileId) * int64(prv.index)
	}
	return result
}

const TestInput = "2333133121414131402"

func Day09() {
	const DAY = 9
	var input string
	var err error
	if slices.Contains(os.Args, "-t") {
		input = TestInput
	} else {
		input, err = aoc.GetDayInputString(DAY)
		aoc.CheckError(err)
	}

	aoc.DayHeader(DAY)
	diskMap, files, freeSpace := buildFileSystem(input + string('0'))
	aoc.PrintResult(1, defragPart1(diskMap, files, freeSpace))

	aoc.PrintResult(2, defragPart2(input))
}

func defragPart1(diskMap map[int]*fsBlock, files []*fsBlock, freeSpace *fsBlock) int64 {
	// Take the last block from the list. Reallocate each file block into
	// the next available free space block. Update the free space first element
	for ep := len(diskMap) - 1; ep > 0; ep-- {
		movefrom := diskMap[ep]
		if movefrom.fileId < 0 {
			continue
		}
		moveto := freeSpace.next
		if movefrom.index <= moveto.index {
			break
		}

		// Update freespace head - ie remove moveto from free space list
		freeSpace.next = moveto.next
		moveto.next.prev = freeSpace

		// Swap from/to indexes & fs map pointers
		movefrom.index, moveto.index = moveto.index, movefrom.index
		diskMap[movefrom.index] = movefrom
		diskMap[moveto.index] = moveto

		// Don't need to bother about the moved free space
	}

	var result int64 = 0
	for _, f := range files {
		result += f.checksum()
	}
	return result
}

// The linked list fsBlock approach is way too complicated for part 2
type fsBlock2 struct {
	fileId int
	index  int
	len    int
}

func (fb *fsBlock2) String() string {
	return fmt.Sprintf("{ %4d, %4d, %d }", fb.fileId, fb.index, fb.len)
}

func defragPart2(input string) int64 {
	in := input
	if len(in)%2 != 0 {
		in += "0"
	}
	ilen := len(in)
	allblocks := make([]*fsBlock2, 0, ilen)
	index := 0
	for i := 0; i < ilen; i += 2 {
		fid := i / 2
		flen := int(in[i] - '0')
		fsb := fsBlock2{fid, index, flen}
		allblocks = append(allblocks, &fsb)
		index += flen

		slen := int(in[i+1] - '0')
		// Don't add zero length space blocks
		if slen > 0 {
			sp := fsBlock2{-1, index, slen}
			allblocks = append(allblocks, &sp)
		}
		index += slen
	}

	ep := len(allblocks) - 1
	for ; ep > 0; ep-- {
		if allblocks[ep].fileId < 0 {
			// Skip space blocks
			continue
		}
		fsb := allblocks[ep]
		// Find free space to fit
		for si := 0; si < len(allblocks); si++ {
			sp := allblocks[si]
			if sp.fileId >= 0 || sp.len < fsb.len {
				// Too small or is not a space block
				continue
			}
			if sp.index > fsb.index {
				// No available space, move on
				break
			}

			var newfsb fsBlock2
			if sp.len == fsb.len {
				// Just make the entire space block into a file block
				sp.fileId = fsb.fileId
			} else {
				// Create a new file block, copy details of fsb and change index
				newfsb = fsBlock2{fsb.fileId, sp.index, fsb.len}
				// Shrink the existing space block
				sp.index += fsb.len
				sp.len -= fsb.len
				// Insert new file block before shrunk space block
				allblocks = slices.Insert(allblocks, si, &newfsb)
				// IMPORTANT because we've inserted a new block
				ep += 1
			}

			// Change fsb block into a space block
			fsb.fileId = -1

			// Merge any space blocks around the old file block
			// The block is at index 'ep'
			// Merge block below/right if it is a space
			if ep+1 < len(allblocks) && allblocks[ep+1].fileId < 0 {
				fsb.len += allblocks[ep+1].len
				allblocks[ep+1].len = 0
				allblocks[ep+1].index = fsb.index + fsb.len
			}

			// Merge into block above/left if it is a space
			if ep > 0 && allblocks[ep-1].fileId < 0 {
				allblocks[ep-1].len += fsb.len
				fsb.len = 0
				fsb.index = allblocks[ep-1].index + allblocks[ep-1].len
			}
			// We could get rid of 0 len space blocks by re-slicing but
			// it isn't really necessary

			break
		}
	}

	// Calculate checksum
	var result int64 = 0
	for _, f := range allblocks {
		//fmt.Println(f)
		if f.fileId < 0 {
			continue
		}
		for i := 0; i < f.len; i++ {
			result += int64(f.fileId) * int64(f.index+i)
		}
	}

	return result
}

func buildFileSystem(input string) (diskMap map[int]*fsBlock, files []*fsBlock, freeSpace *fsBlock) {
	var index = 0
	fls := make([]*fsBlock, 0, len(input)/2)
	fsp := &fsBlock{}
	freeTail := fsp
	fs := make(map[int]*fsBlock)

	for i := 0; i < len(input)-1; i += 2 {
		// File
		fid := i / 2
		flen := int(input[i] - '0')
		fb := fsBlock{fid, index, flen, nil, nil}
		fs[index] = &fb
		fls = append(fls, &fb)
		fsb := &fb
		for fi := 1; fi < int(flen); fi++ {
			fbb := fsBlock{fid, index + fi, flen, nil, fsb}
			fsb.next = &fbb
			fsb = &fbb
			fs[index+fi] = &fbb
		}
		index += flen

		// Space
		slen := int(input[i+1] - '0')
		for si := 0; si < slen; si++ {
			sp := fsBlock{-1, index + si, slen, nil, freeTail}
			freeTail.next = &sp
			freeTail = &sp
			fs[index+si] = &sp
		}
		index += slen
	}

	return fs, fls, fsp
}

// func printDiskMap(diskMap map[int]*fsBlock, count int) {
// 	lindex, lfieldid := "", ""
// 	for i := 0; i < count; i++ {
// 		if blk, ok := diskMap[i]; ok {
// 			fmt.Println(blk)
// 			lindex = lindex + fmt.Sprintf("%4d ", blk.index)
// 			if blk.fileId < 0 {
// 				lfieldid = lfieldid + "   . "
// 			} else {
// 				lfieldid = lfieldid + fmt.Sprintf("%4d ", blk.fileId)
// 			}
// 		} else {
// 			break
// 		}
// 	}
// 	fmt.Println(lindex)
// 	fmt.Println(lfieldid)
// }
