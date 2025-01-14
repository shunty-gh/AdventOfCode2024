package days

// https://adventofcode.com/2024/day/10 - Hoof It

import (
	aoc "aoc2024/aocutils"
)

func Day10() {
	const DAY = 10
	input, err := aoc.GetDayInputLines(DAY)
	aoc.CheckError(err)

	grid := aoc.NewGridFromLines[int](input)
	// Get start points
	starts, err := grid.FindAll(0)
	aoc.CheckError(err)

	part1, part2 := 0, 0
	for _, start := range starts {
		p1, p2 := trailScores(grid, start)
		part1 += p1
		part2 += p2
	}

	aoc.DayHeader(DAY)
	aoc.PrintResult(1, part1)
	aoc.PrintResult(2, part2)
}

func trailScores(grid *aoc.Grid2d[int], start aoc.Point2d) (int, int) {
	q := make([]aoc.Point2d, 0)
	peaks := make(map[aoc.Point2d]bool) // Unique peaks reached, for part 1
	routes := 0                         // Total number of times a peak is reached, for part 2
	q = append(q, start)
	for len(q) > 0 {
		c := q[0]
		q = q[1:]

		cv := grid.CharAt(c)
		for _, dir := range aoc.Directions4() {
			nx := aoc.Point2d{X: c.X + dir.X, Y: c.Y + dir.Y}
			v := grid.CharAt(nx)
			if v == 9 && cv == 8 {
				peaks[nx] = true
				routes++
			} else if v == cv+1 {
				q = append(q, nx)
			}
		}
	}
	return len(peaks), routes
}
