use crate::aocutils::*;

pub fn run() {
    let day = 2;
    let filename = find_input_file(&day).unwrap();
    let nums: Vec<Vec<i32>> = get_vec_of_vec_of_int_from_file(&filename).unwrap();

    let mut part1 = 0;
    let mut part2 = 0;
    nums.iter().for_each(|row| {
        if test_row(&row) {
            part1 += 1;
            part2 += 1;
        } else {
            let len = row.len();
            for i in 0..len {
                let r = [&row[..i], &row[i+1..]].concat();
                if test_row(&r) {
                    part2 += 1;
                    break;
                }
            }
        }
    });
    print_day_result(&1, &part1);
    print_day_result(&2, &part2);

}

fn test_row(row: &Vec<i32>) -> bool {
    let inc = row.iter().zip(row.iter().skip(1))
        .all(|(x,y)| y > x && y - x <= 3);
    let dec = row.iter().zip(row.iter().skip(1))
        .all(|(x,y)| x > y && x - y <= 3);
    return inc || dec;
}
