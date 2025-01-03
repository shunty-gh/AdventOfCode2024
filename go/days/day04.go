package days

import (
	aoc "aoc2024/aocutils"
)

func Day04() {
	const DAY = 4
	lines, err := aoc.GetDayInputLines(DAY)
	aoc.CheckError(err)

	part1, part2 := 0, 0
	xlen := len(lines[0])
	ylen := len(lines)
	for y := 0; y < ylen; y++ {
		for x := 0; x < xlen; x++ {
			c := lines[y][x]
			if c == 'X' {
				part1 += findMAS(x, y, lines)
			} else if c == 'A' && findX_MAS(x, y, lines) {
				part2++
			}
		}
	}
	aoc.DayHeader(DAY)
	aoc.PrintResult(1, part1)
	aoc.PrintResult(2, part2)
}

func findMAS(x, y int, grid []string) int {
	xlen := len(grid[0])
	ylen := len(grid)

	result := 0
	for _, d := range aoc.Directions8() {
		dx, dy := d.X, d.Y
		mx, my := x+dx, y+dy
		if aoc.InRange0(mx, my, xlen, ylen) && grid[my][mx] == 'M' {
			ax, ay := mx+dx, my+dy
			if aoc.InRange0(ax, ay, xlen, ylen) && grid[ay][ax] == 'A' {
				sx, sy := ax+dx, ay+dy
				if aoc.InRange0(sx, sy, xlen, ylen) && grid[sy][sx] == 'S' {
					result++
				}
			}
		}
	}
	return result
}

func findX_MAS(x, y int, grid []string) bool {
	xlen := len(grid[0])
	ylen := len(grid)

	tlx, trx, blx, brx := x-1, x+1, x-1, x+1
	tly, try, bly, bry := y-1, y-1, y+1, y+1
	if aoc.InRange0(tlx, tly, xlen, ylen) &&
		aoc.InRange0(trx, try, xlen, ylen) &&
		aoc.InRange0(blx, bly, xlen, ylen) &&
		aoc.InRange0(brx, bry, xlen, ylen) {

		return ((grid[tly][tlx] == 'M' && grid[bry][brx] == 'S') || (grid[tly][tlx] == 'S' && grid[bry][brx] == 'M')) &&
			((grid[try][trx] == 'M' && grid[bly][blx] == 'S') || (grid[try][trx] == 'S' && grid[bly][blx] == 'M'))
	}
	return false
}
