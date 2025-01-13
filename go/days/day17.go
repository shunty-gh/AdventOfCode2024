package days

import (
	aoc "aoc2024/aocutils"
	"strconv"
	"strings"
)

type day17 struct {
	day int
}

func Day17() {
	d := &day17{17}
	d.Run()
}

func (d *day17) Run() {
	input, err := aoc.GetDayInputString(d.day)
	aoc.CheckError(err)

	sp := strings.Split(strings.TrimSpace(input), "\n\n")
	regs := strings.Split(sp[0], "\n")
	registers := [3]int64{0, 0, 0}
	for i := 0; i < 3; i++ {
		registers[i], _ = strconv.ParseInt(strings.Split(strings.TrimSpace(regs[i]), " ")[2], 10, 64)
	}
	program, _ := aoc.StringToInts(strings.Split(strings.TrimSpace(sp[1]), " ")[1], ",")

	aoc.DayHeader(d.day)
	aoc.PrintResult(1, aoc.IntsToStr(d.runProgram(program, registers[:]), ","))
	aoc.PrintResult(2, d.part2(program))
}

func (d *day17) runProgram(program []int, registers []int64) []int {
	output := make([]int, 0)
	reg := append([]int64{}, registers...)
	opvalue := func(op int64) int64 {
		if op <= 3 {
			return int64(op)
		}
		return reg[op-4]
	}

	var ip int64 = 0
	for ip < int64(len(program)) {
		opcode := int64(program[ip])
		operand := int64(program[ip+1])

		jnz := false
		switch opcode {
		case 0:
			reg[0] = reg[0] / (1 << opvalue(operand))
		case 1:
			reg[1] = reg[1] ^ operand
		case 2:
			reg[1] = opvalue(operand) % 8
		case 3:
			jnz = reg[0] != 0
		case 4:
			reg[1] = reg[1] ^ reg[2]
		case 5:
			output = append(output, int(opvalue(operand)%8))
		case 6:
			reg[1] = reg[0] / (1 << opvalue(operand))
		case 7:
			reg[2] = reg[0] / (1 << opvalue(operand))
		default:
			panic("Unknown opcode")
		}
		if jnz {
			ip = operand
		} else {
			ip += 2
		}
	}
	return output
}

func (d *day17) part2(program []int) int64 {
	return d.solveForA(program, program, 0)
}

// For part 2 we work backwards trying to solve each digit from the last to the first.
// It's a 3-bit computer so we run the program for each value 0..7 to get the current
// target digit then multiply by eight and try and solve the next digit.
func (d *day17) solveForA(program []int, target []int, currentA int64) int64 {
	if len(target) == 0 {
		return currentA
	}

	for i := range 8 {
		nexta := currentA*int64(8) + int64(i)
		regs := []int64{nexta, 0, 0}
		soln := d.runProgram(program, regs)
		// The solution will slowly build to the full program so for each level we
		// want the first digit in our current solution to equal the last digit in
		// the target
		if soln[0] != target[len(target)-1] {
			continue
		}
		// If correct so far, then solve for the next digit in the target using
		// the current value of A
		sol := d.solveForA(program, target[:len(target)-1], nexta)
		if sol < 0 {
			continue
		} else {
			return sol
		}
	}
	return -1
}
