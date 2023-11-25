// INIT
var lines = File.ReadAllLines("input.txt");
List<(string key, string value)> Replacements = lines
  .Where(l => l.Contains("=>"))
  .Select(l => (l.Split(" => ")[0], l.Split(" => ")[1]))
  .ToList();
var InputMolecule = lines.Last();

// STEP 1
HashSet<string> PossibleOutputs = GetAllPossibleOutputs(InputMolecule, Replacements);
Console.WriteLine(PossibleOutputs.Count);

HashSet<string> GetAllPossibleOutputs(string inputMolecule, List<(string key, string value)> replacements) {
  HashSet<string> PossibleOutputs = [];
  foreach (var (key, value) in replacements) {
    var index = 0;
    while (true) {
      var prevIndex = index;
      index = inputMolecule[index..].IndexOf(key);
      if (index == -1) break;
      index += prevIndex;
      var newMolecule = inputMolecule.Remove(index, key.Length);
      PossibleOutputs.Add(newMolecule.Insert(index, value));
      index++;
    }
  }
  return PossibleOutputs;
}

// STEP 2
// Should try going backwards and go from biggest replacement to smallest
// If this hits, it's guaranteed to be shortest.
// Feels a bit hacky since it could probably fail to find "e" based on input.
Console.WriteLine(SolveStep2(InputMolecule));

int SolveStep2(string fromMolecule) {
  var OrderedRepls = Replacements.OrderByDescending(k => k.value.Length).ToList();
  var currMol = fromMolecule;
  var count = 0;
  while (currMol != "e") {
    currMol = ApplyFirstPossible(currMol, OrderedRepls);
    count++;
  }
  return count;
}

string ApplyFirstPossible(string fromMolecule, List<(string key, string value)> orderedRepls) {
  foreach (var (key, value) in orderedRepls) {
    var index = fromMolecule.IndexOf(value);   
    if (index == -1) continue;
    var toMolecule = fromMolecule.Remove(index, value.Length);
    toMolecule = toMolecule.Insert(index, key);
    return toMolecule;
  }
  return "";
}