package days

// https://adventofcode.com/2024/day/7 - Bridge Repair

import (
	aoc "aoc2024/aocutils"
	"strconv"
	"strings"
)

func Day07() {
	const DAY = 7
	lines, err := aoc.GetDayInputLines(DAY)
	aoc.CheckError(err)

	part1, part2 := 0, 0
	for _, line := range lines {
		sp := strings.Split(line, ": ")
		tgt, _ := strconv.Atoi(sp[0])
		nums, _ := aoc.StringToInts(sp[1], " ")

		part1 += calibrationResult(tgt, nums, false)
		part2 += calibrationResult(tgt, nums, true)
	}

	aoc.DayHeader(DAY)
	aoc.PrintResult(1, part1)
	aoc.PrintResult(2, part2)
}

type qItem struct {
	acc, idx int
}

func calibrationResult(target int, nums []int, p2 bool) int {
	q := make([]qItem, 0, len(nums)*2048)
	q = append(q, qItem{0, 0})
	for len(q) > 0 {
		curr := q[0]
		q = q[1:]

		nplus := curr.acc + nums[curr.idx]
		nmult := curr.acc * nums[curr.idx]
		nconc := 0
		if p2 {
			nconc = curr.acc*tensMultiplier(nums[curr.idx]) + nums[curr.idx]
		}

		if curr.idx == len(nums)-1 {
			if nplus == target || nmult == target || nconc == target {
				return target
			}
			continue
		}
		if nplus <= target {
			q = append(q, qItem{nplus, curr.idx + 1})
		}
		if nmult <= target {
			q = append(q, qItem{nmult, curr.idx + 1})
		}
		if p2 && nconc <= target {
			q = append(q, qItem{nconc, curr.idx + 1})
		}
	}
	return 0
}

func tensMultiplier(n int) int {
	result := 1
	nn := n
	for nn > 0 {
		nn = nn / 10
		result *= 10
	}
	return result
}
