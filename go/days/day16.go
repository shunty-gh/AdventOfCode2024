package days

// https://adventofcode.com/2024/day/16 - Reindeer Maze

import (
	aoc "aoc2024/aocutils"
	"slices"
)

type day16 struct {
	day, xLen, yLen int
	wall            byte
}

func Day16() {
	d := &day16{16, 101, 103, '#'}
	d.Run()
}

func (d *day16) Run() {
	input, err := aoc.GetDayInputLines(d.day)
	aoc.CheckError(err)

	d.xLen = len(input[0])
	d.yLen = len(input)
	grid := aoc.NewGridFromLines[byte](input)
	start, _ := grid.Find('S')
	end, _ := grid.Find('E')

	aoc.DayHeader(d.day)
	part1 := d.bestPath(grid, start, end)
	aoc.PrintResult(1, part1)
	part2 := d.bestPaths(grid, start, end, part1)
	aoc.PrintResult(2, part2)
}

type d16qItem struct {
	loc  aoc.Point2d
	path []aoc.Point2d
	dir  aoc.Direction
	cost int
}

// Get the best score for a path from start to end
func (d *day16) bestPath(grid *aoc.Grid2d[byte], start aoc.Point2d, target aoc.Point2d) int {
	seen := aoc.NewSet[aoc.PointDirection]()
	q := []d16qItem{{start, nil, aoc.DirectionE, 0}}
	for len(q) > 0 {
		// The crucial bit. A fake priority queue.
		slices.SortFunc(q, func(i, j d16qItem) int { return i.cost - j.cost })
		curr := q[0]
		q = q[1:]

		if curr.loc == target {
			return curr.cost
		}
		k := aoc.PointDirection{Loc: curr.loc, Dir: curr.dir}
		if seen.Contains(k) {
			continue
		}
		seen.Add(k)

		for _, dir := range aoc.Directions4() {
			nx := curr.loc.AddDir(dir)
			if c, ok := grid.TryGet(nx); ok && c != d.wall {
				nxcost := curr.cost + 1
				if dir != curr.dir {
					nxcost += 1000 // A turn AND a move
				}
				q = append(q, d16qItem{nx, nil, dir, nxcost})
			}
		}
	}
	return -1 // This should never happen
}

// Find the unique points in all paths that have a score equal to bestScore
func (d *day16) bestPaths(grid *aoc.Grid2d[byte], start aoc.Point2d, target aoc.Point2d, bestScore int) int {
	seen := make(map[aoc.PointDirection]int)
	seen[aoc.PointDirection{Loc: start, Dir: aoc.DirectionE}] = 0
	bestset := aoc.NewSet[aoc.Point2d]()

	q := []d16qItem{{start, []aoc.Point2d{start}, aoc.DirectionE, 0}}
	for len(q) > 0 {
		curr := q[0]
		q = q[1:]

		// Do our existence and validity checks *before* adding to the queue
		// as opposed to after popping off the queue. This will work fine as
		// we already know the exact target score we're looking for.

		for _, dir := range aoc.Directions4() {
			nx := curr.loc.AddDir(dir)
			nxcost := curr.cost + 1
			if dir != curr.dir {
				nxcost += 1000 // A turn AND a move
			}
			if nx == target && bestScore == nxcost {
				bestset.AddSlice(curr.path)
				bestset.Add(nx)
				continue
			}
			if nxcost >= bestScore {
				continue
			}
			if c, ok := grid.TryGet(nx); ok && c != d.wall {
				pd := aoc.PointDirection{Loc: nx, Dir: dir}
				// If the cost of this move is greater than the cost we've
				// already seen at this point then skip it
				if v, ok := seen[pd]; ok && v < nxcost {
					continue
				}
				seen[pd] = nxcost
				newpath := make([]aoc.Point2d, len(curr.path)+1)
				copy(newpath, curr.path)
				newpath[len(curr.path)-1] = nx
				q = append(q, d16qItem{nx, newpath, dir, nxcost})
			}
		}
	}

	return bestset.Count()
}
