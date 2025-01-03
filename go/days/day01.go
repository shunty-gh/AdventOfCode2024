package days

import (
	aoc "aoc2024/aocutils"
	"sort"
)

func Day01() {
	lines, err := aoc.GetDayInputLines(1)
	aoc.CheckError(err)

	l := make([]int, len(lines))
	r := make([]int, len(lines))
	lunique := make(map[int]bool)
	rmap := make(map[int]int)
	for i, line := range lines {
		nums, err := aoc.StringToInts(line, "   ")
		aoc.CheckError(err)
		l[i] = nums[0]
		r[i] = nums[1]

		lunique[nums[0]] = true
		rmap[nums[1]] = rmap[nums[1]] + 1
	}

	sort.Ints(l)
	sort.Ints(r)
	part1 := 0
	for i := 0; i < len(l); i++ {
		d := aoc.Abs(l[i] - r[i])
		part1 += d
	}

	aoc.DayHeader(1)
	aoc.PrintResult(1, part1)

	part2 := 0
	for v := range lunique {
		part2 += rmap[v] * v
	}
	aoc.PrintResult(2, part2)
}
