use std::collections::HashMap;
use std::collections::HashSet;
use queues::*;
use crate::aocutils::*;

#[derive(Eq, Hash, PartialEq)]
#[derive(Clone)]
#[derive(Copy)]
struct Point2 {
    x: usize,
    y: usize,
}
impl Point2 {
    fn new(x: usize, y: usize) -> Self {
        Self {
            x: x,
            y: y,
        }
    }
}

struct Plot {
    x: usize,
    y: usize,
    val: char,
    borders: i32,
    region: i32,
}

pub fn run() {
    let day = 12;
    let filename = find_input_file(&day).unwrap();
    let lines = get_lines_from_file(&filename).unwrap();
    let grid = lines.into_iter()
        .map(|line| line.chars().collect::<Vec<char>>())
        .collect::<Vec<Vec<char>>>();

    let mut regionid = 0;
    let mut plots: HashMap<Point2, Plot> = HashMap::new();
    let mut part1 = 0i64;
    let mut part2 = 0i64;
    for (y, row) in grid.iter().enumerate() {
        for (x, _) in row.iter().enumerate() {
            let key = Point2::new(x, y);
            if !plots.contains_key(&key) {
                let (p1, p2) = build_region(&key, &grid, &mut plots, regionid);
                part1 += p1;
                part2 += p2;
                regionid += 1;
            }
        }
    }

    print_day_result(&1, part1);
    print_day_result(&2, part2);
    println!("  Part 2 unfinished!");
}

fn build_region(start: &Point2, map: &[Vec<char>], plots: &mut HashMap<Point2, Plot>, regionid: i32) -> (i64, i64) {
    let mut q: Queue<Point2> = queue![*start];
    let mut region: HashSet<Point2> = HashSet::new();
    let mut allborders = 0;
    let mut allplots = 0;
    let mut allcorners = 0;
    while q.size() > 0 {
        let curr = q.remove().unwrap();
        if plots.contains_key(&curr) {
            continue;
        }

        let ch = map[curr.y][curr.x];
        let mut borders = 4;
        for (dx, dy) in [(0, -1), (1, 0), (0, 1), (-1, 0)].iter() {
            let (nx, ny) = (curr.x as i32 + dx, curr.y as i32 + dy);
            if nx < 0 || nx >= map[0].len() as i32 || ny < 0 || ny >= map.len() as i32 {
                continue;
            }
            let next = Point2::new(nx as usize, ny as usize);
            let nch = map[next.y][next.x];
            if ch == nch {
                borders -= 1;
                if !plots.contains_key(&next) {
                    q.add(next).unwrap();
                }
            }
        }

        plots.insert(curr, Plot { x: curr.x, y: curr.y, val: ch, borders: borders, region: regionid });
        region.insert(curr);

        allplots += 1;
        allborders += borders;
    }

    // Now find all the corners
    for plot in region.iter() {
        for (dx, dy) in [(0, -1), (0, 1)].iter() {
            let (nx, ny) = (plot.x as i32 + dx, plot.y as i32 + dy);
            if nx < 0 || ny < 0 {
                continue;
            }
            // ToDo - Finish this
        }
    }

    (allplots as i64 * allborders as i64, allplots as i64 * allcorners as i64)
}