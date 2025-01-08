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

type Grid2d[T byte | int] struct {
	data         map[Point2d]T
	CharNotFound byte
}

func NewGrid[T byte | int]() *Grid2d[T] {
	result := new(Grid2d[T])
	result.data = make(map[Point2d]T)
	result.CharNotFound = ' '
	return result
}

func NewGridFromLines[T byte | int](lines []string) *Grid2d[T] {
	result := NewGrid[T]()
	result.ReadLines(lines, []byte{})
	return result
}

func NewGridFromLinesIgnore[T byte | int](lines []string, ignoreChars []byte) *Grid2d[T] {
	result := NewGrid[T]()
	result.ReadLines(lines, ignoreChars)
	return result
}

func (g *Grid2d[T]) ReadLines(lines []string, ignoreChars []byte) {
	for y := 0; y < len(lines); y++ {
		line := lines[y]
		for x := 0; x < len(line); x++ {
			c := line[x]
			if slices.Contains(ignoreChars, c) {
				continue
			}
			p := Point2d{x, y}
			var def T
			switch any(def).(type) {
			case int:
				g.data[p] = T(c - '0')
			case byte:
				g.data[p] = T(c)
			}
		}
	}
}

func (g *Grid2d[T]) CharAt(p Point2d) T {
	return g.data[p]
}

func (g *Grid2d[T]) CharAtXY(x, y int) T {
	p := Point2d{x, y}
	return g.data[p]
}

func (g *Grid2d[T]) Find(c T) (Point2d, error) {
	for k, v := range g.data {
		if v == c {
			return k, nil
		}
	}
	return Point2d{0, 0}, fmt.Errorf("Value '%v' not found in grid", c)
}

func (g *Grid2d[T]) FindAll(c T) ([]Point2d, error) {
	result := make([]Point2d, 0, len(g.data)/2)

	for k, v := range g.data {
		if v == c {
			result = append(result, k)
		}
	}
	return result, nil
}
