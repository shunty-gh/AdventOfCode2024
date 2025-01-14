package days

// https://adventofcode.com/2024/day/5 - Print Queue

import (
	aoc "aoc2024/aocutils"
	"slices"
)

func Day05() {
	const DAY = 5
	lines, err := aoc.GetDayInputLines(DAY)
	aoc.CheckError(err)

	section1 := true
	rules := make(map[int][]int)
	pages := make([][]int, 0)
	for _, line := range lines {
		if line == "" {
			section1 = false
			continue
		}
		if section1 {
			rr, _ := aoc.StringToInts(line, "|")
			if v, ok := rules[rr[0]]; ok {
				rules[rr[0]] = append(v, rr[1])
			} else {
				rules[rr[0]] = []int{rr[1]}
			}

			if _, ok := rules[rr[1]]; !ok {
				rules[rr[1]] = make([]int, 0)
			}

		} else {
			pp, _ := aoc.StringToInts(line, ",")
			pages = append(pages, pp)
		}
	}

	pageComparer := func(a, b int) int {
		if slices.Contains(rules[a], b) {
			return 1
		}
		return -1
	}

	part1, part2 := 0, 0
	for _, pp := range pages {
		pass := true
		for i := 0; i < len(pp)-1; i++ {
			pn := pp[i]
			if !slices.Contains(rules[pn], pp[i+1]) {
				pass = false
				break
			}
		}

		if pass {
			part1 += pp[(len(pp)-1)/2]
		} else {
			slices.SortFunc(pp, pageComparer)
			part2 += pp[(len(pp)-1)/2]
		}
	}

	aoc.DayHeader(DAY)
	aoc.PrintResult(1, part1)
	aoc.PrintResult(2, part2)
}
