import re

def cheapest(machine) -> int:
    ax, ay, bx, by, tx, ty = machine
    a = (ty * bx - by * tx) // (ay * bx - by * ax)
    b = (tx - a * ax) // bx
    return 3 * a + b if a * ax + b * bx == tx and a * ay + b * by == ty else 0

input = [line.strip() for line in open(f'../input/day13-input', 'r').read().strip().split('\n')]
i, p1, p2, offset = 0, 0, 0, 10000000000000
while i < len(input):
    ax, ay = [int(x) for x in re.findall("\d+", input[i])]
    bx, by = [int(x) for x in re.findall("\d+", input[i+1])]
    px, py = [int(x) for x in re.findall("\d+", input[i+2])]
    i += 4

    p1 += cheapest((ax, ay, bx, by, px, py))
    p2 += cheapest((ax, ay, bx, by, px + offset, py + offset))

print(p1)
print(p2)

# From basic rearranging of 2 simultaneous equations
#
# a * ax + b * bx = tx
# a * ay + b * by = ty
#
# b = (tx - a * ax) / bx
#
# a * ay + by * (tx - a * ax) / bx = ty
# a * ay + (by * tx - a * by * ax) / bx = ty
# a * ay * bx + by * tx - a * by * ax = ty * bx
# a * (ay * bx - by * ax) = ty * bx - by * tx
# a = (ty * bx - by * tx) / (ay * bx - by * ax)
