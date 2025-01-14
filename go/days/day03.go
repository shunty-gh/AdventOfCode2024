package days

// https://adventofcode.com/2024/day/3 - Mull It Over

import (
	aoc "aoc2024/aocutils"
	"regexp"
)

func Day03() {
	const DAY = 3
	input, err := aoc.GetDayInputString(DAY)
	aoc.CheckError(err)

	reP1 := regexp.MustCompile(`mul\(\d+,\d+\)`)
	matches := reP1.FindAllString(input, -1)

	part1 := 0
	for _, match := range matches {
		part1 += mul(match)
	}
	aoc.DayHeader(DAY)
	aoc.PrintResult(1, part1)

	reP2 := regexp.MustCompile(`mul\(\d+,\d+\)|do\(\)|don\'t\(\)`)
	matches = reP2.FindAllString(input, -1)

	part2 := 0
	do := true
	for _, match := range matches {
		switch match {
		case "do()":
			do = true
		case "don't()":
			do = false
		default:
			if do {
				part2 += mul(match)
			}
		}
	}
	aoc.PrintResult(2, part2)
}

func mul(mulStr string) int {
	s := mulStr[4 : len(mulStr)-1] // cut off 'mul(' and ')'
	nums, err := aoc.StringToInts(s, ",")
	aoc.CheckError(err)
	return nums[0] * nums[1]
}
