use std::collections::HashSet;
use queues::*;
use crate::aocutils::*;


pub fn run() {
    let day = 10;
    let filename = find_input_file(&day).unwrap();
    let lines = get_lines_from_file(&filename).unwrap();
    let grid = lines.into_iter().map(|line| line.chars().collect::<Vec<char>>()).collect::<Vec<Vec<char>>>();

    let mut starts: HashSet<(usize, usize)> = HashSet::new();
    grid.iter().enumerate().for_each(|(y, row)| {
        row.iter().enumerate().for_each(|(x, ch)| {
            if *ch == '0' {
                starts.insert((x, y));
            };
        });
    });

    let (mut part1, mut part2) = (0, 0);
    starts.iter().for_each(|start| {
        find_trails(&grid, *start).map(|res| {
            part1 += res.0;
            part2 += res.1;
        });
    });
    // OR
    // for start in starts {
    //     let (a,b) = find_trails(&grid, start).unwrap();
    //     part1 += a;
    //     part2 += b;
    // }

    print_day_result(&1, part1);
    print_day_result(&2, part2);
}

fn find_trails(map: &[Vec<char>], start: (usize, usize)) -> Option<(usize, usize)> {
    let mut visited: HashSet<(usize, usize)> = HashSet::new();
    let mut routes = 0;
    let mut q: Queue<(usize, usize)> = queue![];
    q.add(start).unwrap();

    let ylen = map.len() as i32;
    let xlen = map[0].len() as i32;
    while q.size() > 0 {
        let (cx, cy) = q.remove().unwrap();
        let cv = map[cy][cx].to_digit(10).unwrap();

        for (dx, dy) in [(0, -1), (1, 0), (0, 1), (-1, 0)].iter() {
            let (nx, ny) = (cx as i32 + dx, cy as i32 + dy);
            if nx < 0 || nx >= xlen || ny < 0 || ny >= ylen || map[ny as usize][nx as usize] < '0' || map[ny as usize][nx as usize] > '9' {
                continue;
            }
            let (nx, ny) = (nx as usize, ny as usize);
            let nv = map[ny][nx].to_digit(10).unwrap();
            if nv == 9 && cv == 8 {
                visited.insert((nx, ny));
                routes += 1;
            } else if nv == cv + 1 {
                let _ = q.add((nx, ny));
            }
        }
    }

    return Some((visited.len(), routes as usize));
}
