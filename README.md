# 3DChess
Unity game to play chess in 3 dimensions on an 8x8x8 board; planar moves, advancement squares idiom.

If the 2D board is a square of squares (8x8), then the 3D board should be a cube of cubes (8x8x8).
However, simply extending the linear moves of 2D chess fails to reproduce the feel of chess. 
Thus this variation introduces planar moves, the pieces move in flat planes.
The rook moves in 3 orthogonal planes; horizontal, leftVertical and rightVertical.
This divides the board into 8 quadrants (octants?) so a rook and a king can defeat a lone king with exactly the same algorithm.
The bishop moves in 4 diagonal planes, and as in 2D chess can only reach half the squares on the board.
The new piece, the duke, moves in 6 slant planes, and can only reach one quarter of the squares on the board.

Patented in 1991 - 5,193,813.

Objectives of this project:
1) Provide an effective platform in which to play the game against another human opponent.
2) Exemplifies a challenging paradigm shift (planar moves), i.e. useful padagogy for how to think out of the box.
3) Example of quantum precursors; for instance pieces loose the concept of having a trajectory, wave particle duality, etc.
4) Provide a basis for another AI challenge.
5) Demonstrate superior user ergonomic principles.
6) Example of modern software engineering, such as design patterns.

TODO: Rules, architecture, coding, etc.
