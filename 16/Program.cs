var runStep2 = false;
Dictionary<string, int> CompoundIndexMap = new() {
  { "children", 0 },
  { "cats", 1 },
  { "samoyeds", 2 },
  { "pomeranians", 3 },
  { "akitas", 4 },
  { "vizslas", 5 },
  { "goldfish", 6 },
  { "trees", 7 },
  { "cars", 8 },
  { "perfumes", 9 },
};
int[] ticker = [3, 7, 2, 3, 0, 0, 5, 3, 2, 1];
var aunts = ReadAunts("input.txt");
var matches = aunts.Select((a, i) => (AuntNumber: i+1, Match(ticker, a, runStep2)));
Console.WriteLine(matches.OrderByDescending(m => m.Item2).First().AuntNumber);

int Match(int[] ticker, int[] a, bool runStep2 = false)
{
  var sum = 0;
  for (int i = 0; i < ticker.Length; i++) {
    // Unknown value, don't check
    if (a[i] == -1) continue;

    // cats/trees rule
    if (runStep2 && (i == CompoundIndexMap["cats"] || i == CompoundIndexMap["trees"])) {
      if (a[i] > ticker[i]) sum += 1;
      else sum -= 100;
      continue;
    }

    // pomeranians/goldfish rule
    if (runStep2 && (i == CompoundIndexMap["pomeranians"] || i == CompoundIndexMap["goldfish"])) {
      if (a[i] < ticker[i]) sum += 1;
      else sum -= 100;
      continue;
    }

    if (ticker[i] == a[i])
      sum += 1;
    else {
      sum -= 100;
      break;
    }
  }
  return sum;
}

List<int[]> ReadAunts(string filename) {
  List<int[]> ret = [];
  foreach (var line in File.ReadAllLines(filename)) {
    var data = string.Join(":", line.Split(':')[1..]);
    var sets = data.Split(',').Select(s => (s.Split(':')[0].Trim(), int.Parse(s.Split(':')[1].Trim()))).ToList();
    int[] aunt = [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1];
    foreach (var s in sets) {
      aunt[CompoundIndexMap[s.Item1]] = s.Item2;
    }
    ret.Add(aunt);
  }
  return ret;
}