input = "2333133121414131402"
input = open(f'../input/day09-input', 'r').read().strip()

def part1():
    fs = []
    for i in range(0, len(input), 2):
        id = i // 2
        flen = int(input[i])
        slen = int(input[i+1]) if i+1 < len(input) else 0

        for fl in range(flen):
            fs.append(id)
        for sl in range(slen):
            fs.append(' ')

    sp = 0
    ep = len(fs)-1
    while sp < ep:
        while fs[sp] != ' ':
            sp += 1

        while fs[ep] == ' ':
            ep -= 1

        if sp < ep:
            fs[sp] = fs[ep]
            fs[ep] = ' '

        sp += 1
        ep -= 1

    result = 0
    for i,c in enumerate(fs):
        if c != ' ':
            result += i * c
    return result

def part2():
    fs = []
    for i in range(0, len(input), 2):
        id = i // 2
        flen = int(input[i])
        slen = int(input[i+1]) if i+1 < len(input) else 0

        fs.append((id, flen))
        fs.append((' ', slen))

    ep = len(fs) - 1
    while ep > 1:
        mv_val, mv_len = fs[ep]
        if mv_val == ' ':
            ep -= 1
            continue

        for i in range(1, ep):
            v,l = fs[i]
            if v == ' ' and l >= mv_len:
                fs.insert(i, (mv_val, mv_len))
                fs[i+1] = (' ', l - mv_len)

                # Consolidate free space
                if fs[ep][0] == ' ' and ep+2 < len(fs) and fs[ep+2][0] == ' ':
                    fs[ep] = (' ', fs[ep][1] + fs[ep+2][1] + mv_len)
                    del fs[ep+2]
                    del fs[ep+1]
                elif fs[ep][0] == ' ':
                    fs[ep] = (' ', (fs[ep][1] + mv_len))
                    del fs[ep+1]
                elif ep+2 < len(fs) and fs[ep+2][0] == ' ':
                    fs[ep+2] = (' ', fs[ep+2][1] + mv_len)
                    del fs[ep+1]
                else:
                    fs[ep] = (' ', mv_len)

                break

        ep -= 1

    idx = 0
    result = 0
    for c, n in fs:
        for i in range(n):
            if c != ' ':
                result += idx * c
            idx += 1
    return result


print("Part 1:", part1())
print("Part 2:", part2())
