var count = 0;
foreach (var word in File.ReadAllLines("input.txt"))
  if (ValidateWord1(word)) count++;
Console.WriteLine(count);
count = 0;
foreach (var word in File.ReadAllLines("input.txt"))
  if (ValidateWord2(word)) count++;
Console.WriteLine(count);

bool ValidateWord1(string word) {
  var vv = ValidateVowels(word);
  var vd = ValidateDoubles(word);
  var vbc = ValidateBadCombination(word);
  return vv && vd && vbc;
}

bool ValidateWord2(string word) {
  var vdd = ValidateDoubleDoubles(word);
  var vols = ValidateOneLetterSkipRepeat(word);
  return vdd && vols;
}

bool ValidateVowels(string word) {
  char[] vowels = ['a', 'i', 'u', 'e', 'o'];
  return word.Count(c => vowels.Contains(c)) > 2;
}

bool ValidateDoubles(string word) {
  foreach (var bracket in WordSegments(word, 2)) {
    if (bracket[0] == bracket[1])
      return true;
  }
  return false;
}

bool ValidateBadCombination(string word) {
  string[] badCombinations = ["ab", "cd", "pq", "xy"];
  foreach (var bracket in WordSegments(word, 2)) {
    if (badCombinations.Contains(bracket))
      return false;
  }
  return true;
}

bool ValidateDoubleDoubles(string word) {
  Dictionary<string, (int, int)> doubles = [];
  for (var i = 0; i <= word.Length - 2; i++) {
    var bracket = word[i..(i+2)];
    if (doubles.TryGetValue(bracket, out (int, int) value) && value.Item2 != i)
      return true;
    if (!doubles.ContainsKey(bracket))
      doubles.Add(bracket, (i, i+1));
  }
  return false;
}

bool ValidateOneLetterSkipRepeat(string word) {
  foreach (var bracket in WordSegments(word, 3)) {
    if (bracket[0] == bracket[2])
      return true;
  }
  return false;
}

IEnumerable<string> WordSegments(string word, int segmentSize) {
  for (var i = 0; i <= word.Length - segmentSize; i++) {
    yield return word[i..(i+segmentSize)];
  }
}