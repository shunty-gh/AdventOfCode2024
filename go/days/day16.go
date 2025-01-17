package days

// https://adventofcode.com/2024/day/16 - Reindeer Maze

import (
	aoc "aoc2024/aocutils"
	"container/heap"
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
	part1, part2 := d.bestPaths(grid, start, end)
	aoc.PrintResult(1, part1)
	aoc.PrintResult(2, part2)
}

type d16qItem struct {
	loc  aoc.Point2d
	dir  aoc.Direction
	cost int
}

// Get cheapest paths from start and the best total score from start to end
func (d *day16) bestPaths(grid *aoc.Grid2d[byte], start aoc.Point2d, target aoc.Point2d) (int, int) {
	// Use Dijkstra's to get the best score for each point from the start
	// When we find what we consider to be the best/better cost for a point, record
	// how we got to that point in the `comeFrom` map - ie when we find a valid `next` point that we
	// are about to put in the queue, record the `curr` value in comeFrom

	dist := aoc.NewCounter[aoc.PointDirection]()
	dist.Set(aoc.PointDirection{Loc: start, Dir: aoc.DirectionE}, 0)
	comeFrom := make(map[aoc.PointDirection][]aoc.PointDirection)
	best := 0

	pq := make(PriorityQueue, 1)
	pq[0] = &d16qItem{start, aoc.DirectionE, 0}
	heap.Init(&pq)
	for pq.Len() > 0 {
		curr := heap.Pop(&pq).(*d16qItem)

		if curr.loc == target && (best == 0 || curr.cost < best) {
			best = curr.cost
		}

		for _, dir := range []aoc.Direction{curr.dir, curr.dir.TurnLeft(), curr.dir.TurnRight()} {
			nx := curr.loc.AddDir(dir)
			if c, ok := grid.TryGet(nx); ok && c != d.wall {
				nxpd := aoc.PointDirection{Loc: nx, Dir: dir}
				newcost := curr.cost + 1
				if dir != curr.dir {
					newcost += 1000
				}
				if nxcost, ok := dist.TryGet(nxpd); !ok || newcost < nxcost {
					dist.Set(nxpd, newcost)
					heap.Push(&pq, &d16qItem{nx, dir, newcost})
					// If it is better than the current best cost than clear out any
					// existing comeFrom values for this point+dir and start a new list
					comeFrom[nxpd] = []aoc.PointDirection{{Loc: curr.loc, Dir: curr.dir}}
				} else if nxcost == newcost {
					// If it is equal to the best cost for this point+dir then add it to
					// the list of places to have come from
					comeFrom[nxpd] = append(comeFrom[nxpd], aoc.PointDirection{Loc: curr.loc, Dir: curr.dir})
				}
			}
		}
	}

	// Every entry in the comeFrom list will now take us via the cheapest path from
	// the given point+dir back to the start.
	// So now work our way back through the `comeFrom` list from the target/end
	// point (via all possible directions) and create a set of all points appearing
	// in any of those best paths.
	q := []aoc.PointDirection{
		{Loc: target, Dir: aoc.DirectionN},
		{Loc: target, Dir: aoc.DirectionE},
		{Loc: target, Dir: aoc.DirectionS},
		{Loc: target, Dir: aoc.DirectionW},
	}
	bestpoints := aoc.NewSet[aoc.PointDirection]()
	bestset := aoc.NewSet[aoc.Point2d]()
	bestset.Add(target)
	for len(q) > 0 {
		curr := q[0]
		q = q[1:]
		for _, from := range comeFrom[curr] {
			if !bestpoints.Contains(from) {
				bestpoints.Add(from)
				bestset.Add(from.Loc)
				q = append(q, from)
			}
		}
	}
	return best, bestset.Count()
}

// A simple priority queue using a heap, as described on the Go docs
// at https://pkg.go.dev/container/heap#example-package-PriorityQueue
type PriorityQueue []*d16qItem

// Len, Less and Swap from the sort.Interface
func (pq PriorityQueue) Len() int { return len(pq) }

func (pq PriorityQueue) Less(i, j int) bool {
	return pq[i].cost < pq[j].cost
}

func (pq PriorityQueue) Swap(i, j int) {
	pq[i], pq[j] = pq[j], pq[i]
}

// Push and Pop from the heap interface
func (pq *PriorityQueue) Push(x any) {
	item := x.(*d16qItem)
	*pq = append(*pq, item)
}

func (pq *PriorityQueue) Pop() any {
	old := *pq
	n := len(old)
	item := old[n-1]
	old[n-1] = nil // don't stop the GC from reclaiming the item eventually
	*pq = old[0 : n-1]
	return item
}
