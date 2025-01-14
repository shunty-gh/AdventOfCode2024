package aocutils

import (
	"cmp"
	"errors"
	"fmt"
	"os"
	"regexp"
	"strconv"
	"strings"
)

func GetDayInputLines(day int) ([]string, error) {
	if day <= 0 || day > 25 {
		return nil, fmt.Errorf("Invalid day number '%d'", day)
	}
	fn := fmt.Sprintf("../input/day%02d-input", day)
	return GetInputLines(fn)
}

func GetDayInputString(day int) (string, error) {
	if day <= 0 || day > 25 {
		return "", fmt.Errorf("Invalid day number '%d'", day)
	}
	fn := fmt.Sprintf("../input/day%02d-input", day)
	return GetInputString(fn)
}

func GetInputLines(fileName string) ([]string, error) {
	in, err := os.ReadFile(fileName)
	if err != nil {
		return nil, err
	} else {
		return strings.Split(strings.TrimSpace(string(in)), "\n"), nil
	}
}

func GetInputString(fileName string) (string, error) {
	in, err := os.ReadFile(fileName)
	if err != nil {
		return "", err
	} else {
		return strings.Replace(strings.TrimSpace(string(in)), "\r\n", "\n", -1), nil
	}
}

func CheckError(err error) {
	if err != nil {
		panic(err)
	}
}

func StringToInts(s string, sep string) ([]int, error) {
	s = strings.TrimSpace(s)
	if s == "" {
		return nil, errors.New("Empty string, no numbers found")
	}
	strs := strings.Split(s, sep)
	result := make([]int, len(strs))
	for i := range result {
		n, err := strconv.Atoi(strings.TrimSpace(strs[i]))
		if err != nil {
			return nil, err
		} else {
			result[i] = n
		}
	}
	return result, nil
}

func StringToInts64(s string, sep string) ([]int64, error) {
	s = strings.TrimSpace(s)
	if s == "" {
		return nil, errors.New("Empty string, no numbers found")
	}
	strs := strings.Split(s, sep)
	result := make([]int64, len(strs))
	for i := range result {
		n, err := strconv.Atoi(strings.TrimSpace(strs[i]))
		if err != nil {
			return nil, err
		} else {
			result[i] = int64(n)
		}
	}
	return result, nil
}

func FindAllInts[T byte | int8 | int | int16 | int32 | int64](s string) ([]T, error) {
	result := make([]T, 0)
	numRe := regexp.MustCompile(`[\+\-]?\d+`)
	matches := numRe.FindAllString(s, -1)
	for _, m := range matches {
		v, err := strconv.ParseInt(m, 10, 64)
		if err != nil {
			return []T{}, err
		}
		num := T(v)
		result = append(result, num)
	}
	return result, nil
}

func IntsToStr[T byte | int8 | int | int16 | int32 | int64](nums []T, separator string) string {
	result := ""
	for _, v := range nums {
		result += fmt.Sprintf("%s%d", separator, v)
	}
	return result[1:] // Don't return the first separator
}

func Abs(a int) int {
	if a < 0 {
		return -a
	}
	return a
}

func DayHeader(day int) {
	fmt.Printf("Day %d\n", day)
}

func PrintResult[R any](part int, result R) {
	fmt.Printf("  Part %d: %v\n", part, result)
}

// Simple inplace reverse of a slice
// func Reverse[T any](a []T) []T {
// 	for left, right := 0, len(a)-1; left < right; left, right = left+1, right-1 {
// 		a[left], a[right] = a[right], a[left]
// 	}
// 	return a
// }

// An intersection method for two *sorted* slices of an ordered type T
func Intersect[T cmp.Ordered](s1, s2 []T) []T {
	minlen := len(s1)
	if len(s2) < minlen {
		minlen = len(s2)
	}
	common := make([]T, 0, minlen)
	for i, j := 0, 0; i < len(s1) && j < len(s2); {
		if s1[i] == s2[j] {
			common = append(common, s1[i])
			i++
			j++
		} else if s1[i] < s2[j] {
			i++
		} else {
			j++
		}
	}
	return common
}
