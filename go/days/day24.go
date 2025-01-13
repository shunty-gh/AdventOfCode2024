package days

import (
	aoc "aoc2024/aocutils"
)

type day24 struct {
	day int
}

func Day24() {
	d := &day24{24}
	d.Run()
}

func (d *day24) Run() {
	_, err := aoc.GetDayInputLines(d.day)
	aoc.CheckError(err)

	aoc.DayHeader(d.day)
	aoc.PrintResult(1, 0)
	aoc.PrintResult(2, 0)
}
