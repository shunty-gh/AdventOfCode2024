package days

import (
	aoc "aoc2024/aocutils"
	"fmt"
)

type day18 struct {
	day, xLen, yLen int
}

func Day18() {
	d := &day18{18, 71, 71}
	d.Run()
}

func (d *day18) Run() {
	input, err := aoc.GetDayInputLines(d.day)
	aoc.CheckError(err)

	bytes := make([]aoc.Point2d, 0, len(input))
	for _, ln := range input {
		nums, _ := aoc.StringToInts(ln, ",")
		bytes = append(bytes, aoc.Point2d{X: nums[0], Y: nums[1]})
	}

	aoc.DayHeader(d.day)
	part1 := d.minSteps(bytes, 1024)
	aoc.PrintResult(1, part1)
	p2 := d.part2(bytes, part1)
	part2 := bytes[p2]
	aoc.PrintResult(2, fmt.Sprintf("%d,%d", part2.X, part2.Y))
}

func (d *day18) minSteps(bytes []aoc.Point2d, byteCount int) int {
	corruption := aoc.ToSet(bytes[:byteCount])
	seen := make(map[aoc.Point2d]int)
	q := make([]aoc.Point2d, 0)
	start := aoc.NewPoint2d(0, 0)
	target := aoc.NewPoint2d(d.xLen-1, d.yLen-1)
	seen[start] = 0
	q = append(q, start)
	for len(q) > 0 {
		curr := q[0]
		q = q[1:]

		cost := seen[curr]
		nxcost := cost + 1
		for _, dir := range aoc.Directions4() {
			nx := curr.AddDir(dir)
			if nx.InRange0(d.xLen, d.yLen) && !corruption.Contains(nx) {
				if c, ok := seen[nx]; !ok || c > nxcost {
					seen[nx] = nxcost
					q = append(q, nx)
				}
			}
		}
	}

	if c, ok := seen[target]; ok {
		return c
	}
	return 0
}

func (d *day18) part2(bytes []aoc.Point2d, part1 int) int {
	// from part 1 we know we're safe for at least 'part1' counts, so start there
	// and increase by 100 until the path is blocked then go back one by one
	var i int
	for i = part1 + 100; ; i += 100 {
		if !d.pathExists(bytes, i) {
			break
		}
	}
	// go back one by one
	for i -= 1; i > 0; i-- {
		if d.pathExists(bytes, i) {
			return i
		}
	}
	// this should never happen, assuming the data is valid for the question
	return -1
}

func (d *day18) pathExists(bytes []aoc.Point2d, byteCount int) bool {
	corruption := aoc.ToSet(bytes[:byteCount])
	seen := aoc.NewSet[aoc.Point2d]()
	q := make([]aoc.Point2d, 0)
	start := aoc.NewPoint2d(0, 0)
	target := aoc.NewPoint2d(d.xLen-1, d.yLen-1)
	seen.Add(start)
	q = append(q, start)
	for len(q) > 0 {
		curr := q[0]
		q = q[1:]

		for _, dir := range aoc.Directions4() {
			nx := curr.AddDir(dir)
			if nx == target {
				return true
			}
			if nx.InRange0(d.xLen, d.yLen) && !corruption.Contains(nx) {
				if !seen.Contains(nx) {
					seen.Add(nx)
					q = append(q, nx)
				}
			}
		}
	}

	return false

}
