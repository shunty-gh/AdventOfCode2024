package days

import (
	aoc "aoc2024/aocutils"
	"fmt"
)

type pointDirection struct {
	p aoc.Point2d
	d aoc.Direction
}

func (p pointDirection) String() string {
	return fmt.Sprintf("P2D: P:%v; D:%v,", p.p, p.d)
}

func Day06() {
	const DAY = 6
	lines, err := aoc.GetDayInputLines(DAY)
	aoc.CheckError(err)

	xlen, ylen := len(lines[0]), len(lines)
	grid := aoc.NewGridFromLinesIgnore(lines, []byte{'.'})
	start, err := grid.Find('^')
	aoc.CheckError(err)

	part1 := make(map[aoc.Point2d]bool)
	part1[start] = true
	dir := aoc.Direction{X: 0, Y: -1}
	p := aoc.Point2d(start)
	for {
		nx := aoc.Point2d{X: p.X + dir.X, Y: p.Y + dir.Y}
		if !aoc.InRange0(nx.X, nx.Y, xlen, ylen) { // We've wandered out of the map
			break
		}
		c := grid.CharAt(nx)
		if c == '#' { // An obstacle, turn right
			dir = dir.TurnRight()
		} else {
			part1[nx] = true
			p = nx
		}
	}

	// For part 2 try putting an obstacle at each point along the original
	// path and see if this causes the guard to walk in an endless loop
	part2 := 0
	for pt := range part1 {
		p2seen := make(map[pointDirection]bool)
		dir = aoc.Direction{X: 0, Y: -1}
		p = aoc.Point2d(start)
		p2seen[pointDirection{start, dir}] = true
		for {
			nx := aoc.Point2d{X: p.X + dir.X, Y: p.Y + dir.Y}
			if !aoc.InRange0(nx.X, nx.Y, xlen, ylen) { // We've wandered out of the map
				break
			}
			c := grid.CharAt(nx)
			pd := pointDirection{nx, dir}
			if c == '#' || nx == pt { // An obstacle, turn right
				dir = dir.TurnRight()
			} else if _, ok := p2seen[pd]; ok {
				part2++
				break
			} else {
				p2seen[pd] = true
				p = nx
			}
		}
	}

	aoc.DayHeader(DAY)
	aoc.PrintResult(1, len(part1))
	aoc.PrintResult(2, part2)
}
