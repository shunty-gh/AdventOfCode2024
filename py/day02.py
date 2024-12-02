def test_line(line):
    # Are they all up or all down and close enough
    inc = all(x < y and y - x <= 3 for (x,y) in zip(line, line[1:]))
    dec = all(x > y and x - y <= 3 for (x,y) in zip(line, line[1:]))
    return inc or dec


with open(f'../input/day02-input', 'r') as file:
    lines = [line.strip().split(" ") for line in file.readlines()]
    lines = [[int(x) for x in line] for line in lines]

print("Part 1:", sum(1 for line in lines if test_line(line)))

part2 = 0
for line in lines:
    # Remove each element in turn and re-test the line
    for i in range(len(line)):
        ln = line[:i] + line[i+1:]  # or 'ln = line.copy()' ; 'del(ln[i])'
        if test_line(ln):
            part2 += 1
            break

print("Part 2:", part2)
