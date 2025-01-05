package aocutils

import (
	"fmt"
	"slices"
)

type Point2d struct {
	X, Y int
}

type Direction struct {
	X, Y int
}

//DirectionUp Direction = Direction{0, -1}

func Directions4() []Direction {
	return []Direction{{0, -1}, {1, 0}, {0, 1}, {-1, 0}}
}

func Directions8() []Direction {
	return []Direction{{0, -1}, {1, -1}, {1, 0}, {1, 1}, {0, 1}, {-1, 1}, {-1, 0}, {-1, -1}}
}

func (d Direction) TurnRight() Direction {
	return Direction{-d.Y, d.X}
}

func (p Point2d) Neighbours4() []Point2d {
	return []Point2d{{p.X, p.Y - 1}, {p.X + 1, p.Y}, {p.X, p.Y + 1}, {p.X - 1, p.Y}}
}

func (p Point2d) InRange(xmin, ymin, xmax, ymax int) bool {
	return p.X >= xmin && p.X < xmax && p.Y >= ymin && p.Y < ymax
}

func (p Point2d) InRange0(xmax, ymax int) bool {
	return p.X >= 0 && p.X < xmax && p.Y >= 0 && p.Y < ymax
}

func (p Point2d) Add(dx, dy int) Point2d {
	return Point2d{p.X + dx, p.Y + dy}
}

func InRange(x, y, xmin, ymin, xmax, ymax int) bool {
	return x >= xmin && x < xmax && y >= ymin && y < ymax
}

func InRange0(x, y, xmax, ymax int) bool {
	return x >= 0 && x < xmax && y >= 0 && y < ymax
}

type Grid2d struct {
	data         map[Point2d]byte
	CharNotFound byte
}

func NewGrid() *Grid2d {
	result := new(Grid2d)
	result.data = make(map[Point2d]byte)
	result.CharNotFound = ' '
	return result
}

func NewGridFromLines(lines []string) *Grid2d {
	result := NewGrid()
	result.ReadLines(lines, []byte{})
	return result
}

func NewGridFromLinesIgnore(lines []string, ignoreChars []byte) *Grid2d {
	result := NewGrid()
	result.ReadLines(lines, ignoreChars)
	return result
}

func (g *Grid2d) ReadLines(lines []string, ignoreChars []byte) {
	for y := 0; y < len(lines); y++ {
		line := lines[y]
		for x := 0; x < len(line); x++ {
			c := line[x]
			if slices.Contains(ignoreChars, c) {
				continue
			}
			p := Point2d{x, y}
			g.data[p] = c
		}
	}
}

func (g *Grid2d) CharAt(p Point2d) byte {
	return g.data[p]
}

func (g *Grid2d) CharAtXY(x, y int) byte {
	p := Point2d{x, y}
	return g.data[p]
}

func (g *Grid2d) Find(c byte) (Point2d, error) {
	for k, v := range g.data {
		if v == c {
			return k, nil
		}
	}
	return Point2d{0, 0}, fmt.Errorf("Value '%v' not found in grid", c)
}
