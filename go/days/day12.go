package days

// https://adventofcode.com/2024/day/12 - Garden Groups

import (
	aoc "aoc2024/aocutils"
	"os"
	"slices"
	"strings"
)

type day12 struct {
	day int
}

func Day12() {
	d := &day12{12}
	d.Run()
}

func (d *day12) Run() {
	const testInput = `RRRRIICCFF
RRRRIICCCF
VVRRRCCFFF
VVRCCCJFFF
VVVVCJJCFE
VVIVCCJJEE
VVIIICJJEE
MIIIIIJJEE
MIIISIJEEE
MMMISSJEEE`

	var input []string
	var err error
	if slices.Contains(os.Args, "-t") {
		input = strings.Split(strings.TrimSpace(testInput), "\n")
	} else {
		input, err = aoc.GetDayInputLines(d.day)
		aoc.CheckError(err)
	}

	regions, pointIndex := buildRegions(input)

	aoc.DayHeader(d.day)
	aoc.PrintResult(1, d.part1(regions))
	aoc.PrintResult(2, d.part2(regions, pointIndex))
}

type plot struct {
	loc    aoc.Point2d
	bounds int
}

func (d *day12) part1(regions map[int][]plot) int {
	result := 0
	for _, plots := range regions {
		area := len(plots)
		perim := 0
		for _, p := range plots {
			perim += p.bounds
		}
		result += perim * area
	}
	return result
}

func (d *day12) part2(regions map[int][]plot, pointIndex map[aoc.Point2d]int) int {
	// The number of sides can be determined by the total number of corners in the region.
	// A corner is either an internal or an external corner.

	result := 0
	for rid, plots := range regions {
		area := len(plots)
		corners := 0
		for _, p := range plots {
			// A plot with 4 external boundaries has 4 external corners
			// A plot with 3 external boundaries must have exactly 2 external corners
			if p.bounds == 4 {
				corners += 4
			} else if p.bounds == 3 {
				corners += 2
			} else {
				// Determine if it is an internal or external corner or neither
				// An external corner must have two adjacent external boundaries - ie if north or
				// south side is not in the region then either east or west must also not be in
				// the region. But not both.
				pt := p.loc
				north := pt.Add(0, -1)
				south := pt.Add(0, 1)
				west := pt.Add(-1, 0)
				east := pt.Add(1, 0)
				onlynorthorsouth := (pointIndex[north] == rid) != (pointIndex[south] == rid)
				onlyeastorwest := (pointIndex[west] == rid) != (pointIndex[east] == rid)
				if onlynorthorsouth && onlyeastorwest {
					corners++
				}

				// An internal corner must have two adjacent plots that are in the
				// same region and a diagonal plot that is not in the region
				for _, dir := range aoc.Diagonals() {
					diag := pt.Add(dir.X, dir.Y)
					// diag must not be in the region...
					if r, ok := pointIndex[diag]; ok && r != rid {
						// and the adjacent plots must both be in the region
						p1 := pt.Add(dir.X, 0)
						p2 := pt.Add(0, dir.Y)
						if r1, ok1 := pointIndex[p1]; ok1 && r1 == rid {
							if r2, ok2 := pointIndex[p2]; ok2 && r2 == rid {
								corners++
							}
						}
					}
				}
			}
		}
		result += area * corners
	}
	return result
}

func buildRegions(input []string) (map[int][]plot, map[aoc.Point2d]int) {
	xlen := len(input[0])
	ylen := len(input)
	// Important that we don't use 0 for the first regionId otherwise the
	// corner detection fails in part 2 as the default value if a point does
	// not exist in the map is also 0 - therefore we can't correctly
	// differentiate between a point that is in region 0 and a point that
	// is non-existent or out of bounds
	regionId := 1
	seen := make(map[aoc.Point2d]int)
	regionsById := make(map[int][]plot)
	for y := 0; y < ylen; y++ {
		for x := 0; x < xlen; x++ {
			pt := aoc.Point2d{X: x, Y: y}
			// Have we already handled this plot
			if _, ok := seen[pt]; ok {
				continue
			}

			// New region
			plots := make([]plot, 0)
			q := make([]aoc.Point2d, 0)
			c := input[y][x]
			q = append(q, pt)
			for len(q) > 0 {
				curr := q[0]
				q = q[1:]

				if _, ok := seen[curr]; ok {
					continue
				}

				bounds := 4 // Start with the max possible number of external boundaries
				for _, dir := range aoc.Directions4() {
					nx := curr.Add(dir.X, dir.Y)
					if nx.InRange0(xlen, ylen) && input[nx.Y][nx.X] == c {
						bounds--
						if _, ok := seen[nx]; !ok {
							q = append(q, nx)
						}
					}
				}
				// Add the current point, now that we know how many external boundaries it has
				seen[curr] = regionId
				plots = append(plots, plot{curr, bounds})
			}
			regionsById[regionId] = append(regionsById[regionId], plots...)
			regionId++
		}
	}

	return regionsById, seen
}
