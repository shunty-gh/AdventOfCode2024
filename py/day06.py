def walkroute(grid, sX, sY, oX, oY):
    p2 = oX >= 0
    (dX, dY) = (0, -1)
    (cX, cY) = (sX, sY)
    visited = set()
    if p2: visited.add((cX, cY, dX, dY))
    else: visited.add((cX, cY))

    while True:
        (nX, nY) = (cX + dX, cY + dY)

        # Part1 - guard has left the area
        if not (0 <= nX < xlen and 0 <= nY < ylen):
            return len(visited), visited

        # Have we found a loop - ie we've seen this location AND this direction before
        if p2 and (nX, nY, dX, dY) in visited:
            return -1, []

        # Hit an obstacle? Turn right.
        if input[nY][nX] == '#' or (oX, oY) == (nX, nY):
            (dX, dY) = (-dY, dX)
        else: # otherwise, record it and move on
            if p2: visited.add((nX, nY, dX, dY))
            else: visited.add((nX, nY))
            (cX, cY) = (nX, nY)


input = open(f'../input/day06-input', 'r').read().strip().split('\n')

xlen, ylen = len(input[0]), len(input)
startX = -1
# Find start
for y in range(ylen):
    for x in range(xlen):
        if input[y][x] == '^':
            (startX, startY) = (x, y)
            break
    if startX >= 0: break

part1, p1visited = walkroute(input, startX, startY, -1, -1)
print("Part 1:", part1)

part2 = 0
for oby in range(ylen):
    for obx in range(xlen):
        # Only try obstacles at points that the original route visted. If the point wasn't
        # in the original route then an obstacle at that point won't make any difference
        if input[oby][obx] == '.' and (obx, oby) in p1visited:
            p2, _ = walkroute(input, startX, startY, obx, oby)
            if p2 < 0: part2 += 1

print("Part 2:", part2)
