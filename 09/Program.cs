HashSet<string> Locations = GetAllLocations("input.txt");
Dictionary<string, Dictionary<string, int>> Graph = GenerateDistanceGraph("input.txt");
var perms = GenerateAllPermutations(Locations);
(var minCount, var maxCount) = CalculateMinMaxPath(Graph, perms);
Console.WriteLine(minCount);
Console.WriteLine(maxCount);

HashSet<string> GetAllLocations(string inputfile) {
  HashSet<string> ret = [];
  foreach (var line in File.ReadAllLines(inputfile)) {
    var from = line.Split('=')[0].Split("to")[0].Trim();
    var to = line.Split('=')[0].Split("to")[1].Trim();
    ret.Add(from);
    ret.Add(to);
  }
  return ret;
}

Dictionary<string, Dictionary<string, int>> GenerateDistanceGraph(string inputfile) {
  Dictionary<string, Dictionary<string, int>> ret = [];
  foreach (var line in File.ReadAllLines(inputfile)) {
    var distance = int.Parse(line.Split('=')[1].Trim());
    var from = line.Split('=')[0].Split("to")[0].Trim();
    var to = line.Split('=')[0].Split("to")[1].Trim();
    if (!ret.ContainsKey(from))
      ret.Add(from, []);
    if (!ret.ContainsKey(to))
      ret.Add(to, []);
    ret[from].Add(to, distance);
    ret[to].Add(from, distance);
  }
  return ret;
}

List<string[]> GenerateAllPermutations(IEnumerable<string> locs) {
  List<string[]> ret = [];
  if (locs.Count() == 1) return [ locs.ToArray() ];
  foreach (var loc in locs) {
    var perms = GenerateAllPermutations(locs.Where(l => l != loc)).Select(a => a.Prepend(loc).ToArray()).ToList();
    ret.AddRange(perms);
  }
  return ret;
}

(int minCount, int maxCount) CalculateMinMaxPath(Dictionary<string, Dictionary<string, int>> graph, List<string[]> perms) {
  var minCount = 1000000;
  var maxCount = 0;
  foreach (var perm in perms) {
    var count = 0;
    string prevLoc = "";
    foreach (var loc in perm) {
      if (prevLoc == "")
        prevLoc = loc;
      else {
        count += graph[prevLoc][loc];
        prevLoc = loc;
      }
    }
    if (count < minCount)
      minCount = count;
    if (count > maxCount)
      maxCount = count;
  }
  return (minCount, maxCount);
}