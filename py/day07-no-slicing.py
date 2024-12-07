import time
from collections import deque

def solveequation(target: int, nums: list[int], ispart2: bool) -> int:
    nlen = len(nums)
    q = deque()
    q.appendleft((nums[0], 1))
    while len(q) > 0:
        (acc, idx) = q.popleft()

        next = nums[idx]
        np = acc + next
        nt = acc * next
        nc = int(str(acc) + str(next)) if ispart2 else 0
        if idx+1 == nlen:
            if (np == target or nt == target or (ispart2 and nc == target)):
                return target
            continue

        if np <= target: q.appendleft((np, idx+1))
        if nt <= target: q.appendleft((nt, idx+1))

        if ispart2 and nc <= target: # Add the concatenation of the first two numbers
            q.appendleft((nc, idx+1))

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
