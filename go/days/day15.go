package days

import (
	aoc "aoc2024/aocutils"
	"fmt"
	"os"
	"slices"
	"strings"
)

type day15 struct {
	day              int
	start, box, wall byte
	boxL, boxR       byte
	isTest           bool
}

func Day15() {
	d := &day15{15, '@', 'O', '#', '[', ']', false}
	d.run()
}

func (d *day15) run() {
	const testInput = `##########
#..O..O.O#
#......O.#
#.OO..O.O#
#..O@..O.#
#O#..O...#
#O..O..O.#
#.OO.O.OO#
#....O...#
##########

<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^`

	var input string
	var err error

	if slices.Contains(os.Args, "-t") {
		input = strings.TrimSpace(testInput)
		d.isTest = true
	} else {
		input, err = aoc.GetDayInputString(d.day)
		aoc.CheckError(err)
	}

	blocks := strings.Split(input, "\n\n")
	grid := aoc.NewGridFromLinesIgnore[byte](strings.Split(blocks[0], "\n"), []byte{'.'})
	instructions := strings.Replace(blocks[1], "\n", "", -1)

	aoc.DayHeader(d.day)
	aoc.PrintResult(1, d.part1(grid, instructions))
	aoc.PrintResult(2, d.part2(strings.Split(blocks[0], "\n"), instructions))
}

func (d *day15) part1(grid *aoc.Grid2d[byte], instructions string) int {
	curr, err := grid.Find(d.start)
	aoc.CheckError(err)

	for i := 0; i < len(instructions); i++ {
		dir := aoc.CharToDirection(instructions[i])
		nx := curr.AddDir(dir)
		c := grid.CharAt(nx)
		if c == d.wall {
			continue
		}
		if c == d.box {
			// Can we push the box(es) - look along the same direction for a gap
			next := nx.AddDir(dir)
			cnext := grid.CharAt(next)
			for cnext == d.box {
				next = next.AddDir(dir)
				cnext = grid.CharAt(next)
			}
			// Have we arrived at a space or a wall
			if grid.CharAt(next) == d.wall {
				continue
			}
			// 'Move' the element at nx into the new space
			grid.Move(nx, next)
			curr = nx
		} else {
			curr = nx
		}
	}

	return d.gpsTotal(grid)
}

func (d *day15) gpsTotal(grid *aoc.Grid2d[byte]) int {
	// Sum co-ords of all boxes
	sums := aoc.Apply(grid, func(k aoc.Point2d, v byte) int {
		if v == d.box || v == d.boxL {
			return 100*k.Y + k.X
		}
		return 0
	})
	result := 0
	for _, s := range sums {
		result += s
	}
	return result
}

func (d *day15) part2(input []string, instructions string) int {
	grid := d.buildPart2Grid(input)
	curr, err := grid.Find(d.start)
	aoc.CheckError(err)

	for i := 0; i < len(instructions); i++ {
		grid.Set(curr, ' ')
		dir := aoc.CharToDirection(instructions[i])
		nx := curr.AddDir(dir)
		c := grid.CharAt(nx)
		if c == d.wall {
			continue
		}
		if c == d.boxL || c == d.boxR {
			// Are we going horizontal
			if dir.Y == 0 {
				next := nx.AddDir(dir)
				cnext := grid.CharAt(next)
				for cnext == d.boxL || cnext == d.boxR {
					next = next.AddDir(dir)
					cnext = grid.CharAt(next)
				}
				// Have we arrived at a space or a wall
				if cnext == d.wall {
					continue
				}
				// Shuffle everything along
				revdir := aoc.Direction{X: -dir.X, Y: 0}
				for next.X != nx.X {
					nrev := next.AddDir(revdir)
					grid.Set(next, grid.CharAt(nrev))
					next = nrev
				}
				grid.Set(nx, ' ')
				curr = nx
			} else {
				// We must be pushing boxes up or down
				pushrows := make([][]aoc.Point2d, 0)
				needtopush := make([]aoc.Point2d, 0)
				needtopush = append(needtopush, curr)

				fail := false
				for !fail && len(needtopush) > 0 {
					pushnext := make([]aoc.Point2d, 0)
					// Can we push each element in needtopush. We can, potentially, push if
					// no point that we need to push into is a wall
					for _, p := range needtopush {
						pnx := p.AddDir(dir)
						if slices.Contains(pushnext, pnx) { // Make sure we haven't already added the point
							continue
						}
						cp := grid.CharAt(pnx)
						if cp == d.wall {
							fail = true
							break
						}
						if cp == d.boxL {
							pushnext = append(pushnext, pnx)
							pushnext = append(pushnext, pnx.AddDir(aoc.DirectionRight))
						} else if cp == d.boxR {
							pushnext = append(pushnext, pnx.AddDir(aoc.DirectionLeft))
							pushnext = append(pushnext, pnx)
						}
						// else it's a space - which is good
					}
					if !fail { // so far so good. move on to next row
						pushrows = append(pushrows, needtopush)
						needtopush = pushnext
					}
				}
				if !fail {
					// Move them
					for ri := len(pushrows) - 1; ri >= 0; ri-- {
						row := pushrows[ri]
						for _, pt := range row {
							grid.Set(pt.AddDir(dir), grid.CharAt(pt))
							grid.Set(pt, ' ')
						}
					}
					curr = nx
				}
			}
		} else {
			curr = nx
		}
		grid.Set(curr, d.start)
		if d.isTest {
			d.printGrid(grid)
		}
	}

	return d.gpsTotal(grid)
}

func (d *day15) buildPart2Grid(input []string) *aoc.Grid2d[byte] {
	grid := aoc.NewGrid[byte]()
	for y := 0; y < len(input); y++ {
		for x := 0; x < len(input[0]); x++ {
			pt := aoc.Point2d{X: x * 2, Y: y}
			c := input[y][x]
			switch c {
			case d.wall:
				grid.Set(pt, d.wall)
				grid.Set(pt.AddDir(aoc.DirectionRight), d.wall)
			case d.box:
				grid.Set(pt, d.boxL)
				grid.Set(pt.AddDir(aoc.DirectionRight), d.boxR)
			case d.start:
				grid.Set(pt, d.start)
				grid.Set(pt.AddDir(aoc.DirectionRight), ' ') // Not really needed
			default:
				//
			}
		}
	}
	if d.isTest {
		d.printGrid(grid)
	}
	return grid
}

func (d *day15) printGrid(grid *aoc.Grid2d[byte]) {
	for y := 0; y < 10; y++ {
		s := ""
		for x := 0; x < 20; x++ {
			pt := aoc.Point2d{X: x, Y: y}
			c := grid.CharAt(pt)
			switch c {
			case grid.CharNotFound, ' ', 0:
				s += "."
			default:
				s += string(c)
			}
		}
		fmt.Println(s)
	}
	fmt.Println()
}
