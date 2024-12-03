import re

input = open(f'../input/day03-input', 'r').read()

pattern1 = "mul\((\d+),(\d+)\)"
part1 = sum([int(x) * int(y) for x, y in re.findall(pattern1, input)])
print("Part 1:", part1)

pattern2 = "mul\(\d+,\d+\)|do\(\)|don\'t\(\)"
matches = re.findall(pattern2, input)
ok = True
part2 = 0
for match in matches:
    if match == "do()":
        ok = True
    elif match == "don't()":
        ok = False
    elif ok:
        part2 += sum([int(x) * int(y) for x,y in re.findall(pattern1, match)]) # there's only going to be one pair
print("Part 2:", part2)
