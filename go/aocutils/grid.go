package aocutils

type Point2d struct {
	X, Y int
}

func Directions4() []Point2d {
	return []Point2d{{0, -1}, {1, 0}, {0, 1}, {-1, 0}}
}

func Directions8() []Point2d {
	return []Point2d{{0, -1}, {1, -1}, {1, 0}, {1, 1}, {0, 1}, {-1, 1}, {-1, 0}, {-1, -1}}
}

func (p Point2d) Neighbours4() []Point2d {
	return []Point2d{{p.X, p.Y - 1}, {p.X + 1, p.Y}, {p.X, p.Y + 1}, {p.X - 1, p.Y}}
}

func (p Point2d) InRange(xmin, ymin, xmax, ymax int) bool {
	return p.X >= xmin && p.X < xmax && p.Y >= ymin && p.Y < ymax
}

func InRange(x, y, xmin, ymin, xmax, ymax int) bool {
	return x >= xmin && x < xmax && y >= ymin && y < ymax
}

func InRange0(x, y, xmax, ymax int) bool {
	return x >= 0 && x < xmax && y >= 0 && y < ymax
}
