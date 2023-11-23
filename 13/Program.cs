bool DoStep2 = true;
HashSet<string> People = File.ReadAllLines("input.txt").Select(n => n.Split(' ').First()).ToHashSet();
if (DoStep2) People.Add("Me");
List<IEnumerable<string>> Permutations = GenerateAllPermutations(People);
Dictionary<(string, string), int> happyMap = GenerateHappyMap("input.txt");
Console.WriteLine(Permutations.Max(CalculateSitting));

List<IEnumerable<string>> GenerateAllPermutations(IEnumerable<string> people) {
  if (people.Count() == 1) return [ [ people.First() ] ];
  List<IEnumerable<string>> ret = [];
  foreach (var p in people) {
    ret.AddRange(GenerateAllPermutations(people.Where(person => person != p)).Select(l => l.Prepend(p)));
  }
  return ret;
}

Dictionary<(string, string), int> GenerateHappyMap(string filename) {
  Dictionary<(string, string), int> ret = [];
  foreach (var line in File.ReadAllLines(filename)) {
    var lineData = line.TrimEnd('.').Split(' ');
    ret.Add((lineData[0], lineData[10]), int.Parse($"{(lineData[2] == "lose" ? "-" : "+")}{lineData[3]}"));
  }
  if (DoStep2) {
    foreach (var p in People) {
      if (p == "Me") continue;
      ret.Add(("Me", p), 0);
      ret.Add((p, "Me"), 0);
    }
  }
  return ret;
}

int CalculateSitting(IEnumerable<string> permutation) {
  var sum = 0;
  var first = "";
  var previous = "";
  var current = "";
  foreach (var p in permutation) {
    if (first == "") {
      first = p;
      previous = p;
      continue;
    }
    current = p;
    sum += happyMap[(previous, current)];
    sum += happyMap[(current, previous)];
    previous = current;
  }
  sum += happyMap[(previous, first)];
  sum += happyMap[(first, previous)];
  return sum;
}