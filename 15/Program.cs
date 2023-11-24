var Ingredients = ReadIngredients("input.txt").ToList();
Console.WriteLine(MaxCombination(Ingredients));
Console.WriteLine(MaxCombination(Ingredients, true));

long MaxCombination(List<Ingredient> ingredients, bool checkCalories = false) {
  long max = 0;
  for (var x = 0; x <= 100; x++)
    for (var y = 0; y <= 100; y++)
      for (var j = 0; j <= 100; j++)
        for (var k = 0; k <= 100; k++) {
          if (x+y+j+k > 100) continue;
          if (x+y+j+k < 100) continue;
          var sum = MultiplyIngredients(ingredients, x, y, j, k, checkCalories);
          if (sum > max)
            max = sum;
        }
  return max;
}

IEnumerable<Ingredient> ReadIngredients(string filename) {
  foreach (var line in File.ReadAllLines(filename)) {
    if (string.IsNullOrWhiteSpace(line)) continue;
    var data = line.Split(' ');
    yield return new Ingredient(
      data[0].TrimEnd(':'),
      int.Parse(data[2].TrimEnd(',')),
      int.Parse(data[4].TrimEnd(',')),
      int.Parse(data[6].TrimEnd(',')),
      int.Parse(data[8].TrimEnd(',')),
      int.Parse(data[10]));
  }
}

long MultiplyIngredients(List<Ingredient> ingredients, int x, int y, int j, int k, bool checkCalories = false) {
  if (checkCalories) {
    var cal = (ingredients[0].Calories * x) + (ingredients[1].Calories * y) + (ingredients[2].Calories * j) + (ingredients[3].Calories * k);
    if (cal != 500) return 0;
  }
  var cap = (ingredients[0].Capacity * x) + (ingredients[1].Capacity * y) + (ingredients[2].Capacity * j) + (ingredients[3].Capacity * k);
  if (cap < 1) return 0;
  var dur = (ingredients[0].Durability * x) + (ingredients[1].Durability * y) + (ingredients[2].Durability * j) + (ingredients[3].Durability * k);
  if (dur < 1) return 0;
  var fla = (ingredients[0].Flavor * x) + (ingredients[1].Flavor * y) + (ingredients[2].Flavor * j) + (ingredients[3].Flavor * k);
  if (fla < 1) return 0;
  var tex = (ingredients[0].Texture * x) + (ingredients[1].Texture * y) + (ingredients[2].Texture * j) + (ingredients[3].Texture * k);
  if (tex < 1) return 0;
  return cap * dur * fla * tex;
}

readonly record struct Ingredient(string Name, long Capacity, long Durability, long Flavor, long Texture, long Calories);