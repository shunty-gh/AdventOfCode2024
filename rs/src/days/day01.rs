use counter::Counter;
use crate::aocutils::*;

pub fn run() {
    let day = 1;
    let filename = find_input_file(&day).unwrap();

    let mut left: Vec<i32> = Vec::new();
    let mut right: Vec<i32> = Vec::new();

    match get_lines_from_file(&filename) {
        Ok(lines) => {
            for (index, line) in lines.iter().enumerate() {
                let numbers: Result<Vec<i32>, _> = line
                .split_whitespace()
                .map(|part| part.parse::<i32>())
                .collect();

                match numbers {
                    Ok(numbers) => {
                        left.push(numbers[0]);
                        right.push(numbers[1]);
                    }
                    Err(e) => eprintln!("Error parsing line {}: {}", index + 1, e),
                }
            }
        }
        Err(e) => eprintln!("Error reading file: {}", e),
    }

    left.sort();
    right.sort();
    let part1: i32 = left.iter().zip(right.iter()).map(|(x,y)| (x - y).abs() ).sum();
    print_day_result(&1,part1);

    let right2 = right.iter().collect::<Counter<_>>();
    let mut part2 = 0;
    left.iter().for_each(|x| {
        if right2.contains_key(x) {
            let rc = *right2.get(x).unwrap();
            part2 += x * rc as i32;
        }
    });
    print_day_result(&2, part2);
}
