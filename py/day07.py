import time
from collections import deque

def solveequation(target: int, nums: list[int], ispart2: bool) -> int:
    q = deque()
    q.appendleft(nums)
    while len(q) > 0:
        curr = q.popleft()

        acc = curr[0]
        next = curr[1]
        np = acc + next
        nt = acc * next
        nc = int(str(acc) + str(next)) if ispart2 else 0
        if len(curr) == 2:
            if (np == target or nt == target or (ispart2 and nc == target)):
                return target
            continue

        if np <= target: q.appendleft([np] + curr[2:])
        if np <= target: q.appendleft([nt] + curr[2:])

        if ispart2 and nc <= target: # Add the concatenation of the first two numbers
            q.appendleft([nc] + curr[2:])

    return 0 # No solution found


input = open(f'../input/day07-input', 'r').read().strip().split('\n')

start = time.time()
part1, part2 = 0, 0
for line in input:
    target = int(line.split(':')[0])
    nums = [int(x) for x in line.split(':')[1].split()]
    part1 += solveequation(target, nums, False)
    part2 += solveequation(target, nums, True)

stop = time.time() - start
print("Part 1:", part1)
print("Part 2:", part2)
print("Time:", stop)
