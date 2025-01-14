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

	nums := make([]int64, 0, len(input))
	for _, ln := range input {
		n, _ := strconv.ParseInt(strings.TrimSpace(ln), 10, 64)
		nums = append(nums, n)
	}
	aoc.DayHeader(d.day)
	aoc.PrintResult(1, d.part1(nums))
	aoc.PrintResult(2, d.part2(nums))
}

func (d *day22) part1(nums []int64) int64 {
	// Calculate 2000 secrets and push the result onto the outch channel
	getSecret2000 := func(start int64, outch chan int64) {
		n := start
		for i := 0; i < 2000; i++ {
			n = d.nextSecret(n)
		}
		outch <- n
	}

	ch := make(chan int64, len(nums)/2)
	resultch := make(chan int64)
	// Read and sum all the results coming from the in channel and, finally, push
	// the result onto the out channel
	go func(in, out chan int64, count int) {
		var result int64 = 0
		for c := 0; c < count; c++ {
			result += <-in
		}
		out <- result
	}(ch, resultch, len(nums))

	for _, num := range nums {
		go getSecret2000(num, ch)
	}
	return <-resultch
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

type d22PriceDiff struct {
	price, diff int
}

func (d *day22) part2(nums []int64) int {
	allseqs := make([]map[[4]int]int, 0, len(nums))
	seqset := aoc.NewSet[[4]int]()
	for _, num := range nums {
		seqs := d.getSequences(num)
		allseqs = append(allseqs, seqs)
		for s := range seqs {
			seqset.Add(s)
		}
	}

	mostbananas := 0
	for _, seq := range seqset.Keys() {
		bananas := 0
		for _, s := range allseqs {
			if b, ok := s[seq]; ok {
				bananas += b
			}
		}
		if bananas > mostbananas {
			mostbananas = bananas
		}
	}
	return mostbananas
}

func (d *day22) getSequences(num int64) map[[4]int]int {
	diffs := make([]d22PriceDiff, 0, 2000)
	seqs := make(map[[4]int]int)
	prev := 0
	ns := num
	for i := 1; i <= 2000; i++ {
		ns = d.nextSecret(ns)
		price := int(ns % 10)
		diff := price - prev
		prev = price
		diffs = append(diffs, d22PriceDiff{price, diff})

		if i > 3 {
			pd := [4]int{diffs[i-4].diff, diffs[i-3].diff, diffs[i-2].diff, diffs[i-1].diff}
			if _, ok := seqs[pd]; !ok { // Only add the first occurrence of this sequence. The monkey will only look for the first and then move on.
				seqs[pd] = price
			}
		}
	}
	return seqs
}
