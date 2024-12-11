def evolve(stones: list[int], count: int) -> int:
    return sum([evolvestone(stone, count) for stone in stones])

def evolvestone(stone: int, count: int) -> int:
    if (stone, count) in seen:
        return seen[(stone, count)]

    # Are we at the end of the recursion
    if count == 1:
        return 1 if stone == 0 or len(str(stone)) % 2 != 0 else 2

    if len(str(stone)) % 2 == 0:
        s = str(stone)
        nums = [int(s[:(len(s) // 2)]), int(s[(len(s) // 2):])]
    else:
        nums = [1] if stone == 0 else [stone * 2024]

    # Cursed recursion
    result = 0
    for num in nums:
        if (num, count-1) not in seen:
            res = evolvestone(num, count-1)
            seen[(num, count-1)] = res
        result += seen[(num, count-1)]
    return result

#input = [int(x) for x in "125 17".strip().split(" ")]  ## P1 expect 55312
input = [int(x) for x in open(f'../input/day11-input', 'r').read().strip().split(" ")]

seen = {}
print("Part 1:", evolve(input, 25))
seen = {}
print("Part 2:", evolve(input, 75))
