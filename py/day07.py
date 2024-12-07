from collections import deque

input = open(f'../input/day07-input', 'r').read().strip().split('\n')

def solveequation(target: int, nums: list[int], ispart2: bool) -> int:
    q = deque()
    q.appendleft(nums)
    while len(q) > 0:
        curr = q.popleft()
        if len(curr) == 1:
            if curr[0] == target:
                return target
            continue

        q.appendleft([curr[0] + curr[1]] + curr[2:])
        q.appendleft([curr[0] * curr[1]] + curr[2:])

        if ispart2:
            q.appendleft([int(str(curr[0]) + str(curr[1]))] + curr[2:])

    return 0


part1, part2 = 0, 0
for line in input:
    target = int(line.split(':')[0])
    nums = [int(x) for x in line.split(':')[1].split()]
    part1 += solveequation(target, nums, False)
    part2 += solveequation(target, nums, True)

print("Part 1:", part1)
print("Part 2:", part2)
