package aocutils

import (
	"errors"
	"fmt"
	"os"
	"strconv"
	"strings"
)

func GetDayInputLines(day int) ([]string, error) {
	if day <= 0 || day >= 25 {
		return nil, fmt.Errorf("Invalid day number '%d'", day)
	}
	fn := fmt.Sprintf("../input/day%02d-input", day)
	return GetInputLines(fn)
}

func GetDayInputString(day int) (string, error) {
	if day <= 0 || day >= 25 {
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
		return strings.TrimSpace(string(in)), nil
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
