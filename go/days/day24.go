package days

// https://adventofcode.com/2024/day/24 - Crossed Wires

import (
	aoc "aoc2024/aocutils"
	"maps"
	"slices"
	"strings"
)

type day24 struct {
	day int
}

type day24Gate struct {
	inL, op, inR string
}

func Day24() {
	d := &day24{24}
	d.Run()
}

func (d *day24) Run() {
	input, err := aoc.GetDayInputString(d.day)
	aoc.CheckError(err)

	sp := strings.Split(strings.TrimSpace(input), "\n\n")
	sp0 := strings.Split(strings.TrimSpace(sp[0]), "\n")
	sp1 := strings.Split(strings.TrimSpace(sp[1]), "\n")
	// Initial values
	inits := make(map[string]bool, len(sp0))
	for _, ln := range sp0 {
		inits[ln[0:3]] = ln[5:] == "1"
	}
	// Gate connections
	gates := make(map[string]day24Gate)
	for _, ln := range sp1 {
		gg := strings.Split(strings.TrimSpace(ln), " ")
		gate := day24Gate{gg[0], gg[1], gg[2]}
		gates[gg[4]] = gate
	}

	aoc.DayHeader(d.day)
	aoc.PrintResult(1, d.calculateZ(gates, inits))
	aoc.PrintResult(2, 0)
}

func (d *day24) calculateZ(gates map[string]day24Gate, startValues map[string]bool) int64 {
	wires := make(map[string]bool)
	for k, v := range startValues {
		wires[k] = v
	}
	// Find all the z wires and work backwards to solve them
	zvalues := make(map[string]bool)
	for k := range gates {
		if k[0] == 'z' {
			wz := d.solveWire(k, gates, wires)
			zvalues[k] = wz
		}
	}
	return d.bitsToLong(zvalues)
}

func (d *day24) solveWire(wire string, gates map[string]day24Gate, wires map[string]bool) bool {
	if v, ok := wires[wire]; ok {
		return v
	}

	gate := gates[wire]
	var gl, gr, ok, result bool
	if gl, ok = wires[gate.inL]; !ok {
		gl = d.solveWire(gate.inL, gates, wires)
	}
	if gr, ok = wires[gate.inR]; !ok {
		gr = d.solveWire(gate.inR, gates, wires)
	}

	switch gate.op {
	case "AND":
		result = gl && gr
	case "OR":
		result = gl || gr
	case "XOR":
		result = gl != gr
	default:
		panic("Invalid operation")
	}
	wires[wire] = result
	return result
}

func (d *day24) bitsToLong(bits map[string]bool) int64 {
	var result int64 = 0
	zv := slices.Sorted(maps.Keys(bits))
	for i := 0; i < len(zv); i++ {
		if bits[zv[i]] {
			result += 1 << i
		}
	}
	return result
}
