use std::fmt::Display;
use std::fs;
use std::fs::File;
use std::io::{self, BufRead};
use std::path::Path;

const MAX_SEARCH_LEVEL: i32 = 6;

/// Find the input file for the given day number by searching in the current
/// directory and then in a `./input` sub-directory, if it exists. If not found
/// then repeat the search up the parent directory chain for a few levels.
///
/// Returns the full path and filename if found and `None` if not found.
pub fn find_input_file(day_no: &i32) -> Option<String> {
    let inname = format!("day{:02}-input", day_no);
    // Start from the current directory
    let mut dir = ".".to_string();
    let mut fname = format!("{}/{}", dir, inname);

    let mut level = 0;
    loop {
        if level > MAX_SEARCH_LEVEL {
            break None;
        }

        // look in dir
        if Path::new(&fname).exists() {
            break Some(fname);
        }

        // look in dir/input
        fname = format!("{}/input/{}", dir, inname);
        if Path::new(&fname).exists() {
            break Some(fname);
        }

        // go up to the parent directory
        dir = format!("{}/..", dir);
        level += 1;
    }
}

/// Read all text from a file into a `String`. Remove all CR/LF.
pub fn get_text_from_file(filename: &str) -> io::Result<String> {
    let content = fs::read_to_string(filename)?;
    let result = content.replace(['\n', '\r'], "");
    Ok(result)
}

/// Read all text from a file into a `Vec<String>`
pub fn get_lines_from_file(filename: &str) -> io::Result<Vec<String>> {
    let file = File::open(filename)?;
    let reader = io::BufReader::new(file);
    reader.lines().collect()
}

/// Read a file with lines of integers separated by whitespace into a `Vec<Vec<i32>>`.
///
/// Each line of the input file is expected to have the format 'nn nnn n nn'
pub fn get_vec_of_vec_of_int_from_file(filename: &str) -> io::Result<Vec<Vec<i32>>> {

    let mut result: Vec<Vec<i32>> = Vec::new();

    match get_lines_from_file(filename) {
        Ok(lines) => {
            for (index, line) in lines.iter().enumerate() {
                let numbers: Result<Vec<i32>, _> = line
                    .split_whitespace()
                    .map(|part| part.parse::<i32>())
                    .collect();

                match numbers {
                    Ok(numbers) => {
                        result.push(numbers);
                    }
                    Err(e) => eprintln!("Error parsing line {}: {}", index + 1, e),
                }
            }
        }
        Err(e) => eprintln!("Error reading file: {}", e),
    }

    Ok(result)
}

/// Print the part number (usually 1 or 2) along with the solution result
pub fn print_day_result<T: Display>(part: &i32, result: T) {
    println!("  Part {}: {}", part, result);
}

