use std::collections::HashSet;
use crate::aocutils::*;

// https://adventofcode.com/2024/day/6 - Guard Gallivant

// Part 2 runs very slowly in debug build (~50s) but gets down to about 4s in
// release build - on my machine :-)

pub fn run() {
    let day = 6;
    let filename = find_input_file(&day).unwrap();
    let input = get_lines_from_file(&filename).unwrap();
    let grid = input.iter().map(|s| s.chars().collect::<Vec<char>>()).collect::<Vec<Vec<char>>>();

    // Find start
    let (mut start_x, mut start_y) = (0, 0);
    for (y, v) in grid.iter().enumerate() {
        for (x, c) in v.iter().enumerate() {
            if *c == '^' {
                (start_x, start_y) = (x as i32, y as i32);
                break;
            }
        }
        if start_x > 0 {
            break;
        }
    }

    // Part 1
    let part1 = do_patrol(&grid, start_x, start_y, -1, -1);
    print_day_result(&1, part1);

    // Part 2 - try to patrol with an obstacle at each possible location
    let mut part2 = 0;
    for (y, v) in grid.iter().enumerate() {
        for (x, c) in v.iter().enumerate() {
            if *c == '.' && do_patrol(&grid, start_x, start_y, x as i32, y as i32) < 0 {
                part2 += 1;
            }
        }
    }
    print_day_result(&2, part2);
}

fn do_patrol(grid: &[Vec<char>], sx: i32, sy: i32, ox: i32, oy: i32) -> i32 {
    // sx,sy -> start position; cx,cy -> current position; ox,oy -> obstacle position
    // nx,ny -> next position; dx,dy -> direction
    let p2 = ox > 0;
    let xlen = grid[0].len() as i32;
    let ylen = grid.len() as i32;
    let (mut cx, mut cy) = (sx, sy);
    let (mut dx, mut dy) = (0, -1);
    let mut seen1: HashSet<(i32, i32)> = HashSet::new();
    let mut seen2: HashSet<(i32, i32, i32, i32)> = HashSet::new();
    seen1.insert((cx, cy));
    seen2.insert((cx, cy, dx, dy));

    loop {
        let (nx, ny) = (cx + dx, cy + dy);

        // Are we wandering out of the area (part 1)
        if nx < 0 || nx >= xlen || ny < 0 || ny >= ylen {
            return seen1.len() as i32;
        }

        // If we've been here before AND going in the same direction, then
        // we've found a loop (part 2)
        if p2 && seen2.contains(&(nx, ny, dx, dy)) {
            return -1;
        }

         // Have we hit an obstacle? Turn right.
         if grid[ny as usize][nx as usize] == '#' || (nx, ny) == (ox, oy) {
            (dx, dy) = (-dy, dx);
        } else {
            if !p2 { seen1.insert((nx, ny)); }
            if p2 { seen2.insert((nx, ny, dx, dy)); }
            (cx, cy) = (nx, ny);
        }
   }
}