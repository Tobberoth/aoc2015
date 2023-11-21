Console.WriteLine(File.ReadAllLines("input.txt").Sum(CalcSquareFeet));
Console.WriteLine(File.ReadAllLines("input.txt").Sum(CalcRibbonFeet));

int CalcSquareFeet(string input) {
  if (input.Split('x').Select(int.Parse).ToList() is [int l, int w, int h]) {
    var s1 = l * w;
    var s2 = w * h;
    var s3 = l * h;
    var minSide = Math.Min(s1, s2);
    minSide = Math.Min(minSide, s3);
    return (2 * s1) + (2 * s2) + (2 * s3) + minSide;
  }
  return 0;
}

int CalcRibbonFeet(string input) {
  if (input.Split('x').Select(int.Parse).Order().ToList() is List<int> sides) {
    var ribbon = sides[0] + sides[0] + sides[1] + sides[1]; 
    var bow = sides[0] * sides[1] * sides[2];
    return ribbon + bow;
  }
  return 0;
}