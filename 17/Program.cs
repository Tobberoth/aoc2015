var containers = File.ReadAllLines("input.txt").Select(int.Parse).ToList();
Console.WriteLine(GetValidCombinations(150, containers));

static long GetValidCombinations(int target, List<int> bottles, int depth = 0) {
  if (target == 0) {
    // Remove the following conditional to get step 1, set num to least number of coins
    if (depth > 4)
      return 0;
    return 1; // Hit target
  }
  if (target < 0 || !bottles.Any()) return 0; // Missed target
  var first = bottles[0];
  var rest = bottles[1..];
  long sum = 0;
  sum += GetValidCombinations(target - first, rest, depth+1);
  sum += GetValidCombinations(target, rest, depth);
  return sum;
}