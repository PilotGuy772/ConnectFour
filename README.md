# ConnectFour

You all know the cult classic board game, right?
[Connect Four](https://en.wikipedia.org/wiki/Connect_Four) is a two-player zero-sum game published by Hasbro Gaming. In Connect Four, players drop colored tokens into a 7x6 board; each token falls to the bottom of its respective column. As suggested by the name, the object of the game is to make a line of four tokens either vertically, horizontally, or diagonally. Connect Four is considered a [solved game](https://en.wikipedia.org/wiki/Connect_Four#Mathematical_solution).

In this program, I am working on my best attempt at a heuristics-based solving algorithm / AI to play the game.

---

### How it works

My Connect Four AI is still at an early stage of development, so natuarally it is not the best it can be. Right now, the system works based on a recursive algorithm that explores a tree to a set depth and scores board states, then returns the average scores up to the root of the tree so the AI can choose based on the scores what is the best move. In any given board, the AI has up to seven choices available to it, one for each column. In its current state the AI will always act in self preservation if it finds it is under attack (about to lose if it doesn't do something about it immediately) and also assumes that the player it plays against will do the same. 

I plan to implement a proper [minimax](https://en.wikipedia.org/wiki/Minimax) algorithm in the next version.

### Installation

I have provided in the [releases](https://github.com/PilotGuy772/ConnectFour/releases) page a compiled binary for Windows-64 bit and X86-64 Linux systems. If you would like to run the program on another system, you will have to compile it yourself. You can do this by installing DotNet SDK version 7 and copying the source code into a directory. Then, you can use the `dotnet run` or `dotnet publish` commands to run or publish the program.

### Changelog, Future Plans, and Known Issues

```
|- v0.0.0 -> preliminary version; basic infrastructure only
|- v1.0.0 -> first working version
```
Goals for the future:
* Add a proper minimax algorithm
* Add a menu to choose tree depth and other choices
* Optimize like crazy to allow for deeper tree searches
* Implement multithreading

Known Issues:
* AI sometimes places two tokens in one turn
* AI sometimes ignores player attacks that are entirely on y = 0
* AI sometimes spits out four extra tokens on the board after the player wins
* Sometimes there's a ListIndexOutOfRangeException ???

I made most of this in one sitting don't mind the spagetti code, it will get better I promise.
