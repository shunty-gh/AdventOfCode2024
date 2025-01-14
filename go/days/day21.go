package days

// https://adventofcode.com/2024/day/21 - Keypad Conundrum

import (
	aoc "aoc2024/aocutils"
	"strconv"
	"strings"
)

type d21CacheKey struct {
	path  string
	level int
}

type day21 struct {
	day       int
	buttonMap map[string][]string
	cache     map[d21CacheKey]int64
}

func Day21() {
	d := &day21{21, nil, make(map[d21CacheKey]int64)}
	d.Run()
}

func (d *day21) Run() {
	input, err := aoc.GetDayInputLines(d.day)
	aoc.CheckError(err)

	buttonMap := d.getButtonMap()
	d.buttonMap = buttonMap
	var part1, part2 int64 = 0, 0
	for _, line := range input {
		linenum, _ := strconv.ParseInt(line[:len(line)-1], 10, 64)
		part1 += d.pathLength(line, 3) * linenum
		part2 += d.pathLength(line, 26) * linenum
	}
	aoc.DayHeader(d.day)
	aoc.PrintResult(1, part1)
	aoc.PrintResult(2, part2)
}

func (d *day21) pathLength(path string, level int) int64 {
	if level == 0 {
		return int64(len(path))
	}
	key := d21CacheKey{path, level}
	if v, ok := d.cache[key]; ok {
		return v
	}
	var result int64 = 0
	apath := "A" + path
	for i := range len(path) {
		frag := apath[i : i+2]
		paths := d.buttonMap[frag]
		if len(paths) == 1 {
			result += d.pathLength(paths[0], level-1)
		} else {
			result += min(d.pathLength(paths[0], level-1), d.pathLength(paths[1], level-1))
		}
	}

	d.cache[key] = result
	return result
}

func (d *day21) getButtonMap() map[string][]string {
	result := map[string][]string{
		"AA": {"A"}, "A^": {"<A"}, "A<": {"v<<A"}, "Av": {"<vA", "v<A"}, "A>": {"vA"},
		"^A": {">A"}, "^^": {"A"}, "^<": {"v<A"}, "^v": {"vA"}, "^>": {"v>A", ">vA"},
		"<A": {">>^A"}, "<^": {">^A"}, "<<": {"A"}, "<v": {">A"}, "<>": {">>A"},
		"vA": {"^>A", ">^A"}, "v^": {"^A"}, "v<": {"<A"}, "vv": {"A"}, "v>": {">A"},
		">A": {"^A"}, ">^": {"<^A", "^<A"}, "><": {"<<A"}, ">v": {"<A"}, ">>": {"A"},
	}

	numpad := map[string]aoc.Point2d{
		"7": aoc.NewPoint2d(0, 0), "8": aoc.NewPoint2d(1, 0), "9": aoc.NewPoint2d(2, 0),
		"4": aoc.NewPoint2d(0, 1), "5": aoc.NewPoint2d(1, 1), "6": aoc.NewPoint2d(2, 1),
		"1": aoc.NewPoint2d(0, 2), "2": aoc.NewPoint2d(1, 2), "3": aoc.NewPoint2d(2, 2),
		" ": aoc.NewPoint2d(0, 3), "0": aoc.NewPoint2d(1, 3), "A": aoc.NewPoint2d(2, 3),
	}

	for _, b0 := range []string{"A", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"} {
		for _, b1 := range []string{"A", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"} {
			key := b0 + b1
			if b0 == b1 {
				result[key] = []string{"A"}
				continue
			}
			xy0 := numpad[b0]
			xy1 := numpad[b1]
			dx, dy := xy1.X-xy0.X, xy1.Y-xy0.Y
			result[key] = []string{}

			// Left/right first
			if dx > 0 || !(xy0.Y == 3 && (xy0.X == 0 || xy1.X == 0)) {
				path := ""
				if dx < 0 {
					path += strings.Repeat("<", -dx)
				} else {
					path += strings.Repeat(">", dx)
				}
				if dy < 0 {
					path += strings.Repeat("^", -dy)
				} else {
					path += strings.Repeat("v", dy)
				}
				path += "A"
				result[key] = append(result[key], path)
			}

			// Up/down first
			if !(dx == 0 || dy == 0) { // Only if path is not a single straight line
				if dy < 0 || !(xy1.Y == 3 && (xy0.X == 0 || xy1.X == 0)) {
					path := ""
					if dy < 0 {
						path += strings.Repeat("^", -dy)
					} else {
						path += strings.Repeat("v", dy)
					}
					if dx < 0 {
						path += strings.Repeat("<", -dx)
					} else {
						path += strings.Repeat(">", dx)
					}
					path += "A"
					result[key] = append(result[key], path)
				}
			}
		}
	}
	return result
}
