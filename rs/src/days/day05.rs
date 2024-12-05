use std::collections::HashMap;
use crate::aocutils::*;

pub fn run() {
    let day = 5;
    let filename = find_input_file(&day).unwrap();
    let input = get_raw_text_from_file(&filename).unwrap();
    let (rulelines, runlines) = input.split_once("\n\n").unwrap();

    let mut rules: HashMap<i32, Vec<i32>> = HashMap::new();
    for rule in rulelines.lines() {
        let (rx, ry) = rule.split_once("|").unwrap();
        let (x, y) = (rx.parse::<i32>().unwrap(), ry.parse::<i32>().unwrap());

        rules.entry(x).or_default().push(y);
        rules.entry(y).or_default();
    }

    let (mut part1, mut part2) = (0, 0);
    for line in runlines.lines() {
        let nums: Vec<i32> = line.split(",").map(|s| s.parse::<i32>().unwrap()).collect();
        let ok = nums.is_sorted_by(|a,b| rules[a].contains(b));

        let mid = (nums.len() - 1) / 2;
        if ok {
            part1 += nums[mid];
        } else {
            let mut fixed = nums.clone();
            fixed.sort_unstable_by(|a,b| rules[a].contains(b).cmp(&true));
            part2 += fixed[mid];
        }
    }

    print_day_result(&1, part1);
    print_day_result(&2, part2);
}