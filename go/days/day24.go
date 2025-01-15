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
	inL, op, inR, out string
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
		gate := day24Gate{gg[0], gg[1], gg[2], gg[4]}
		gates[gg[4]] = gate
	}

	aoc.DayHeader(d.day)
	aoc.PrintResult(1, d.calculateZ(gates, inits))

	badgates := d.findBadGates(gates)
	part2 := make([]string, 0, len(badgates))
	for _, g := range badgates {
		part2 = append(part2, g.out)
	}
	slices.Sort(part2)
	//aoc.PrintResult(2, badgates)
	aoc.PrintResult(2, strings.Join(part2, ","))
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

// There's no way I worked this out for myself, but it is a whole lot better
// than my original C# version (which did actually work but was a lot more convoluted).
// This came about with so much help from:
// https://www.reddit.com/r/adventofcode/comments/1hla5ql/2024_day_24_part_2_a_guide_on_the_idea_behind_the/
// and https://www.bytesizego.com/blog/aoc-day24-golang
// and https://en.wikipedia.org/wiki/Adder_(electronics)#Ripple-carry_adder
// and https://www.advanced-ict.info/mathematics/full_adder.html
func (d *day24) findBadGates(gates map[string]day24Gate) []day24Gate {
	result := make([]day24Gate, 0)

	// There are 4 rules that determine whether a gate is 'bad' or not
	for k, g := range gates {
		// Any z wire ("sum" or "output"), unless it is the last one, must be the result of an XOR gate
		if k[0] == 'z' && k != "z45" && g.op != "XOR" {
			result = append(result, g)
			continue
		}
		// Any XOR gate must output to z OR must have x or y inputs
		if k[0] != 'z' && g.op == "XOR" && g.inL[0] != 'x' && g.inL[0] != 'y' && g.inR[0] != 'x' && g.inR[0] != 'y' {
			result = append(result, g)
			continue
		}
		// If an XOR gate has both x and y inputs then there must be another XOR gate
		// that takes the output of this gate as one of its inputs. The x and y inputs must not be x00 or y00 (as the
		// second xor gate is meant to combine the ouptut of the first with the previous carry bit - but for x00,y00
		// there is no previous carry).
		if g.op == "XOR" &&
			((g.inL[0] == 'x' && g.inR[0] == 'y') || (g.inL[0] == 'y' && g.inR[0] == 'x')) &&
			g.inL != "x00" && g.inL != "y00" && g.inR != "x00" && g.inR != "y00" {

			found := false
			for _, g2 := range gates {
				if g2.op == "XOR" {
					if g2.inL == g.out || g2.inR == g.out {
						found = true
					}
				}
			}
			if !found {
				result = append(result, g)
				continue
			}
		}
		// If an AND gate has both x and y inputs then there must be another OR gate
		// that takes the output of this gate as one of its inputs. The x and y inputs must not be x00 or y00.
		// Ignore x00,y00 as they may be just a half-adder rather than a full adder
		if g.op == "AND" &&
			((g.inL[0] == 'x' && g.inR[0] == 'y') || (g.inL[0] == 'y' && g.inR[0] == 'x')) &&
			g.inL != "x00" && g.inL != "y00" && g.inR != "x00" && g.inR != "y00" {

			found := false
			for _, g2 := range gates {
				if g2.op == "OR" {
					if g2.inL == g.out || g2.inR == g.out {
						found = true
					}
				}
			}
			if !found {
				result = append(result, g)
				continue
			}
		}
	}
	return result
}

/*
A full adder, the basis for a ripple carry adder - as defined by the input:


X   X               $$$$$$$$$$$
 X X                  $ $$$$$$$$$
  X    $$$$$$$$$$$$$$$$$ $    $$$$
 X X             $     $ $$     $$$
X   X            $     $ $$      $$$
                 $     $ $$       $$$
Y   Y            $     $ $$  XOR  $$$$$$         $$$$$$$$$$
 Y Y             $     $ $$      $$$   $$         $ $$$$$$$$$
  Y    $$$$$$$$$$$$$$$$$ $$     $$$     $$$$$$$$$$$$ $    $$$$
  Y        $     $     $ $    $$$$           $     $ $$     $$$                                SSS
  Y        $     $    $ $$$$$$$$$            $     $ $$      $$$                              S
           $     $   $$$$$$$$$$$             $     $ $$  XOR  $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$  SSS
  CCC      $     $                           $     $ $$      $$$                                  S
 C         $     $                           $     $ $$     $$$                                SSS    Sum
C      $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$ $    $$$$
 C         $     $                      $    $    $ $$$$$$$$$
  CCC C_in $     $                      $    $   $$$$$$$$$$$
           $     $                      $    $
           $     $                      $    $     $$$$$$$
           $     $                      $    $     $$$$$$$$$$
           $     $                      $    $     $$     $$$$
           $     $                      $     $$$$$$$      $$$
           $     $                      $          $$       $$$
           $     $                      $          $$       $$$
           $     $                      $          $$  AND  $$$$$$$$
           $     $                      $          $$       $$$    $   $$$$$$$$
           $     $                      $          $$      $$$     $    $$$$$$$$$
           $     $                       $$$$$$$$$$$$     $$$$     $     $$   $$$$
           $     $                                 $$$$$$$$$$       $$$$$$$    $$$               CCC
           $     $                                 $$$$$$$$              $$     $$$             C
           $     $                                                       $$  OR $$$$$$$$$$$$$  C
           $     $                                 $$$$$$$               $$     $$$             C
           $     $                                 $$$$$$$$$$       $$$$$$$    $$$               CCC  C_out
           $     $                                 $$     $$$$     $     $$  $$$$
           $      $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$       $$$    $    $$$$$$$$
           $                                       $$        $$$   $   $$$$$$$
           $                                       $$  AND   $$$$$$
           $                                       $$        $$$
           $                                       $$       $$$
            $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$     $$$$
                                                   $$$$$$$$$$
                                                   $$$$$$$$


A half adder:

X   X                         $$$$$$$$$$$
 X X                            $ $$$$$$$$$
  X    $$$$$$$$$$$$$$$$$$$$$$$$$$$ $    $$$$
 X X             $               $ $$     $$$
X   X            $               $ $$      $$$
                 $               $ $$       $$$
Y   Y            $               $ $$  XOR  $$$$$$$$$   Sum
 Y Y             $               $ $$      $$$
  Y    $$$$$$$$$$$$$$$$$$$$$$$$$$$ $$     $$$
  Y        $     $               $ $    $$$$
  Y        $     $              $ $$$$$$$$$
           $     $             $$$$$$$$$$$
           $     $
           $     $
           $     $               $$$$$$$
           $     $               $$$$$$$$$$
           $     $               $$     $$$$
           $      $$$$$$$$$$$$$$$$$       $$$
           $                     $$        $$$
           $                     $$  AND   $$$$$$$$$$   C_out
           $                     $$        $$$
           $                     $$       $$$
            $$$$$$$$$$$$$$$$$$$$$$$     $$$$
                                 $$$$$$$$$$
                                 $$$$$$$$

*/
