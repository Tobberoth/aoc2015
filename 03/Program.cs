Step1();
Step2();

void Step1() {
  Dictionary<(int x, int y), int> Grid = [];
  (int x, int y) currentPosition = (0, 0);
  Grid.Add(currentPosition, 1);
  foreach (char c in File.ReadAllText("input.txt"))
    currentPosition = Move(Grid, currentPosition, c);
  Console.WriteLine(Grid.Keys.Distinct().Count());
}

void Step2() {
  Dictionary<(int x, int y), int> Grid = [];
  (int x, int y) santaPos = (0, 0);
  (int x, int y) roboPos = (0, 0);
  Grid.Add(santaPos, 2);
  var santaTurn = true;
  foreach (char c in File.ReadAllText("input.txt")) {
    if (santaTurn)
      santaPos = Move(Grid, santaPos, c);
    else
      roboPos = Move(Grid, roboPos, c);
    santaTurn = !santaTurn;
  }
  Console.WriteLine(Grid.Keys.Distinct().Count());
}

(int x, int y) Move(Dictionary<(int x, int y), int> grid, (int x, int y) currentPosition, char direction) {
  var newPosition = direction switch {
    '^' => (currentPosition.x, currentPosition.y + 1),
    'v' => (currentPosition.x, currentPosition.y - 1),
    '<' => (currentPosition.x - 1, currentPosition.y),
    _ => (currentPosition.x + 1, currentPosition.y),
  };
  if (grid.ContainsKey(newPosition))
    grid[newPosition]++;
  else
    grid.Add(newPosition, 1);
  return newPosition;
}