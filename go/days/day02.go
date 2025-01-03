package days

import (
	aoc "aoc2024/aocutils"
)

func reportIsOk(nums []int) bool {
	isinc := nums[1] > nums[0]
	for i := 1; i < len(nums); i++ {
		diff := nums[i] - nums[i-1]
		if isinc && (diff <= 0 || diff > 3) {
			return false
		}
		if !isinc && (diff >= 0 || diff < -3) {
			return false
		}
	}
	return true
}

func Day02() {
	lines, err := aoc.GetDayInputLines(2)
	if err != nil {
		panic(err)
	}

	var nums []int
	part1, part2 := 0, 0
	for _, line := range lines {
		if line == "" {
			continue
		}
		nums, _ = aoc.StringToInts(line, " ")

		if reportIsOk(nums) {
			part1++
			part2++
		} else {
			// For part 2, test nums array but skip each element in turn
			n2 := make([]int, len(nums)-1)
			for i := range nums {
				copy(n2, nums[:i])
				copy(n2[i:], nums[i+1:])
				// OR...
				// n2 := make([]int, 0, len(nums)-1)
				// n2 = append(n2, nums[:i]...)
				// n2 = append(n2, nums[i+1:]...)
				if reportIsOk(n2) {
					part2++
					break
				}
			}
		}
	}

	aoc.DayHeader(2)
	aoc.PrintResult(1, part1)
	aoc.PrintResult(2, part2)
}
