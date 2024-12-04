# this, perhaps, takes 'pythonic' too far
def letter_at(x, y, grid):
    return ' ' if x < 0 or x >= len(grid[0]) or y < 0 or y >= len(grid) else grid[y][x]

def find_mas(x, y, grid):
    if letter_at(x, y, grid) != 'X': return 0
    return sum((1 if all([letter_at(x+dx*i, y+dy*i, grid) == ch for i, ch in [(1,'M'), (2,'A'), (3, 'S')]]) else 0) for dx,dy in [(0,-1), (1,-1), (1,0), (1,1), (0,1), (-1,1), (-1,0), (-1,-1)])

def find_x_mas(x, y, grid):
    if letter_at(x, y, grid) != 'A': return False
    return all(diag in [['M', 'S'], ['S', 'M']] for diag in [ [letter_at(x - 1, y - 1, grid), letter_at(x + 1, y + 1, grid)], [letter_at(x + 1, y - 1, grid), letter_at(x - 1, y + 1, grid)] ])

with open(f'../input/day04-input', 'r') as file:
    lines = [line.strip() for line in file.readlines()]

part1, part2 = 0, 0
for yy,line in enumerate(lines):
    for xx in range(0, len(line)):
        part1 += find_mas(xx, yy, lines)
        part2 += find_x_mas(xx, yy, lines)

print("Part 1:", part1)
print("Part 2:", part2)
