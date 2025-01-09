#![allow(dead_code)]
extern crate queues;

mod aocutils;
mod days;

use chrono::Datelike;
use days::*;

fn main() {
    let args: Vec<String> = std::env::args().collect();
    let days: Vec<u8>;
    if args.contains(&String::from("-h")) {
        show_help();
        std::process::exit(0);
    }
    if args.len() > 1 {
        days = args[1..].iter().map(|a| a.parse::<u8>().unwrap_or_else(|_| 0)).filter(|x| *x > 0).collect::<Vec<u8>>();
    } else {
        // Add todays date
        let today = chrono::Utc::now();
        if today.year() ==  2024 && today.month() == 12 && today.day() <= 25 {
            days = vec![today.day() as u8];
        } else {
            show_help();
            std::process::exit(0);
        }
    }

    run_days(&days);
}

fn show_help() {
    println!();
    println!("*** Advent of Code 2024 in Rust ***");
    println!();
    println!("Usage:");
    println!("  cargo run <day_num> <day_num>...");
    println!("  cargo run --release <day_num> <day_num>...");
    println!("  cargo run -- -h");
    println!();
    println!("eg:");
    println!("  cargo run 1 4 6");
    println!();
}

fn run_days(days: &[u8]) {

    println!();
    println!("*** Advent of Code 2024 in Rust ***");
    println!();

    for day in days {
        println!("Day {day}");
        match day {
            1 => {
                day01::run();
            },
            2 => {
                day02::run();
            },
            3 => {
                day03::run();
            },
            4 => {
                day04::run();
            },
            5 => {
                day05::run();
            },
            6 => {
                day06::run();
            },
            10 => {
                day10::run();
            },
            11 => {
                day11::run();
            },
            12 => {
                day12::run();
            },
            _ => {
                println!("  Error: Day {day} not available!")
            }
        }
        println!();
    }
    println!();
}
