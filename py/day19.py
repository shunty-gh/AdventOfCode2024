def perms(towels: list[str], pattern: str) -> int:
    if pattern == "":
        return 1
    if pattern in cache:
        return cache[pattern]
    result = 0
    for towel in towels:
        if pattern.startswith(towel):
            result += perms(towels, pattern[len(towel):])
    cache[pattern] = result
    return result

input = open(f'../input/day19-input', 'r').read().strip().split("\n\n")
towels = input[0].split(", ")
patterns = input[1].split("\n")

cache = {} # the cache can be global, we don't need to clear it between runs as it's the same set of towels for each
p1 = sum(1 if perms(towels, pattern) > 0 else 0 for pattern in patterns)
print("Part 1:", p1)

p2 = sum(perms(towels, pattern) for pattern in patterns)
print("Part 2:", p2)
