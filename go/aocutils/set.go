package aocutils

func ToSet[K comparable](src []K) map[K]bool {
	result := make(map[K]bool, len(src))
	for _, v := range src {
		result[v] = true
	}
	return result
}

func SetContains[K comparable, V any](set map[K]V, v K) bool {
	_, ok := set[v]
	return ok
}

func NewSet[K comparable]() map[K]bool {
	return make(map[K]bool)
}

func NewCounter[K comparable]() map[K]int {
	return make(map[K]int)
}
