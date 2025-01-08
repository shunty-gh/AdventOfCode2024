package days

import (
	aoc "aoc2024/aocutils"
	"strconv"
)

func Day11() {
	const DAY = 11
	input, err := aoc.GetDayInputString(DAY)
	aoc.CheckError(err)

	stones, err := aoc.StringToInts64(input, " ")
	aoc.CheckError(err)

	cache := make(map[cacheItem]int64)
	aoc.DayHeader(DAY)
	aoc.PrintResult(1, blinkAtStones(stones, 25, cache))
	aoc.PrintResult(2, blinkAtStones(stones, 75, cache))
}

type cacheItem struct {
	stone int64
	count int
}

func blinkAtStones(stones []int64, count int, cache map[cacheItem]int64) int64 {
	var result int64 = 0
	for _, stone := range stones {
		result += blinkAtStone(stone, count, cache)
	}
	return result
}

func blinkAtStone(stone int64, count int, cache map[cacheItem]int64) int64 {
	ci := cacheItem{stone, count}
	if v, ok := cache[ci]; ok {
		return v
	}

	stonestr := strconv.FormatInt(stone, 10)
	if count == 1 && len(stonestr)%2 == 0 {
		return 2
	} else if count == 1 {
		return 1
	}

	var result int64
	switch {
	case len(stonestr)%2 == 0:
		splitlen := len(stonestr) / 2
		l, _ := strconv.ParseInt(stonestr[:splitlen], 10, 64)
		r, _ := strconv.ParseInt(stonestr[splitlen:], 10, 64)
		result = blinkAtStone(l, count-1, cache) + blinkAtStone(r, count-1, cache)
	case stone == 0:
		result = blinkAtStone(1, count-1, cache)
	default:
		result = blinkAtStone(stone*2024, count-1, cache)
	}
	cache[ci] = result
	return result
}
