package aocutils

type Counter[K comparable] struct {
	data map[K]int
}

func NewCounter[K comparable]() *Counter[K] {
	return &Counter[K]{data: make(map[K]int)}
}

func (c *Counter[K]) Contains(k K) bool {
	_, ok := c.data[k]
	return ok
}

func (c *Counter[K]) Add(k K) {
	if v, ok := c.data[k]; !ok {
		c.data[k] = 1
	} else {
		c.data[k] = v + 1
	}
}

func (c *Counter[K]) Inc(k K, amount int) {
	if v, ok := c.data[k]; !ok {
		c.data[k] = amount
	} else {
		c.data[k] = v + amount
	}
}

func (c *Counter[K]) Set(k K, v int) {
	c.data[k] = v
}

func (c *Counter[K]) Get(k K) int {
	if v, ok := c.data[k]; ok {
		return v
	}
	return 0
}

func (c *Counter[K]) TryGet(k K) (int, bool) {
	if v, ok := c.data[k]; ok {
		return v, true
	}
	return 0, false
}

func (c *Counter[K]) Count() int {
	return len(c.data)
}

func (c *Counter[K]) Keys() []K {
	result := make([]K, 0, len(c.data))
	for k := range c.data {
		result = append(result, k)
	}
	return result
}

func (c *Counter[K]) Values() []int {
	result := make([]int, 0, len(c.data))
	for _, v := range c.data {
		result = append(result, v)
	}
	return result
}

// Return the larget count in the set
func (c *Counter[K]) MaxValue() int {
	result := 0
	for _, v := range c.data {
		if v > result {
			result = v
		}
	}
	return result
}

// Return the key for the entry with the largest count
func (c *Counter[K]) MaxKey() K {
	var result K
	maxv := 0
	for k, v := range c.data {
		if v > maxv {
			maxv = v
			result = k
		}
	}
	return result
}

type CounterKeyValuePair[K comparable] struct {
	Key   K
	Value int
}

func (c *Counter[K]) Items() []CounterKeyValuePair[K] {
	result := make([]CounterKeyValuePair[K], 0, len(c.data))
	for k, v := range c.data {
		result = append(result, CounterKeyValuePair[K]{k, v})
	}
	return result
}

func (c *Counter[K]) Filter(fn func(k K, v int) bool) []CounterKeyValuePair[K] {
	result := make([]CounterKeyValuePair[K], 0)
	for k, v := range c.data {
		if fn(k, v) {
			result = append(result, CounterKeyValuePair[K]{k, v})
		}
	}
	return result
}
