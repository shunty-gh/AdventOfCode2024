package days

// https://adventofcode.com/2024/day/8 - Resonant Collinearity

import (
	aoc "aoc2024/aocutils"
)

func Day08() {
	const DAY = 8
	lines, err := aoc.GetDayInputLines(DAY)
	aoc.CheckError(err)

	xlen := len(lines[0])
	ylen := len(lines)
	antennas := make(map[byte][]aoc.Point2d)
	for y := 0; y < ylen; y++ {
		line := lines[y]
		for x := 0; x < xlen; x++ {
			c := line[x]
			if c == '.' {
				continue
			}
			if _, ok := antennas[c]; !ok {
				antennas[c] = make([]aoc.Point2d, 0)
			}
			antennas[c] = append(antennas[c], aoc.Point2d{X: x, Y: y})
		}
	}

	antinodesP1 := make(map[aoc.Point2d]bool)
	antinodesP2 := make(map[aoc.Point2d]bool)
	for _, ants := range antennas {
		for ai := 0; ai < len(ants)-1; ai++ {
			for aj := ai + 1; aj < len(ants); aj++ {
				// Get the distance apart
				dx, dy := ants[aj].X-ants[ai].X, ants[aj].Y-ants[ai].Y

				// For part 2 the each antenna will also be part of the answer
				antinodesP2[ants[ai]] = true
				antinodesP2[ants[aj]] = true

				mul := 0
				okH, okL := true, true
				for okH || okL {
					mul++
					// Distance before first is 2*dist from second and dist after second is 2*dist from first
					// Part 1 only counts when mul == 1. For part 2 we add multiples of dx,dy.
					pH := ants[ai].Add(-mul*dx, -mul*dy)
					pL := ants[aj].Add(mul*dx, mul*dy)

					if okH && pH.InRange0(xlen, ylen) {
						if mul == 1 {
							antinodesP1[pH] = true
						}
						antinodesP2[pH] = true
					} else {
						okH = false
					}

					if okL && pL.InRange0(xlen, ylen) {
						if mul == 1 {
							antinodesP1[pL] = true
						}
						antinodesP2[pL] = true
					} else {
						okL = false
					}
				}
			}
		}
	}

	aoc.DayHeader(DAY)
	aoc.PrintResult(1, len(antinodesP1))
	aoc.PrintResult(2, len(antinodesP2))
}
