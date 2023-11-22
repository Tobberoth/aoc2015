using System.Text;

var word = "1113222113";
var timesToRepeat = 50;
for (var i = 0; i < timesToRepeat; i++)
  word = LookAndSay(word);
Console.WriteLine(word.Length);

static string LookAndSay(string input) {
  StringBuilder output = new();
  char currentChar = ' ';
  int count = 0;
  foreach (char c in input) {
    if (currentChar == ' ')
      currentChar = c;
    if (c != currentChar) {
      output.Append($"{count}{currentChar}");
      count = 0;
      currentChar = c;
    }
    count++;
  }
  output.Append($"{count}{currentChar}");
  return output.ToString();
}