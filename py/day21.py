
# +---+---+---+
# | 7 | 8 | 9 |
# +---+---+---+
# | 4 | 5 | 6 |
# +---+---+---+
# | 1 | 2 | 3 |
# +---+---+---+
#     | 0 | A |
#     +---+---+

#     +---+---+
#     | ^ | A |
# +---+---+---+
# | < | v | > |
# +---+---+---+

def buildbuttonmap():
    # Create a map of all moves from button a to button b. Use the same map for number
    # and direction pads
    # Ignore zigzag paths 'cos they have to result in longer final sequences as it will
    # be better to have same direction arrows next to each other so that extra moves aren't required
    # in the child pad

    # Hard code the direction pads, they're easy
    result = {
        "AA": ["A"],           "A^": ["<A"],         "A<": ["v<<A"],  "Av": ["<vA", "v<A"],  "A>": ["vA"],
        "^A": [">A"],          "^^": ["A"],          "^<": ["v<A"],   "^v": ["vA"],          "^>": ["v>A", ">vA"],
        "<A": [">>^A"],        "<^": [">^A"],        "<<": ["A"],     "<v": [">A"],          "<>": [">>A"],
        "vA": ["^>A", ">^A"],  "v^": ["^A"],         "v<": ["<A"],    "vv": ["A"],           "v>": [">A"],
        ">A": ["^A"],          ">^": ["<^A", "^<A"], "><": ["<<A"],   ">v": ["<A"],          ">>": ["A"],
    }

    numpad = {
        '7': (0,0), '8': (1,0), '9': (2,0),
        '4': (0,1), '5': (1,1), '6': (2,1),
        '1': (0,2), '2': (1,2), '3': (2,2),
        ' ': (0,3), '0': (1,3), 'A': (2,3),
    }
    for b0 in ['A', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9']:
        for b1 in ['A', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9']:
            k = b0+b1
            if b0 == b1:
                result[k] = ["A"]
                continue
            x0,y0 = numpad[b0]
            x1,y1 = numpad[b1]
            dx,dy = x1 - x0, y1 - y0
            # Straight lines only, avoiding the empty space
            result[k] = []
            # Left/right first
            if dx > 0 or not (y0 == 3 and (x0 == 0 or x1 == 0)):
                path = ""
                path += '<' * -dx if dx < 0 else '>' * dx
                path += '^' * -dy if dy < 0 else 'v' * dy
                path += "A"
                result[k].append(path)

            # Up/down first
            if not (dx == 0 or dy == 0): # Only if path is not a single straight line
                if dy < 0 or not (y1 == 3 and (x0 == 0 or x1 == 0)):
                    path = ""
                    path += '^' * -dy if dy < 0 else 'v' * dy
                    path += '<' * -dx if dx < 0 else '>' * dx
                    path += "A";
                    result[k].append(path)

    return result

cache = {}
def pathlength(path: str, level: int) -> int:
    if level == 0:
        return len(path)
    if (path,level) in cache:
        return cache[(path,level)]

    result = 0
    apath = "A" + path
    for i in range(len(path)):
        frag = apath[i:(i+2)]
        result += min([pathlength(p, level-1) for p in padmap[frag]])

    cache[(path,level)] = result
    return result


input = [s.strip() for s in open(f'../input/day21-input', 'r').read().strip().split("\n")]
padmap = buildbuttonmap()
#print (map)

part1 = sum(int(code[:-1]) * pathlength(code, 3) for code in input)  # 2 dir pads + 1 num pad
part2 = sum(int(code[:-1]) * pathlength(code, 26) for code in input) # 25 dir pads + 1 num pad

print("Part 1:", part1)
print("Part 2:", part2)
