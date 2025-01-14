package days

// https://adventofcode.com/2024/day/25 - Code Chronicle

import (
	aoc "aoc2024/aocutils"
)

type day25 struct {
	day int
}

func Day25() {
	d := &day25{25}
	d.Run()
}

func (d *day25) Run() {
	_, err := aoc.GetDayInputLines(d.day)
	aoc.CheckError(err)

	aoc.DayHeader(d.day)
	aoc.PrintResult(1, 0)
	aoc.PrintResult(2, 0)
}
