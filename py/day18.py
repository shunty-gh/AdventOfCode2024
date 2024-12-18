from collections import deque
import re

def minsteps(bytes: list[(int,int)], bytecount: int) -> int:
    xlen = max(bytes, key = lambda b: b[0])[0]
    ylen = max(bytes, key = lambda b: b[1])[1]

    bb = set(bytes[:bytecount])
    seen = dict([((0,0),0)])  # ((x,y),steps)
    q = deque([(0,0)])
    (tx, ty) = (xlen, ylen)
    while len(q):
        (cx, cy) = q.popleft()
        #if cx == tx and cy == ty:
        #    return seen[(cx,cy)]

        cost = seen[(cx,cy)]
        for (dx,dy) in [(0,-1), (1,0), (0,1), (-1,0)]:
            nx, ny = (cx + dx, cy + dy)
            if 0<=nx<=xlen and 0 <=ny<=ylen and (nx,ny) not in bb:
                if (nx,ny) not in seen or seen[(nx, ny)] > cost+1:
                    seen[(nx,ny)] = cost+1
                    q.append((nx,ny))

    return 0 if (tx,ty) not in seen else seen[(tx,ty)]

def pathexists(bytes: list[(int,int)], bytecount: int) -> bool:
    xlen = max(bytes, key = lambda b: b[0])[0]
    ylen = max(bytes, key = lambda b: b[1])[1]

    bb = set(bytes[:bytecount])
    seen = set([(0,0)])
    q = deque([(0,0)])
    (tx, ty) = (xlen, ylen)
    while len(q):
        (cx, cy) = q.popleft()
        if cx == tx and cy == ty:
            return True

        for (dx,dy) in [(0,-1), (1,0), (0,1), (-1,0)]:
            nx, ny = (cx + dx, cy + dy)
            if nx == tx and ny == ty:
                return True
            if 0<=nx<=xlen and 0<=ny<=ylen and (nx,ny) not in bb:
                if (nx,ny) not in seen:
                    seen.add((nx,ny))
                    q.append((nx,ny))

    return False

input = [int(x) for x in re.findall("\d+", open(f'../input/day18-input', 'r').read().strip())]
allbytes = []
i = 0
while i < len(input):
    allbytes.append((input[i], input[i+1]))
    i += 2

print("Part 1", minsteps(allbytes, 1024))

i = 1025
while True:
    if not pathexists(allbytes, i):
    #if minsteps(allbytes, i) == 0:   # pathexists() runs a tiny bit faster than minsteps
        break
    i += 1

print("Part 2", allbytes[i-1])
