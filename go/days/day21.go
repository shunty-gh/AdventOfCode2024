package days

import (
	aoc "aoc2024/aocutils"
)

type day21 struct {
	day int
}

func Day21() {
	d := &day21{21}
	d.Run()
}

func (d *day21) Run() {
	_, err := aoc.GetDayInputLines(d.day)
	aoc.CheckError(err)

	aoc.DayHeader(d.day)
	aoc.PrintResult(1, 0)
	aoc.PrintResult(2, 0)
}
