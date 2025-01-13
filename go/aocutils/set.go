package aocutils

type Set[K comparable] struct {
	data map[K]struct{}
}

func ToSet[K comparable](src []K) *Set[K] {
	result := &Set[K]{data: make(map[K]struct{}, len(src))}
	for _, v := range src {
		result.data[v] = struct{}{}
	}
	return result
}

func NewSet[K comparable]() *Set[K] {
	return &Set[K]{data: make(map[K]struct{})}
}

func (s *Set[K]) Contains(k K) bool {
	_, ok := s.data[k]
	return ok
}

func (s *Set[K]) Add(k K) {
	s.data[k] = struct{}{}
}

func (s *Set[K]) Remove(k K) {
	delete(s.data, k)
}

func (s *Set[K]) Count() int {
	return len(s.data)
}

func (s *Set[K]) Keys() []K {
	result := make([]K, 0, len(s.data))
	for k := range s.data {
		result = append(result, k)
	}
	return result
}

func (s *Set[K]) Union(other *Set[K]) *Set[K] {
	result := NewSet[K]()
	for k := range s.data {
		result.Add(k)
	}
	for k := range other.data {
		result.Add(k)
	}
	return result
}

func (s *Set[K]) Intersect(other *Set[K]) *Set[K] {
	result := NewSet[K]()
	for k := range s.data {
		if other.Contains(k) {
			result.Add(k)
		}
	}
	return result
}
