package days

import (
	aoc "aoc2024/aocutils"
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

	part1, part2 := d.bestPath(grid, start, end)
	aoc.DayHeader(d.day)
	aoc.PrintResult(1, part1)
	aoc.PrintResult(2, part2)
}

type d16qItem struct {
	loc  aoc.Point2d
	path []aoc.Point2d
	dir  aoc.Direction
	cost int
}

func (d *day16) bestPath(grid *aoc.Grid2d[byte], start aoc.Point2d, target aoc.Point2d) (int, int) {
	seen := make(map[aoc.PointDirection]int)
	seen[aoc.PointDirection{Loc: start, Dir: aoc.DirectionE}] = 0
	bestpaths := make(map[int][][]aoc.Point2d)

	q := make([]d16qItem, 0)
	q = append(q, d16qItem{start, []aoc.Point2d{start}, aoc.DirectionE, 0})
	bestscore := 0
	for len(q) > 0 {
		curr := q[0]
		q = q[1:]

		if bestscore > 0 && curr.cost >= bestscore {
			continue
		}

		for _, dir := range aoc.Directions4() {
			nx := curr.loc.AddDir(dir)
			nxcost := curr.cost + 1
			if dir != curr.dir {
				nxcost = curr.cost + 1001 // A turn AND a move
			}
			if bestscore > 0 && nxcost > bestscore {
				continue
			}
			if nx == target {
				if bestscore == 0 || bestscore >= nxcost { // '>=' so we get all possible best paths
					bestscore = nxcost
					newpath := append([]aoc.Point2d{}, curr.path...)
					newpath = append(newpath, nx)
					pp := bestpaths[nxcost]
					pp = append(pp, newpath)
					bestpaths[nxcost] = pp
					continue
				}
			}
			if c, ok := grid.TryGet(nx); ok && c != d.wall {
				pd := aoc.PointDirection{Loc: nx, Dir: dir}
				// This comparison has to be '<' as opposed to '<=' because
				// in P2 we want all the possible best paths
				if v, ok := seen[pd]; ok && v < nxcost {
					continue
				}
				seen[pd] = nxcost
				newpath := append([]aoc.Point2d{}, curr.path...)
				newpath = append(newpath, nx)
				q = append(q, d16qItem{nx, newpath, dir, nxcost})
			}
		}
	}

	bp := bestpaths[bestscore]
	bestset := make(map[aoc.Point2d]bool)
	for _, p := range bp {
		for _, pp := range p {
			bestset[pp] = true
		}
	}
	return bestscore, len(bestset)
}
