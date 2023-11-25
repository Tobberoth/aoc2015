Dictionary<string, Item> Store = new() {
  { "Dagger", new Item("Dagger", 8, 4, 0, 1, 1) },
  { "Shortsword", new Item("Shortsword", 10, 5, 0, 1, 1) },
  { "Warhammer", new Item("Warhammer", 25, 6, 0, 1, 1) },
  { "Longsword", new Item("Longsword", 40, 7, 0, 1, 1) },
  { "Greataxe", new Item("Greataxe", 75, 8, 0, 1, 1) },
  { "Leather", new Item("Leather", 13, 0, 1, 0, 1) },
  { "Chainmail", new Item("Chainmail", 31, 0, 2, 0, 1) },
  { "Splintmail", new Item("Splintmail", 53, 0, 3, 0, 1) },
  { "Bandedmail", new Item("Bandedmail", 75, 0, 4, 0, 1) },
  { "Platemail", new Item("Platemail", 102, 0, 5, 0, 1) },
  { "Damage +1", new Item("Damage +1", 25, 1, 0, 0, 2) },
  { "Damage +2", new Item("Damage +2", 50, 2, 0, 0, 2) },
  { "Damage +3", new Item("Damage +3", 100, 3, 0, 0, 2) },
  { "Defense +1", new Item("Defense +1", 20, 0, 1, 0, 2) },
  { "Defense +2", new Item("Defense +2", 40, 0, 2, 0, 2) },
  { "Defense +3", new Item("Defense +3", 80, 0, 3, 0, 2) },
};

var Boss = new Combatant(109, 8, 2); // Change for your input

List<(int cost, Item[] items)> EqSet = GenerateAllItemEquipments()
  .Select(c => (c.Sum(i => i.Cost), c))
  .ToList();

var winningAmount = 0;
foreach (var eq in EqSet.OrderBy(eq => eq.cost)) {
  var p = new Combatant(100, eq.items.Sum(i => i.Damage), eq.items.Sum(i => i.Armor));
  if (CheckWin(p, Boss)) {
    winningAmount = eq.cost;
    break;
  }
}
Console.WriteLine(winningAmount);

var losingCost = 0;
foreach (var eq in EqSet.OrderByDescending(e => e.cost)) {
  var p = new Combatant(100, eq.items.Sum(i => i.Damage), eq.items.Sum(i => i.Armor));
  if (!CheckWin(p, Boss)) {
    losingCost = eq.cost;
    break;
  }
}
Console.WriteLine(losingCost);

HashSet<Item[]> GenerateAllItemEquipments() {
  var combos = GetAllArmorItemCombinations();
  HashSet<Item[]> initial = [..combos];
  foreach (var weapon in Store.Where(k => k.Value.MinAmount == 1)) {
    foreach (var i in initial) {
      combos.Add([weapon.Value, ..i]);
    }
  }
  combos.RemoveWhere(eq => !eq.Any(i => i.MinAmount == 1));
  return combos;
}

HashSet<Item[]> GetAllArmorItemCombinations() {
  var ringCombos = GetAllRingItemCombinations();
  HashSet<Item[]> noArmorCombos = [..ringCombos];
  foreach (var armor in Store.Where(k => k.Value.MinAmount == 0 && k.Value.MaxAmount == 1)) {
    foreach (var ac in noArmorCombos)
      ringCombos.Add([armor.Value, ..ac]);
  }
  return ringCombos;
}

HashSet<Item[]> GetAllRingItemCombinations() {
  HashSet<Item[]> ret = [];
  ret.Add([]); // No rings;
  foreach (var r in Store.Where(k => k.Value.MaxAmount == 2)) {
    ret.Add([ r.Value ]);
    foreach (var r2 in Store.Where(k => k.Value.MaxAmount == 2)) {
      if (r.Key != r2.Key) // Can't reuse ring
        ret.Add([ r.Value, r2.Value ]);
    }
  }
  return ret;
}

static bool CheckWin(Combatant a, Combatant b) {
  var aTurns = b.HP / Math.Clamp(a.Damage - b.Armor, 1, 100);
  var bTurns = a.HP / Math.Clamp(b.Damage - a.Armor, 1, 100);
  return aTurns <= bTurns;
}

record Combatant(int HP, int Damage, int Armor);

record Item(string Name, int Cost, int Damage, int Armor, int MinAmount, int MaxAmount);