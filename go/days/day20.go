package days

// https://adventofcode.com/2024/day/20 - Race Condition

import (
	aoc "aoc2024/aocutils"
)

type day20 struct {
	day              int
	xLen, yLen       int
	wall, start, end byte
}

func Day20() {
	d := &day20{20, 0, 0, '#', 'S', 'E'}
	d.Run()
}

func (d *day20) Run() {
	input, err := aoc.GetDayInputLines(d.day)
	aoc.CheckError(err)
	d.xLen = len(input[0])
	d.yLen = len(input)

	grid := aoc.NewGridFromLinesIgnore[byte](input, []byte{'.'})
	start, _ := grid.Find(d.start)
	end, _ := grid.Find(d.end)

	fromStart := d.bfs(grid, start)
	fromEnd := d.bfs(grid, end)

	aoc.DayHeader(d.day)
	aoc.PrintResult(1, d.part1(fromStart, fromEnd, end))
	aoc.PrintResult(2, d.part2(fromStart, fromEnd, end))
}

func (d *day20) part1(fromStart *aoc.Counter[aoc.Point2d], fromEnd *aoc.Counter[aoc.Point2d], target aoc.Point2d) int {
	cheatThreshold := 100
	withoutCheating := fromStart.Get(target)
	result := 0

	// for each point in fromStart, see if there is a viable cheat that involves one piece of wall
	// total rac distance will be the distance up to the given point + 2 for the cheat + distance
	// from the point at which we returned to the track to the end
	for _, from := range fromStart.Items() {
		p0 := from.Key
		if p0 == target {
			continue
		}

		for _, dir1 := range aoc.Directions4() {
			p1 := p0.AddDir(dir1)
			// Needs to be a wall otherwise it's not cheating. ie it must not be in the fromStart map
			if fromStart.Contains(p1) {
				continue
			}

			// Now we need to look for a piece of track that we can get back on to
			for _, dir2 := range aoc.Directions4() {
				p2 := p1.AddDir(dir2)
				if fromStart.Contains(p2) {
					// Check if it was worth the cheat - ie will it save us at least 100 picoseconds
					cheatdist := from.Value + 2 + fromEnd.Get(p2)
					if withoutCheating-cheatdist >= cheatThreshold {
						result++
					}
				}
			}
		}
	}
	return result
}

func (d *day20) part2(fromStart *aoc.Counter[aoc.Point2d], fromEnd *aoc.Counter[aoc.Point2d], target aoc.Point2d) int {
	cheatThreshold := 100
	cheatLimit := 20
	withoutCheating := fromStart.Get(target)
	result := 0

	for _, from := range fromStart.Items() {
		p0 := from.Key
		if p0 == target {
			continue
		}

		// Any piece of track that is within a Manhattan distance of 'cheatLimit' from
		// the current point is a potential valid cheat
		for dx := -cheatLimit; dx <= cheatLimit; dx++ {
			for dy := -cheatLimit; dy <= cheatLimit; dy++ {
				mhdist := aoc.Abs(dx) + aoc.Abs(dy)
				if mhdist <= cheatLimit {
					p1 := p0.Add(dx, dy)
					if distto, ok := fromEnd.TryGet(p1); ok && p0 != p1 {
						cheatdist := from.Value + mhdist + distto
						if withoutCheating-cheatdist >= cheatThreshold {
							result++
						}
					}
				}
			}
		}
	}

	return result
}

func (d *day20) bfs(grid *aoc.Grid2d[byte], start aoc.Point2d) *aoc.Counter[aoc.Point2d] {
	result := aoc.NewCounter[aoc.Point2d]()
	result.Set(start, 0) // Make sure to SET to 0 rather than .Add(), which will make it 1
	q := make([]aoc.Point2d, 0)
	q = append(q, start)

	for len(q) > 0 {
		curr := q[0]
		q = q[1:]
		cost := result.Get(curr)

		for _, dir := range aoc.Directions4() {
			nx := curr.AddDir(dir)
			if nx.InRange0(d.xLen, d.yLen) && grid.CharAt(nx) != d.wall {
				if v, ok := result.TryGet(nx); !ok || v > cost+1 {
					result.Set(nx, cost+1)
					q = append(q, nx)
				}
			}
		}
	}

	return result
}
