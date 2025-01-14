package days

// https://adventofcode.com/2024/day/23 - LAN Party

import (
	aoc "aoc2024/aocutils"
)

type day23 struct {
	day int
}

func Day23() {
	d := &day23{23}
	d.Run()
}

func (d *day23) Run() {
	_, err := aoc.GetDayInputLines(d.day)
	aoc.CheckError(err)

	aoc.DayHeader(d.day)
	aoc.PrintResult(1, 0)
	aoc.PrintResult(2, 0)
}
