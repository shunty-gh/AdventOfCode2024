use regex::Regex;
use crate::aocutils::*;

pub fn run() {
    let day = 3;
    let filename = find_input_file(&day).unwrap();
    let input = get_text_from_file(&filename).unwrap();

    let pattern1 = r"mul\((\d+),(\d+)\)";
    let re = Regex::new(pattern1).expect("Bad regex");

    let caps: Vec<(&str, &str)> = re.captures_iter(&input)
        .map(|c| {
            let (_, [x, y]) = c.extract();
            (x, y)
        }).collect();

    let part1: i32 = caps.iter().map(|c| c.0.parse::<i32>().unwrap() * c.1.parse::<i32>().unwrap()).sum();
    print_day_result(&1, &part1);
}
