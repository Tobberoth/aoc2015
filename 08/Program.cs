using System.Text;

var lines = File.ReadAllLines("input.txt");
var codeCount = 0;
var valueCount = 0;
foreach (var line in lines) {
  var ret = AnalyzeLine(line);
  codeCount += ret.codeCount;
  valueCount += ret.valueCount;
}
Console.WriteLine(codeCount - valueCount);

var encodedLines = File.ReadAllLines("input.txt").Select(EncodeLine).Sum(s => s.Length);
Console.WriteLine(encodedLines - codeCount);

(int codeCount, int valueCount) AnalyzeLine(string line) {
  var escapeMode = false;
  var codeCount = 0;
  var valueCount = 0;
  var unicode = "\\";
  var bytes = Encoding.ASCII.GetBytes(line);
  foreach (var byteData in bytes) {
    if (byteData == 10 || byteData == 13) continue;
    codeCount++;
    char symbol = (char)byteData;
    if (!escapeMode && symbol == '"')
      continue;
    if (!escapeMode && symbol == '\\') {
      escapeMode = true;
      continue;
    }
    if (escapeMode) {
      if (symbol == '\\' || symbol == '"') {
        valueCount++;
        escapeMode = false;
        continue;
      }
      if (unicode.Length == 3) {
        escapeMode = false;
        valueCount++;
        unicode = "\\";
        continue;
      }
      unicode += symbol;
      continue;
    }
    valueCount++;
  }
  return (codeCount, valueCount);
}

string EncodeLine(string line) {
  StringBuilder ret = new();
  var bytes = Encoding.ASCII.GetBytes(line);
  foreach (var byteData in bytes) {
    char symbol = (char)byteData;
    if (symbol == '"' || symbol == '\\')
      ret.Append('\\');
    ret.Append(symbol);
  }
  return "\"" + ret.ToString() + "\"";
}