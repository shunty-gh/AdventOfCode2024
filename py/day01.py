import os
from collections import Counter

with open(f'../input/day01-input', 'r') as file:
    lines = [line.strip().split("  ") for line in file.readlines()]
    lines = [(int(parts[0]), int(parts[-1])) for parts in lines]

left1 = sorted(x[0] for x in lines)
right1 = sorted(x[1] for x in lines)

part1 = sum(abs(x - y) for x, y in zip(left1, right1))
print("Part 1:", part1)

left2 = Counter(left1)
right2 = Counter(right1)

part2 = sum(key * left2[key] * right2[key] for key in left2 if key in right2)
print("Part 2:", part2)
