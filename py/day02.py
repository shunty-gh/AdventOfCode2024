import os

def test_line(line):
    # Are they all up or all down
    inc = all(x < y for (x,y) in zip(line, line[1:]))  # or, perhaps: line == sorted(line)
    dec = all(x > y for (x,y) in zip(line, line[1:]))  #              line == sorted(line, reverse=True)
    # If so, are they close enough together
    if inc or dec:
        for i in range(len(line) - 1):
            if abs(line[i] - line[i+1]) > 3:
                return False
    return inc or dec


with open(f'../input/day02-input', 'r') as file:
    lines = [line.strip().split(" ") for line in file.readlines()]
    lines = [[int(x) for x in line] for line in lines]

print("Part 1:", sum(1 for line in lines if test_line(line)))

part2 = 0
for line in lines:
    for i in range(len(line)):
        #ln = line.copy()
        #del(ln[i])
        ln = line[:i] + line[i+1:]
        if test_line(ln):
            part2 += 1
            break

print("Part 2:", part2)
