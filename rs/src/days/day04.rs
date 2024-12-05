use crate::aocutils::*;

pub fn run() {
    let day = 4;
    let filename = find_input_file(&day).unwrap();
    let lines = get_lines_from_file(&filename).unwrap();
    let chars = lines.into_iter().map(|line| line.chars().collect::<Vec<char>>()).collect::<Vec<Vec<char>>>();

    let mut part1 = 0;
    let mut part2 = 0;
    for y in 0..chars.len() {
        for x in 0..chars[y].len() {
            let ch = chars[y][x];
            if ch == 'X' {
                part1 += find_mas(&x, &y, &chars);
            } else if ch == 'A' {
                part2 += find_x_mas(&x, &y, &chars);
            }
        }
    }
    print_day_result(&1, part1);
    print_day_result(&2, part2);
}

fn find_mas(x: &usize, y: &usize, grid: &[Vec<char>]) -> i32 {
    let mut result = 0;
    let dirs = vec![(0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1)];
    for (dx,dy) in dirs {
        let mut px = *x as i32 + dx;
        let mut py = *y as i32 + dy;
        if px >= 0 && px < grid[0].len() as i32 && py >= 0 && py < grid.len() as i32 && grid[py as usize][px as usize] == 'M' {
            px += dx;
            py += dy;
            if px >= 0 && px < grid[0].len() as i32 && py >= 0 && py < grid.len() as i32 && grid[py as usize][px as usize] == 'A' {
                px += dx;
                py += dy;
                if px >= 0 && px < grid[0].len() as i32 && py >= 0 && py < grid.len() as i32 && grid[py as usize][px as usize] == 'S' {
                    result += 1;
                }
            }
        }
    }
    result
}

fn char_at(x: &i32, y: &i32, grid: &[Vec<char>]) -> char {
    if *x < 0 || *x >= grid[0].len() as i32 || *y < 0 || *y >= grid.len() as i32 {
        return ' ';
    }
    grid[*y as usize][*x as usize]
}

fn find_x_mas(x: &usize, y: &usize, grid: &[Vec<char>]) -> i32 {
    let upl = char_at(&(*x as i32 - 1), &(*y as i32 - 1), grid);
    let upr = char_at(&(*x as i32 + 1), &(*y as i32 - 1), grid);
    let dnl = char_at(&(*x as i32 - 1), &(*y as i32 + 1), grid);
    let dnr = char_at(&(*x as i32 + 1), &(*y as i32 + 1), grid);

    if (upl == 'M' && dnr == 'S' || upl == 'S' && dnr == 'M')
    && (upr == 'M' && dnl == 'S' || upr == 'S' && dnl == 'M') {
        return 1;
    }
    0
}