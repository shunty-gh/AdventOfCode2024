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
	// Doing this with go routines is a lot quicker than the 'linear, point
	// by point' approach.
	part2 := make(chan int)   // output channel to provide part 2 result
	ch := make(chan int, 512) // give it an arbitrary, reasonable capacity, there's about 5k possible obstacles to try

	// Read each answer from the 'in' channel, fed by the walkMap
	// func. When it has read 'count' entries push the sum onto the
	// 'out' channel and quit.
	go func(in, out chan int, count int) {
		result := 0
		for c := 0; c < count; c++ {
			result += <-in
		}
		out <- result
	}(ch, part2, len(part1))

	// Only try obstacles on the original part1 route
	for pt := range part1 {
		go walkMapLookForLoop(grid, start, pt, xlen, ylen, ch)
	}

	aoc.DayHeader(DAY)
	aoc.PrintResult(1, len(part1))
	aoc.PrintResult(2, <-part2)
}

// Walk the grid area, with an added obstacle at a given point. If this results in a
// loop then push a 1 onto the output channel otherwise, if we wander out of bounds,
// push 0 onto the channel
func walkMapLookForLoop(grid *aoc.Grid2d, start aoc.Point2d, obstacle aoc.Point2d, xlen, ylen int, ch chan int) {
	seen := make(map[pointDirection]bool)
	dir := aoc.Direction{X: 0, Y: -1} // start by going up
	p := aoc.Point2d(start)
	seen[pointDirection{start, dir}] = true
	for {
		nx := aoc.Point2d{X: p.X + dir.X, Y: p.Y + dir.Y}
		if !aoc.InRange0(nx.X, nx.Y, xlen, ylen) { // We've wandered out of the map
			ch <- 0
			break
		}
		c := grid.CharAt(nx)
		pd := pointDirection{nx, dir}
		if c == '#' || nx == obstacle { // An obstacle, turn right
			dir = dir.TurnRight()
		} else if _, ok := seen[pd]; ok {
			ch <- 1
			break
		} else {
			seen[pd] = true
			p = nx
		}
	}
}
