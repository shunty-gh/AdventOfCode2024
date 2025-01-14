package days

// https://adventofcode.com/2024/day/13 - Claw Contraption

import (
	aoc "aoc2024/aocutils"
	"regexp"
	"strconv"
)

type game struct {
	ax, ay, bx, by, tx, ty int64
}

func Day13() {
	const DAY = 13
	input, err := aoc.GetDayInputLines(DAY)
	aoc.CheckError(err)

	games := make([]game, 0, len(input)/4)
	re := regexp.MustCompile(`\d+`)
	for i := 0; i < len(input); i += 4 {
		matchA := re.FindAllString(input[i], -1)
		matchB := re.FindAllString(input[i+1], -1)
		matchT := re.FindAllString(input[i+2], -1)
		ax, _ := strconv.ParseInt(matchA[0], 10, 64)
		ay, _ := strconv.ParseInt(matchA[1], 10, 64)
		bx, _ := strconv.ParseInt(matchB[0], 10, 64)
		by, _ := strconv.ParseInt(matchB[1], 10, 64)
		tx, _ := strconv.ParseInt(matchT[0], 10, 64)
		ty, _ := strconv.ParseInt(matchT[1], 10, 64)
		games = append(games, game{ax, ay, bx, by, tx, ty})
	}

	var part1, part2 int64 = 0, 0
	for _, game := range games {
		part1 += gameCost(game, 0)
		part2 += gameCost(game, 10_000_000_000_000)
	}

	aoc.DayHeader(DAY)
	aoc.PrintResult(1, part1)
	aoc.PrintResult(2, part2)
}

func gameCost(game game, targetOffset int64) int64 {
	/*
		We have 2 simultaneous equations
		A * ax + B * bx = tx
		A * ay + B * by = ty

		We need to solve for the unknowns A and B
		| ax  bx | | A | = | tx |
		| ay  by | | B |   | ty |

		Using Cramers rule https://en.wikipedia.org/wiki/Cramer%27s_rule
		(or see the Python version ../py/day13.py for the basic math derivation):

		det(org) = ax * by - bx * ay
		det(A)   = tx * by - bx * ty
		det(B)   = ax * ty - tx * ay

		A = det(A) / det(org)
		B = det(B) / det(org)
	*/
	tx := game.tx + targetOffset
	ty := game.ty + targetOffset
	detOrg := game.ax*game.by - game.bx*game.ay
	detA := tx*game.by - game.bx*ty
	detB := game.ax*ty - tx*game.ay

	// The game can only deal in whole numbers of presses
	if detA%detOrg != 0 || detB%detOrg != 0 {
		return 0
	}

	// And they must be +ve
	a := detA / detOrg
	b := detB / detOrg
	if a < 0 || b < 0 {
		return 0
	}

	// "...it costs 3 tokens to push the A button and 1 token to push the B button"
	return 3*a + b
}
