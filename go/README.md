# Advent Of Code 2024

Some solutions for the [Advent of Code 2024](https://adventofcode.com/2024) puzzles in [Go](https://go.dev/).

> DISCLAIMER:
> I am a complete Go beginner

All of these Go solutions were written after the 25th December and after I had written them in C# (and possibly Python or Rust). Therefore they have the benefit of hindsight and the knowledge of how to actually solve the problem. They were written as a learning experience and, in theory, they might even improve on some of my original solutions. 

## Pre-requisites

* [Go installed](https://go.dev/dl)
* (Optional) [Visual Studio Code](https://code.visualstudio.com/)
* (Optional) Any other C# code editor, notepad, IDE, environment, cloud eg [GoLand](https://www.jetbrains.com/go/)

## Run

```
cd <path-to-this-go-directory>
go run .
```

or

```
cd <path-to-this-go-directory>
go run aoc2024.go
```

By default it will run the solutions for all days. To run one or more specific days just list them on the command line separated by spaces. It will only run days where the solution has been written, obviously. eg:

```
go run . 1 11 19
```

## Input data

The solutions all expect input data to be in the `../input` directory and named in the form `dayNN-input`.

### Run with test data

Some of the solutions have test data hard coded into the source. To run with test data enabled, where applicable, you can append `-t` to the command line/

eg:
```
go run . 15 -t
```

## Native Compilation

To build a compiled, platform specific, 'release' version (Go doesn't really have a release build) use the command:

```
go build
```
This will produce a native executable in the current directory.

Run this compiled exectuable with the same parameters as listed in the [Run](#run) section, above