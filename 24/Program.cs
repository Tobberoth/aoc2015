var Packages = File.ReadAllLines("input.txt")
  .Select(long.Parse)
  .OrderByDescending(i => i)
  .ToList();
long TargetSize = Packages.Sum() / 4; // Set to 3 for step 1
var allCombos = GetAllCombinationsHitting(TargetSize, Packages);
var minAmount = allCombos.Min(c => c.Count);
long minQE = long.MaxValue;
foreach (var combo in allCombos.Where(c => c.Count == minAmount))
  minQE = Math.Min(combo.Aggregate((i, j) => i * j), minQE);
Console.WriteLine(minQE);

List<List<long>>? GetAllCombinationsHitting(long targetSize, List<long> packages) {
  List<List<long>> ret = [];
  if (targetSize == 0)
    return [[]];
  if (targetSize < 0)
    return null;
  if (targetSize > 0 && packages.Count < 1) return null;
  var first = packages[0];
  var rest = packages[1..];
  var firstCombos = GetAllCombinationsHitting(targetSize - first, rest)?.ToList();
  if (firstCombos != null) {
    firstCombos = firstCombos.Select(a => a.Prepend(first).ToList()).ToList();
    ret.AddRange(firstCombos);
  }
  var restCombos = GetAllCombinationsHitting(targetSize, rest)?.ToList();
  if (restCombos != null) {
    restCombos = restCombos.ToList();
    ret.AddRange(restCombos);
  }
  return ret.Count == 0 ? null : ret;
}