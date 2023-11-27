var Target = (2978, 3083);
long start = 20151125;
long current = start;
foreach (var i in Enumerable.Range(1, CoordToPosition(Target))) {
  long newNum = current * 252533;
  newNum %= 33554393;
  current = newNum;
}
Console.WriteLine(current);

static int CoordToPosition((int row, int col) coord) {
  var realRow = coord.row + coord.col - 2;
  var ans = (realRow / 2f * (1 + realRow)) + coord.col;
  return (int)Math.Floor(ans);
}