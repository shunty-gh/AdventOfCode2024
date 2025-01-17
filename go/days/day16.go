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
	part1, p1 := d.costsFromStart(grid, start, end)
	aoc.PrintResult(1, part1)
	p2 := d.costsFromEnd(grid, end)

	// For each possible point, if the cost from the start to the point
	// plus the cost from the point to the end, equals our target (shortest) cost
	// then the point must be on one of the best paths
	bestset := aoc.NewSet[aoc.Point2d]()
	for y := range d.yLen {
		for x := range d.xLen {
			for _, dir := range aoc.Directions4() {
				pd := aoc.PointDirection{Loc: aoc.Point2d{X: x, Y: y}, Dir: dir}
				if d1, ok := p1.TryGet(pd); ok {
					if d2, ok := p2.TryGet(pd); ok {
						if d1+d2 == part1 {
							bestset.Add(pd.Loc)
						}
					}
				}
			}
		}
	}
	aoc.PrintResult(2, bestset.Count())
}

type d16qItem struct {
	loc  aoc.Point2d
	dir  aoc.Direction
	cost int
}

// Get cheapest paths from start and the best total score from start to end
func (d *day16) costsFromStart(grid *aoc.Grid2d[byte], start aoc.Point2d, target aoc.Point2d) (int, *aoc.Counter[aoc.PointDirection]) {
	// Get the best score for each point from the start
	dist := aoc.NewCounter[aoc.PointDirection]()
	dist.Set(aoc.PointDirection{Loc: start, Dir: aoc.DirectionE}, 0)
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
				}
			}
		}
	}
	// Technically we don't need to store/return the best score as we can find it from
	// the dist counter as the minimum of the NESW entries at the target point.
	// But it makes life a bit neater.
	return best, dist
}

// Get cheapest paths from end
func (d *day16) costsFromEnd(grid *aoc.Grid2d[byte], target aoc.Point2d) *aoc.Counter[aoc.PointDirection] {
	// Similar search but this time we're going in reverse from the end point
	// in order to build a list of scores from each point to the end.
	// We have to allow for the fact that each point can be reached by any neighbour from *any direction*
	dist := aoc.NewCounter[aoc.PointDirection]()

	pq := make(PriorityQueue, 4)
	pq[0] = &d16qItem{target, aoc.DirectionN, 0}
	pq[1] = &d16qItem{target, aoc.DirectionE, 0}
	pq[2] = &d16qItem{target, aoc.DirectionS, 0}
	pq[3] = &d16qItem{target, aoc.DirectionW, 0}
	heap.Init(&pq)
	for pq.Len() > 0 {
		curr := heap.Pop(&pq).(*d16qItem)

		cpd := aoc.PointDirection{Loc: curr.loc, Dir: curr.dir}
		if d, ok := dist.TryGet(cpd); ok && curr.cost >= d {
			continue
		}
		dist.Set(cpd, curr.cost)

		// NB: Reverse direction as we're getting shortest paths *from the end*
		nx := curr.loc.AddDir(curr.dir.Reverse())
		if c, ok := grid.TryGet(nx); ok && c != d.wall {
			heap.Push(&pq, &d16qItem{nx, curr.dir, curr.cost + 1})
		}
		// But left and right turns are still the same
		heap.Push(&pq, &d16qItem{curr.loc, curr.dir.TurnLeft(), curr.cost + 1000})
		heap.Push(&pq, &d16qItem{curr.loc, curr.dir.TurnRight(), curr.cost + 1000})
	}
	return dist
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
