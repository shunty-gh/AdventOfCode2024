package main

import (
	"aoc2024/days"
	"fmt"
	"maps"
	"os"
	"slices"
	"sort"
	"strconv"
)

func main() {
	daymap := make(map[int]func())
	daymap[1] = days.Day01
	daymap[2] = days.Day02
	daymap[3] = days.Day03
	daymap[4] = days.Day04
	daymap[5] = days.Day05

	// Check command line args for which days to evaluate
	// If none specified then run all days
	days := make([]int, 0)
	for _, arg := range os.Args {
		if b, n := isValidAoCDayNumber(arg); b {
			days = append(days, n)
		}
	}
	if len(days) == 0 {
		days = append(days, slices.Collect(maps.Keys(daymap))...)
	}
	sort.Ints(days)

	fmt.Println()
	fmt.Println("Advent Of Code 2024 in Go")
	fmt.Println()

	for _, day := range days {
		daymap[day]()
		fmt.Println()
	}
}

func isValidAoCDayNumber(s string) (bool, int) {
	n, err := strconv.ParseInt(s, 10, 64)
	if err != nil {
		return false, 0
	} else if n >= 1 && n <= 25 {
		return true, int(n)
	}
	return false, 0
}
