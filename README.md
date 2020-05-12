# Game Of Life

## Rant
This is my take on Conway`s Game Of Life, written in C# - language ugly and weirdly pleasing at the same time. Please mind, that I am completely new to C#, thus I would except this implementation to be broken in many ways.


## How to run
Having following CLI behavior 
```bash
$ game_of_life --help
game_of_life 1.0.0
Copyright (C) 2020 game_of_life
  -w, --width     Required. Width of the GOL universe.
  -h, --height    Required. Height of the GOL universe.
  -i, --init      Required. Number of initially alive cells.
  --help          Display this help screen.
  --version       Display version information.
```
I guess this would be a good starting point
```bash
$ game_of_life -w 20 -h 20 -i 100
```
