package days

import (
	aoc "aoc2024/aocutils"
	"strings"
)

type day19 struct {
	day int
}

func Day19() {
	d := &day19{19}
	d.Run()
}

func (d *day19) Run() {
	input, err := aoc.GetDayInputLines(d.day)
	aoc.CheckError(err)

	towels := strings.Split(input[0], ", ")
	patterns := input[2:]

	aoc.DayHeader(d.day)
	part1, part2 := d.findDesigns(towels, patterns)
	aoc.PrintResult(1, part1)
	aoc.PrintResult(2, part2)
}

// Combine part1 and part2. P1 only requires the number of patterns that are
// achievable and P2 needs to know how many ways we can make each one.
func (d *day19) findDesigns(towels []string, patterns []string) (int, int64) {
	cache := make(map[string]int64)
	count := 0
	var perms int64 = 0
	for _, pattern := range patterns {
		dp := d.findDesignPerms(towels, pattern, cache)
		if dp > 0 {
			count++
		}
		perms += dp
	}
	return count, perms
}

// This cries out for recursion and memoization (a cache)
func (d *day19) findDesignPerms(towels []string, pattern string, cache map[string]int64) int64 {
	if pattern == "" {
		return 1
	}
	if v, ok := cache[pattern]; ok {
		return v
	}

	var result int64 = 0
	for _, towel := range towels {
		if strings.HasPrefix(pattern, towel) {
			result += d.findDesignPerms(towels, pattern[len(towel):], cache)
		}
	}
	cache[pattern] = result
	return result
}
