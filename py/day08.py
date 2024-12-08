from collections import defaultdict

input = open(f'../input/day08-input', 'r').read().strip().split('\n')

antenna = defaultdict(list[(int,int)])
for y,ln in enumerate(input):
    for x,c in enumerate(ln):
        if c != '.':
            antenna[c].append((x,y))

rlen = len(input[0])
clen = len(input)
p1 = set()
p2 = set()
for ant in antenna.values():
    for i in range(len(ant)-1):
        for j in range(i+1,len(ant)):
            (ax, ay) = ant[i]
            (bx, by) = ant[j]
            (dx, dy) = (ax - bx, ay - by)

            mul = 1
            upok = True
            dnok = True
            while upok or dnok:
                (upx, upy) = (ax + mul * dx, ay + mul * dy)
                (dnx, dny) = (bx - mul * dx, by - mul * dy)

                upok = 0 <= upx < rlen and 0 <= upy < clen
                dnok = 0 <= dnx < rlen and 0 <= dny < clen

                if mul == 1: # Part 1
                    if upok: p1.add((upx, upy))
                    if dnok: p1.add((dnx, dny))

                if upok: p2.add((upx, upy))
                if dnok: p2.add((dnx, dny))

                mul += 1
            # Add each antenna
            p2.add((ax, ay))
            p2.add((bx, by))

print("Part 1:", len(p1))
print("Part 2:", len(p2))