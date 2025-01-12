package days

import (
	aoc "aoc2024/aocutils"
	"fmt"
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
	aoc.PrintResult(1, d.runProgram(program, registers[:]))
	aoc.PrintResult(2, 0)
}

func (d *day17) runProgram(program []int, registers []int64) string {
	output := make([]int64, 0)
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
			output = append(output, opvalue(operand)%8)
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
	result := ""
	for _, v := range output {
		result += fmt.Sprintf("%d,", v)
	}
	return result
}
