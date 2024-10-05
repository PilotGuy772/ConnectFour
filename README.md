# ConnectFour

You all know the classic board (kind of?) game, right?
[Connect Four](https://en.wikipedia.org/wiki/Connect_Four) is a two-player zero-sum game published by Hasbro Gaming. In Connect Four, players drop colored tokens into a 7x6 board; each token falls to the bottom of its respective column. As suggested by the name, the object of the game is to make a line of four tokens either vertically, horizontally, or diagonally. Connect Four is considered a [solved game](https://en.wikipedia.org/wiki/Connect_Four#Mathematical_solution).

In this program, I am working on my best attempt at a heuristics-based solving algorithm (AI?) to play the game.

---

### How it works

My connect four AI works on a [minimax](https://en.wikipedia.org/wiki/Minimax) algorithm. In simple terms, the AI always acts based on what has the potential to give it the best score. Because Connect Four has so many possible board states and such a huge tree (for a tree of height n, the number of base nodes is represented by 7<sup>n</sup>), the algorithm scores board states based on a heuristic algorithm that recods blockades (two-in-a-rows), threats (three-in-a-rows), and attacks (opportunities for one side to win on the next move if something isn't done about it).

The algorithm essentially predicts what the other player will do based on what it thinks that the player thinks will benefit it most, i.e., give the player the best score. It's not perfect, but it's pretty much the best there is in the realm of game theory.

### Installation

I have provided in the [releases](https://github.com/PilotGuy772/ConnectFour/releases) page a compiled binary for Linux and Windows amd64 systems. If you would like to run the program on another system, you will have to compile it yourself. You can do this by installing .NET SDK version 7 (or newer) and copying the source code into a directory. Then, you can use the `dotnet run` or `dotnet publish` commands to run or publish the program. If you use an SDK version newer than 7, you will have to edit the `.csproj` file to target your installed SDK version. It probably won't cause issues for a program like this, but for guaranteed functionality you should use .NET SDK 7 (although it is deprecated now).

### Changelog, Future Plans, and Known Issues

```
|- v0.0.0 -> preliminary version; basic infrastructure only
|- v1.0.0 -> first working version
   |- v1.1.0 -> QOL changes, added a menu at startup
|- v2.0.0 -> algorithm now works on a minimax algorithm, bugfixes
|- v2.1.0 -> minimax now uses alpha-beta pruning
```
Goals for the future:
* <s>Add a menu for changin environment variables at startup</s>
* <s>Add a proper minimax algorithm</s>
* <s>Add alpha-beta pruning</s>
* Optimize like crazy to allow for deeper tree searches
* Implement multithreading

Known Issues:
* The AI often ignores opportunities to form a win by principle, i.e., two attacks at the same time (more of a balance issue than anything).
* The AI sometimes does really wacky stuff. Again, more of a balance issue than anything.

The spagetti code situation is better now (I hope).
