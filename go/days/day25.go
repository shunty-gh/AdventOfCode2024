package days

// https://adventofcode.com/2024/day/25 - Code Chronicle

import (
	aoc "aoc2024/aocutils"
	"fmt"
)

type day25 struct {
	day int
}

func Day25() {
	d := &day25{25}
	d.Run()
}

func (d *day25) Run() {
	input, err := aoc.GetDayInputLines(d.day)
	aoc.CheckError(err)

	keys := make([][5]int, 0)
	locks := make([][5]int, 0)
	for i := 0; i < len(input); i += 8 {
		v := [5]int{0, 0, 0, 0, 0}
		for j := 0; j < 5; j++ {
			sum := 0
			for k := 0; k < 7; k++ {
				if input[i+k][j] == '#' {
					sum++
				}
			}
			v[j] = sum
		}
		if input[i][0] == '#' {
			locks = append(locks, v)
		} else {
			keys = append(keys, v)
		}
	}

	pairs := aoc.NewSet[string]()
	for li, lock := range locks {
		for ki, key := range keys {
			// Do they overlap
			overlap := false
			for i := 0; i < 5; i++ {
				if lock[i]+key[i] > 7 {
					overlap = true
					break
				}
			}
			if !overlap {
				pairs.Add(fmt.Sprintf("%d_%d", li, ki))
			}
		}
	}
	aoc.DayHeader(d.day)
	aoc.PrintResult(1, pairs.Count())
}
