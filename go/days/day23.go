package days

// https://adventofcode.com/2024/day/23 - LAN Party

import (
	aoc "aoc2024/aocutils"
	"slices"
	"strings"
)

type day23 struct {
	day int
}

func Day23() {
	d := &day23{23}
	d.Run()
}

func (d *day23) Run() {
	input, err := aoc.GetDayInputLines(d.day)
	aoc.CheckError(err)

	lan := make(map[string][]string)
	for _, ln := range input {
		sp := strings.Split(strings.TrimSpace(ln), "-")
		lan[sp[0]] = append(lan[sp[0]], sp[1])
		lan[sp[1]] = append(lan[sp[1]], sp[0])
	}
	for _, v := range lan {
		slices.Sort(v)
	}

	p1 := aoc.NewSet[[3]string]()
	p2 := aoc.NewCounter[string]()
	for k1, v := range lan {
		for _, k2 := range v {
			common := aoc.Intersect(v, lan[k2])
			if len(common) > 0 {

				// Part 1
				for _, k3 := range common {
					if strings.HasPrefix(k1, "t") || strings.HasPrefix(k2, "t") || strings.HasPrefix(k3, "t") {
						connected := []string{k1, k2, k3}
						slices.Sort(connected)
						p1.Add([3]string(connected))
					}
				}

				// Part 2
				if len(common) > 1 {
					party := []string{k1, k2}
					party = append(party, common...)
					slices.Sort(party)
					partykey := strings.Join(party, ",")
					p2.Add(partykey)
				}
			}
		}
	}

	aoc.DayHeader(d.day)
	aoc.PrintResult(1, p1.Count())
	aoc.PrintResult(2, p2.MaxKey())
}
