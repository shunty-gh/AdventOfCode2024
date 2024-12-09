input = "2333133121414131402"
input = open(f'../input/day09-input', 'r').read().strip()

def part1():
    fs = []
    for i in range(0, len(input), 2):
        for fl in range(int(input[i])):
            fs.append(i // 2)
        for sl in range(int(input[i+1]) if i+1 < len(input) else 0):
            fs.append(None)

    sp = 0
    ep = len(fs)-1
    while sp < ep:
        while fs[sp] is not None:
            sp += 1

        while fs[ep] is None:
            ep -= 1

        if sp < ep:
            fs[sp] = fs[ep]
            fs[ep] = None

        sp += 1
        ep -= 1

    return sum(i * (c or 0) for i,c in enumerate(fs))

def part2():
    fs = []
    for i in range(0, len(input), 2):
        fs.append((i // 2, int(input[i])))
        fs.append((None, int(input[i+1]) if i+1 < len(input) else 0))

    first_free = 1
    ep = len(fs) - 1
    while ep > 1:
        mv_id, mv_len = fs[ep]
        if mv_id is None:
            ep -= 1
            continue

        ff = 0
        for i in range(first_free, ep):
            v,l = fs[i]
            if v is None and ff<= 0:
                ff = i
            if v is None and l >= mv_len:
                fs.insert(i, (mv_id, mv_len))
                # shrink the free space after the moved file
                fs[i+1] = (None, l - mv_len)

                # Consolidate free space where the file used to be
                freespace = (None, mv_len)
                # check for free space immediately after the file we're moving
                if ep+2 < len(fs) and fs[ep+2][0] is None:
                    freespace = (None, fs[ep+2][1] + mv_len)
                    del fs[ep+2]

                # check for free space immediately before the file we're moving
                if fs[ep][0] is None:
                    fs[ep] = (None, fs[ep][1] + freespace[1])
                    del fs[ep+1]
                else: # otherwise, just mark this block as free
                    fs[ep+1] = freespace

                # delete the free space entry after the moved file if it is 0 length
                if i+1 < ep and l - mv_len == 0:
                    del fs[i+1]
                    first_free = ff

                break

        ep -= 1

    idx = 0
    result = 0
    for c, n in fs:
        for i in range(n):
            result += idx * (c or 0)
            idx += 1
    return result


print("Part 1:", part1())
print("Part 2:", part2())
