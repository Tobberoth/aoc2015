using System.Text;

var p1 = new Password("hepxcrrq");
p1.UpdateToNextValid();
Console.WriteLine(p1);
p1.UpdateToNextValid();
Console.WriteLine(p1);

class Password {
  private readonly char[] pass = [ ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' ];

  public Password(string initial) {
    var reversed = initial.Reverse().ToList();
    for (var i = 0; i < initial.Length; i++)
      pass[i] = reversed[i];
  }

  public void UpdateToNextValid() {
    Increase();
    while (!Validate())
      Increase();
  }

  public void Increase() {
    var i = 0;
    while (i < 8) {
      int c = pass[i] + 1;
      if (c > 122) {
        pass[i] = 'a';
        i++;
        continue;
      }
      pass[i] = (char)c;
      break;
    }
  }

  public bool Validate() {
    return Rule1() && Rule2() && Rule3();
  }

  private bool Rule1() {
    var count = 0;
    var prevC = 1000;
    foreach (int c in pass) {
      if ((char)c == ' ') break;
      if (c == prevC - 1)
        count++;
      else
        count = 0;
      if (count >= 2) return true;
      prevC = c;
    }
    return count >= 2;
  }

  private bool Rule2() {
    foreach (char c in pass)
      if (c == 'i' || c == 'o' || c == 'l') return false;
    return true;
  }

  private bool Rule3() {
    var prevC = '}';
    var foundPairCount = 0;
    for (var i = 0; i < pass.Length; i++) {
      if (pass[i] == ' ') break;
      if (pass[i] == prevC) {
        foundPairCount++;
        prevC = '}';
        continue;
      }
      prevC = pass[i];
    }
    return foundPairCount > 1;
  }

  public override string ToString() {
    StringBuilder sb = new();
    foreach (char c in pass.Reverse()) {
      if (c == ' ') continue;
      sb.Append(c);
    }
    return sb.ToString();
  }
}