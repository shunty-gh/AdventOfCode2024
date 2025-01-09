package days

import (
	aoc "aoc2024/aocutils"
)

type robot struct {
	px, py, vx, vy int
}

type day14 struct {
	day, xLen, yLen int
}

func Day14() {
	d := &day14{14, 101, 103}
	d.Run()
}

func (d *day14) Run() {
	input, err := aoc.GetDayInputLines(d.day)
	aoc.CheckError(err)

	robots := make([]robot, 0, len(input))
	for _, ln := range input {
		nums, err := aoc.FindAllInts[int](ln)
		aoc.CheckError(err)
		robots = append(robots, robot{nums[0], nums[1], nums[2], nums[3]})
	}
	aoc.DayHeader(d.day)
	aoc.PrintResult(1, d.part1(robots))
	aoc.PrintResult(2, d.part2(robots))
}

func (d *day14) part1(robots []robot) int {
	state := d.moveRobots(robots, 100)

	quads := [4]int{0, 0, 0, 0}
	for k, v := range state {
		q := d.getQuadrant(k)
		if q > 0 {
			quads[q-1] += v
		}
	}
	return quads[0] * quads[1] * quads[2] * quads[3]
}

func (d *day14) moveRobots(robots []robot, moves int) map[aoc.Point2d]int {
	state := make(map[aoc.Point2d]int)
	for _, r := range robots {
		px := (((r.px + moves*r.vx) % d.xLen) + d.xLen) % d.xLen
		py := (((r.py + moves*r.vy) % d.yLen) + d.yLen) % d.yLen
		pt := aoc.Point2d{X: px, Y: py}
		state[pt] = state[pt] + 1
	}
	return state
}

func (d *day14) getQuadrant(p aoc.Point2d) int {
	xl := (d.xLen - 1) / 2
	yl := (d.yLen - 1) / 2
	switch {
	case p.X < xl && p.Y < yl:
		return 1
	case p.X > xl && p.Y < yl:
		return 2
	case p.X < xl && p.Y > yl:
		return 3
	case p.X > xl && p.Y > yl:
		return 4
	default:
		return 0
	}
}

func (d *day14) part2(robots []robot) int {
	// Look for a row containing, let's say, at least 10 consecutive robots
	const searchLen = 10
	// Assume it's in the upper range of possibilities so start high and work our way down
	for i := d.xLen*d.yLen - 1; i > 0; i-- {
		state := d.moveRobots(robots, i)

		for y := 0; y < d.yLen; y++ {
			cont := 0
			// Assume the tree will be centred horizontally (we know this to be true...)
			for x := d.xLen/2 - searchLen; x < d.xLen/2+searchLen; x++ {
				pt := aoc.Point2d{X: x, Y: y}
				if state[pt] > 0 {
					cont++
				} else {
					cont = 0
				}
				if cont >= searchLen {
					return i
				}
			}
		}
	}
	return -1
}
