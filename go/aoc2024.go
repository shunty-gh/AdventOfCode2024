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
	daymap[6] = days.Day06
	daymap[7] = days.Day07
	daymap[8] = days.Day08
	daymap[9] = days.Day09
	daymap[10] = days.Day10
	daymap[11] = days.Day11
	daymap[12] = days.Day12
	daymap[13] = days.Day13
	daymap[14] = days.Day14
	daymap[15] = days.Day15
	daymap[16] = days.Day16
	daymap[17] = days.Day17
	daymap[18] = days.Day18
	daymap[19] = days.Day19
	daymap[20] = days.Day20
	daymap[21] = days.Day21
	daymap[22] = days.Day22
	daymap[23] = days.Day23
	daymap[24] = days.Day24
	daymap[25] = days.Day25

	// Check command line args for which daysToRun to evaluate
	// If none specified then run all daysToRun
	daysToRun := make([]int, 0)
	for _, arg := range os.Args {
		if b, n := isValidAoCDayNumber(arg); b {
			daysToRun = append(daysToRun, n)
		}
	}
	if len(daysToRun) == 0 {
		daysToRun = append(daysToRun, slices.Collect(maps.Keys(daymap))...)
	}
	sort.Ints(daysToRun)

	fmt.Println()
	fmt.Println("Advent Of Code 2024 in Go")
	fmt.Println()

	for _, day := range daysToRun {
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
