bool RUNSTEP2 = false;
Player player = new(50, 500);
Boss boss = new(71, 10);
HashSet<Spell> Spells = [
  new MagicMissileSpell(),
  new DrainSpell(),
  new ShieldSpell(),
  new PoisonSpell(),
  new RechargeSpell()];
Dictionary<State, int> StateMemo = [];

Console.WriteLine(CalculateLeastManaFromState(new State(player, boss, [])));

int CalculateLeastManaFromState(State state) {
  if (StateMemo.ContainsKey(state)) return StateMemo[state];
  var initState = new State(state.P, state.B, state.EffectsInPlay);

  if (RUNSTEP2)
    state = new State(new Player(state.P.HP - 1, state.P.Mana, state.P.Armor), state.B, state.EffectsInPlay);

  // Is B win? return very high manaUse to make it unviable
  if (state.P.HP < 1)
    return 1_000_000;
  // If P win? return 0;
  if (state.B.HP < 1)
    return 0;

  // Do player turn
  foreach (var effect in state.EffectsInPlay.ToList())
    state = ApplyEffect(state, effect);

  // Did effects kill boss during player turn?
  if (state.B.HP < 1)
    return 0;

  // Else try each possible spell and then run again recursively from new state
  // of all combinations, return lowest mana hit

  var results = 1_000_000;
  foreach (var spell in Spells.Where(s => s.ManaCost <= state.P.Mana)) {
    // Pick spell
    // Can't cast spell with effect if effect already there
    if (state.EffectsInPlay.Any(e => e.Origin == spell.GetType())) continue;
    var newState = spell.UpdateState(state);

    // Do boss turn
    foreach (var effect in newState.EffectsInPlay.ToList())
      newState = ApplyEffect(newState, effect);

    // If P win? return 0;
    if (newState.B.HP < 1)
      return spell.ManaCost;
    // Is B win? return very high manaUse to make it unviable
    if (newState.P.HP < 1)
      return 1_000_000;

    var damageTaken = newState.B.Attack - newState.P.Armor;
    newState = new State(
      new Player(newState.P.HP - damageTaken, newState.P.Mana, newState.P.Armor),
      newState.B,
      newState.EffectsInPlay);
    results = Math.Min(spell.ManaCost + CalculateLeastManaFromState(newState), results);
  }
  StateMemo.Add(initState, results);
  return results;
}

State ApplyEffect(State stateIn, Effect effect) {
  Effect[] newEffectsInPlay = [..stateIn.EffectsInPlay.Where(e => e.Origin != effect.Origin)];
  if (effect.TimeLeft > 1)
    newEffectsInPlay = [
      ..newEffectsInPlay,
    new Effect(effect.TimeLeft-1, effect.GiveArmor, effect.DoDamage, effect.GiveMana, effect.Origin)];
  var newArmor = stateIn.P.Armor;
  if (effect.TimeLeft > 1 && effect.GiveArmor > 0)
    newArmor = effect.GiveArmor;
  if (effect.TimeLeft < 1 && effect.GiveArmor > 0)
    newArmor = 0;
  return new State(
    new Player(
      stateIn.P.HP,
      stateIn.P.Mana + effect.GiveMana,
      newArmor),
    new Boss(stateIn.B.HP - effect.DoDamage, stateIn.B.Attack),
    newEffectsInPlay
  );
}

record Player(int HP, int Mana, int Armor = 0);
record Boss(int HP, int Attack);
record Effect(int TimeLeft, int GiveArmor, int DoDamage, int GiveMana, Type Origin);
record State(Player P, Boss B, Effect[] EffectsInPlay) {
  public virtual bool Equals(State other) {
    return P == other.P && B == other.B && EffectsInPlay.SequenceEqual(other.EffectsInPlay);
  }
  public override int GetHashCode() {
    var hashCode = new HashCode();
    hashCode.Add(P);
    hashCode.Add(B);
    foreach (var effect in EffectsInPlay)
      hashCode.Add(effect);
    return hashCode.ToHashCode();
  }
};

abstract class Spell(int manaCost) {
  public int ManaCost { get; set; } = manaCost;
  public abstract State UpdateState(State state);
}
class MagicMissileSpell() : Spell(53) {
  public override State UpdateState(State state) {
    return new(
      new Player(state.P.HP, state.P.Mana - ManaCost),
      new Boss(state.B.HP - 4, state.B.Attack),
      state.EffectsInPlay);
  }
}
class DrainSpell() : Spell(73) {
  public override State UpdateState(State state) {
    return new(
      new Player(state.P.HP + 2, state.P.Mana - ManaCost),
      new Boss(state.B.HP - 2, state.B.Attack),
      state.EffectsInPlay);
  }
}
class ShieldSpell() : Spell(113) {
  public override State UpdateState(State state) {
    return new(
      new Player(state.P.HP, state.P.Mana - ManaCost),
      new Boss(state.B.HP, state.B.Attack),
      [..state.EffectsInPlay, new Effect(6, 7, 0, 0, typeof(ShieldSpell))]);
  }
}
class PoisonSpell() : Spell(173) {
  public override State UpdateState(State state) {
    return new(
      new Player(state.P.HP, state.P.Mana - ManaCost),
      new Boss(state.B.HP, state.B.Attack),
      [..state.EffectsInPlay, new Effect(6, 0, 3, 0, typeof(PoisonSpell))]);
  }
}
class RechargeSpell() : Spell(229) {
  public override State UpdateState(State state) {
    return new(
      new Player(state.P.HP, state.P.Mana - ManaCost),
      new Boss(state.B.HP, state.B.Attack),
      [..state.EffectsInPlay, new Effect(5, 0, 0, 101, typeof(RechargeSpell))]);
  }
}