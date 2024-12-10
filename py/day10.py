from collections import deque

def go_rambling(grid, start: tuple[int,int]) -> tuple[int,int]:
    ylen, xlen = len(grid), len(grid[0])
    q: deque[tuple[int,int]] = deque()
    q.append(start)
    visited = set()
    routes = 0
    while len(q) > 0:
        cx, cy = q.popleft()
        cv = int(grid[cy][cx])

        for dx,dy in [(0,-1), (1,0), (0,1), (-1,0)]: # Go N, E, S, W
            nx, ny = cx + dx, cy + dy
            if 0 <= nx < xlen and 0 <= ny < ylen:
                nv = int(grid[ny][nx]) if '0' <= grid[ny][nx] <= '9' else 0
                if nv == 9 and cv == 8:    # Reached the peak of the trail, via a valid path
                    visited.add((nx, ny))  # Part1 requires how many unique '9's are visited per start point
                    routes += 1            # Part2 asks how many different routes there are to the top - ie in this case, the total number of times we get there
                elif nv == cv + 1:
                    q.append((nx, ny))     # Otherwise, take a step in the right direction

    return len(visited), routes

#input = "89010123\n78121874\n87430965\n96549874\n45678903\n32019012\n01329801\n10456732".strip().split("\n")
input = open(f'../input/day10-input', 'r').read().strip().split("\n")

# find all the possible start points ('trail heads')
starts = []
for y,ln in enumerate(input):
    for x,ch in enumerate(ln):
        if ch == '0':
            starts.append((x,y))

p1, p2 = 0, 0
for a,b in [go_rambling(input, start) for start in starts]:
    p1 += a
    p2 += b

print("Part 1:", p1)
print("Part 2:", p2)
