// This solution is awful. It takes a really long time and only works because
// I looked up a list of highly composite numbers and blindly assumed the
// target numbers would be divisible by 10
long Target = 36_000_000;
// Step 1
PrintLowest(Target, CheckHouse);
// Step 2
PrintLowest(Target, CheckHouseStep2);

void PrintLowest(long target, Func<long, long> HouseCheckFunc) {
  var current = 720720; // Highly composite number which gets the closest
  while (true) {
    var test = HouseCheckFunc(current);
    if (test >= target) break;
    current += 10;
  }
  Console.WriteLine(current);
}

long CheckHouse(long houseNumber) {
  return AllDivisors(houseNumber).Sum() * 10;
}

long CheckHouseStep2(long houseNumber) {
  return AllDivisorsUsedLessThan50Times(houseNumber).Sum() * 11;
}

IEnumerable<long> AllDivisors(long input) {
  yield return 1;
  for (var i = 2; i <= input / 2; i++) {
    if (input % i == 0)
      yield return i;
  }
  yield return input;
}

IEnumerable<long> AllDivisorsUsedLessThan50Times(long input) {
  for (var i = input / 50; i <= input / 2; i++) {
    if (input % i == 0)
      yield return i;
  }
  yield return input;
}