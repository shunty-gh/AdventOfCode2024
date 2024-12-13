# Advent Of Code 2024

![AoC Mug](./mugshot.png)

Some solutions for the [Advent of Code 2024](https://adventofcode.com/2024) puzzles. Variously attempted in [C#](./cs), [Python](./py) and [Rust](./rs). See the relevant directories for further details.


### Input data

All the solutions expect to find the relevant puzzle input in a `./input` subdirectory of this directory. The input files should be named in the form `dayNN-input` eg `day07-input`, `day23-input`.

### Input data fetch utility

There is a little utility program in the `./fetch` directory that can get the input data specific to you for a given day. Run it without parameters for further instructions. You will need either a `.env` file or an `appsettings.private.json` file, in the same directory, containing a vaild session cookie which you can get from the developer tools console of your web browser once you have authenticated with the AoC site.

`.env` file format
```
sessionCookie=435684c7465645f5fb2481103e5....
```

`appsettings.private.json` file format
```
{
  "sessionCookie": "435684c7465645f5fb2481103e5...."
}
```