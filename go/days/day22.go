package days

import (
	aoc "aoc2024/aocutils"
)

type day22 struct {
	day int
}

func Day22() {
	d := &day22{22}
	d.Run()
}

func (d *day22) Run() {
	_, err := aoc.GetDayInputLines(d.day)
	aoc.CheckError(err)

	aoc.DayHeader(d.day)
	aoc.PrintResult(1, 0)
	aoc.PrintResult(2, 0)
}
