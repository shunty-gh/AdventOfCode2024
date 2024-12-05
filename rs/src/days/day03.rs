use regex::Regex;
use crate::aocutils::*;

pub fn run() {
    let day = 3;
    let filename = find_input_file(&day).unwrap();
    let input = get_text_from_file(&filename).unwrap();

    let pattern1 = r"mul\((\d+),(\d+)\)";
    let re1 = Regex::new(pattern1).expect("Bad regex");

    let caps: Vec<(i32, i32)> = re1.captures_iter(&input)
        .map(|c| {
            let (_, [x, y]) = c.extract();
            (x.parse::<i32>().unwrap(), y.parse::<i32>().unwrap())
        }).collect();

    let part1: i32 = caps.iter().map(|c| c.0 * c.1).sum();
    print_day_result(&1, part1);

    let pattern2 = r"mul\(\d+,\d+\)|do\(\)|don\'t\(\)";
    let re2 = Regex::new(pattern2).expect("Bad regex");
    let matches: Vec<&str> = re2.find_iter(&input).map(|m| m.as_str()).collect();

    let mut ok = true;
    let mut part2 = 0;
    for m in matches.iter() {
        if m == &"do()" {
            ok = true;
        } else if m == &"don't()" {
            ok = false;
        } else if ok {
            let caps = re1.captures(m);
            if let Some(c) = caps {
                let (_, [x, y]) = c.extract();
                part2 += x.parse::<i32>().unwrap() * y.parse::<i32>().unwrap();
            }
        }
    }
    print_day_result(&2, part2);
}
