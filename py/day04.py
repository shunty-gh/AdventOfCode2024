def letter_at(x, y, grid):
    return ' ' if x < 0 or x >= len(grid[0]) or y < 0 or y >= len(grid) else grid[y][x]

def find_mas(x, y, grid):
    # pt (x,y) is an X. Now find M A S in all directions.
    result = 0
    for dx,dy in [(0,-1), (1,-1), (1,0), (1,1), (0,1), (-1,1), (-1,0), (-1,-1)]:
        px = x+dx
        py = y+dy
        if letter_at(px, py, grid) == 'M' and letter_at(px + dx, py + dy, grid) == 'A' and letter_at(px + dx + dx, py + dy + dy, grid) == 'S':
            result += 1

    return result

def find_x_mas(x, y, grid):
    # pt (x,y) is an A
    upl = letter_at(x - 1, y - 1, grid)
    upr = letter_at(x + 1, y - 1, grid)
    dnl = letter_at(x - 1, y + 1, grid)
    dnr = letter_at(x + 1, y + 1, grid)

    diag1 = (upl == 'M' and dnr == 'S') or (upl == 'S' and dnr == 'M')
    diag2 = (upr == 'M' and dnl == 'S') or (upr == 'S' and dnl == 'M')
    return diag1 and diag2

with open(f'../input/day04-input', 'r') as file:
    lines = [line.strip() for line in file.readlines()]

part1 = 0
part2 = 0
for y,line in enumerate(lines):
    for x,c in enumerate(line):
        if c == 'X':
            part1 += find_mas(x, y, lines)
        elif c == 'A':
            part2 += find_x_mas(x, y, lines)

print("Part 1:", part1)
print("Part 2:", part2)
