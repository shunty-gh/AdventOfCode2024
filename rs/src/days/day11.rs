use std::collections::HashMap;
use crate::aocutils::*;

pub fn run() {
    let day = 11;
    let filename = find_input_file(&day).unwrap();
    let stones = get_text_from_file(&filename).unwrap()
        .split(' ')
        .map(|s| s.parse::<i64>().unwrap())
        .collect::<Vec<i64>>();

    let part1 = evolve_stones(&stones, 25);
    print_day_result(&1, part1);

    let part2 = evolve_stones(&stones, 75);
    print_day_result(&2, part2);
}

fn evolve_stones(stones: &[i64], count: i32) -> i64 {
    let mut cache: HashMap<(i64, i32), i64> = HashMap::new();
    let mut result: i64 = 0;
    for stone in stones {
        result += evolve_stone(*stone, count, &mut cache);
    }
    result
}

fn evolve_stone(stone: i64, count: i32, cache: &mut HashMap<(i64, i32), i64>) -> i64 {
    if count == 1 {
        return if stone.to_string().len() % 2 == 0 { 2 } else { 1 };
    } else if cache.contains_key(&(stone, count)) {
        return *cache.get(&(stone, count)).unwrap();
    }

    let result: i64;
    if stone.to_string().len() % 2 == 0 {
        let s = stone.to_string();
        let s1 = s[..s.len()/2].parse::<i64>().unwrap();
        let s2 = s[s.len()/2..].parse::<i64>().unwrap();

        let r1 = evolve_stone(s1, count - 1, cache);
        cache.insert((s1, count - 1), r1);
        let r2 = evolve_stone(s2, count - 1, cache);
        cache.insert((s2, count - 1), r2);
        result = r1 + r2;
    } else {
        let st = if stone == 0 { 1 } else { stone * 2024 };
        result = evolve_stone(st, count - 1, cache);
        cache.insert((st, count - 1), result);
    }

    result
}
