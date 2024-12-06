input = open(f'../input/day06-input', 'r').read().strip().split('\n')

ylen = len(input)
xlen = len(input[0])
# Find start
for y in range(ylen):
    for x in range(xlen):
        if input[y][x] == '^':
            (startX, startY) = (x, y)
            break

(dX, dY) = (0, -1)
(cX, cY) = (startX, startY)
visited = set()
visited.add((cX, cY))
part1 = 0
while True:
    (nX, nY) = (cX + dX, cY + dY)
    if not (0 <= nX < xlen and 0 <= nY < ylen):
        part1 = len(visited)
        break
    if input[nY][nX] == '#':
        (dX, dY) = (-dY, dX)
    else:
        visited.add((nX, nY))
        (cX, cY) = (nX, nY)

print(part1)