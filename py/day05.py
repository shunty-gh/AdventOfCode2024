from collections import defaultdict
from functools import cmp_to_key

with open(f'../input/day05-input', 'r') as file:
    (rls, runs) = file.read().split('\n\n')

rls = [(int(k), int(v)) for k, v in [rule.split('|') for rule in rls.split('\n')]]
rules = defaultdict(list)
for (k,v) in rls:
    rules[k].append(v)
    if v not in rules:
        rules[v] = []

part1, part2 = 0, 0
runs = [[int(r) for r in run] for run in [run.split(',') for run in runs.split('\n') if run != '']]
for run in runs:
    ok = True
    for i in range(len(run)-1):
        if run[i+1] not in rules[run[i]]:
            ok = False
            break
    if ok:
        part1 += run[int((len(run) - 1) / 2)]
    else:
        resort = sorted(run, key=cmp_to_key(lambda x, y: 1 if x in rules[y] else -1))
        part2 += resort[int((len(resort) - 1) / 2)]

print(part1)
print(part2)
