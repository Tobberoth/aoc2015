const bool STEP2 = true;
const int GRIDSIZE = 100;
bool[][] Grid = ReadGrid("input.txt");
bool[][] NewGrid = ReadGrid("input.txt");
foreach (var second in Enumerable.Range(1, 100)) {
  foreach (var x in Enumerable.Range(0, GRIDSIZE))
    foreach (var y in Enumerable.Range(0, GRIDSIZE))
      UpdateNewGrid(Grid, x, y, NewGrid);
  (Grid, NewGrid) = (NewGrid, Grid);
}
Console.WriteLine(Grid.Sum(r => r.Sum(c => c ? 1 : 0)));

static bool[][] ReadGrid(string filename) {
  var lines = File.ReadAllLines(filename);
  var ret = new bool[lines.Length][];
  for (var y = 0; y < lines.Length; y++)
    ret[y] = lines[y].Select(c => c == '#').ToArray();
  return ret;
}

void UpdateNewGrid(bool[][] oldGrid, int x, int y, bool[][] newGrid) {
  var sum = 0;
  sum += CheckPos(oldGrid, x-1, y-1);
  sum += CheckPos(oldGrid, x, y-1);
  sum += CheckPos(oldGrid, x+1, y-1);
  sum += CheckPos(oldGrid, x-1, y);
  sum += CheckPos(oldGrid, x+1, y);
  sum += CheckPos(oldGrid, x-1, y+1);
  sum += CheckPos(oldGrid, x, y+1);
  sum += CheckPos(oldGrid, x+1, y+1);
  if (oldGrid[y][x]) {
    if (STEP2 && IsCorner(x, y))
      newGrid[y][x] = true;
    else
      newGrid[y][x] = sum == 2 || sum == 3;
  } else
    newGrid[y][x] = sum == 3;
}

bool IsCorner(int x, int y) {
  return (x == 0 && y == 0) || (x == 0 && y == GRIDSIZE-1) || (x == GRIDSIZE-1 && y == 0) || (x == GRIDSIZE-1 && y == GRIDSIZE-1);
}

int CheckPos(bool[][] oldGrid, int x, int y) {
  if (x < 0 || x > GRIDSIZE-1 || y < 0 || y > GRIDSIZE-1) return 0;
  if (STEP2 && IsCorner(x, y)) return 1;
  return oldGrid[y][x] ? 1 : 0;
}