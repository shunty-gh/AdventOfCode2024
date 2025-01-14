package days

// https://adventofcode.com/2024/day/22 - Monkey Market

import (
	aoc "aoc2024/aocutils"
	"strconv"
	"strings"
)

type day22 struct {
	day int
}

func Day22() {
	d := &day22{22}
	d.Run()
}

func (d *day22) Run() {
	input, err := aoc.GetDayInputLines(d.day)
	aoc.CheckError(err)

	// Create a counter that stores, for each sequence of 4 differences, the
	// accumulated price for each set of secret numbers.
	// For each set of secrets only record the first occurrence of the sequence.
	seqcounter := aoc.NewCounter[[4]int]()
	var part1 int64 = 0
	for _, ln := range input {
		num, _ := strconv.ParseInt(strings.TrimSpace(ln), 10, 64)
		part1 += d.getSequences(num, seqcounter)
	}

	aoc.DayHeader(d.day)
	aoc.PrintResult(1, part1)
	aoc.PrintResult(2, seqcounter.MaxValue())
}

func (d *day22) nextSecret(n int64) int64 {
	r1 := n << 6
	r1 ^= n
	r1 %= 16777216

	r2 := r1 >> 5
	r2 ^= r1
	r2 %= 16777216

	result := r2 << 11
	result ^= r2
	result %= 16777216

	return result
}

func (d *day22) getSequences(num int64, seqcounter *aoc.Counter[[4]int]) int64 {
	diffs := make([]int, 0, 2000)
	seen := aoc.NewSet[[4]int]()
	prev := 0
	ns := num
	for i := 1; i <= 2000; i++ {
		ns = d.nextSecret(ns)

		// For part 2
		price := int(ns % 10)
		diffs = append(diffs, price-prev)
		prev = price

		if i > 3 {
			// Get a sequence of the last 4 (including the current one) diffs
			pd := [4]int(diffs[i-4:])
			// Only add the first occurrence of this sequence. The monkey will only
			// look for the first and then move on.
			if !seen.Contains(pd) {
				seen.Add(pd)
				seqcounter.Inc(pd, price)
			}
		}
	}
	// Return the final (2000th) secret number, for part 1
	return ns
}
