# alpha_beta_pruning#
In the context of Artificial intelligence course the
task was to explore different techniques of searches of adversarial
game, I have developed code to simulate the game of tic-tac-toe
to explore the effectiveness of mini-max and alpha-beta pruning.

I decided to implement tic-tac-toe as I know how to play this
game. This game is a perfect example for a search methods
and adversary. The chess game is more interesting with a
sophisticated evaluation function, but the game difficulty is
very high. Although tic-tac-toe is itself quit simple, but the
concept of programming is not. The computer must recognize
the moves of the player and act accordingly to player’s strategy
using a MiniMax algorithm.
For the search methods the game tree is used. Each branch
of the tree indicates how one board configuration can be
transformed into another by a single move. I used depth-first
search for finding the best solution.
For the ranking of game outcomes I used several utility
functions. In this task the problem difficulty is low enough to
implement minimax algorithm. Consider noughts and crosses
with two players called MAX and MIN. The first player makes
a move to maximize the utility function, the second player
moves and minimize utility function. First task in this problem
is to calculate which move is maximizing utility and which is
minimizing.
I also implemented the Alpha-beta pruning algorithm.
Alpha-beta pruning is more effective algorithm compared to
minimax. It searches only the branches of a tree that are
important, so the program complexity becomes twice easier.
Then I compared the results of the algorithms.
