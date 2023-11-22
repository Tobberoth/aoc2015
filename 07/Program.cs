var Sources = File.ReadAllLines("input.txt").Select(l => new Source(l));
var bOverride = 0; // Set to answer of step 1 to get step 2 answer
Dictionary<string, int> Answers = [];
Console.WriteLine(ReadWireSignal("a"));

int ReadWireSignal(string wire) {
  if (wire == "b" && bOverride != 0) return bOverride;
  if (Answers.ContainsKey(wire)) return Answers[wire];
  // Find instruction pointing to this wire, solve
  var prev = Sources.First(s => s.TargetWire == wire);
  var val = 0;
  switch (prev.Operation) {
    case OperationType.STATIC:
      if (prev.LeftValue.HasValue) return prev.LeftValue.Value;
      val = ReadWireSignal(prev.WireLeft);
      break;
    case OperationType.NOT:
      if (prev.LeftValue.HasValue) return ~prev.LeftValue.Value;
      val = ~ReadWireSignal(prev.WireLeft);
      break;
    case OperationType.AND:
      int leftAndValue = prev.LeftValue ?? ReadWireSignal(prev.WireLeft);
      int rightAndValue = prev.RightValue ?? ReadWireSignal(prev.WireRight);
      val = leftAndValue & rightAndValue;
      break;
    case OperationType.OR:
      int leftOrValue = prev.LeftValue ?? ReadWireSignal(prev.WireLeft);
      int rightOrValue = prev.RightValue ?? ReadWireSignal(prev.WireRight);
      val = leftOrValue | rightOrValue;
      break;
    case OperationType.RSHIFT:
      int leftRValue = prev.LeftValue ?? ReadWireSignal(prev.WireLeft);
      int rightRValue = prev.RightValue ?? ReadWireSignal(prev.WireRight);
      val = leftRValue >> rightRValue;
      break;
    case OperationType.LSHIFT:
      int leftLValue = prev.LeftValue ?? ReadWireSignal(prev.WireLeft);
      int rightLValue = prev.RightValue ?? ReadWireSignal(prev.WireRight);
      val = leftLValue << rightLValue;
      break;
    default:
      return 0;
  }
  Answers[wire] = val;
  return val;
}

class Source {
  public string TargetWire { get; set; }
  public OperationType Operation { get; set; }
  public int? LeftValue { get; set; }
  public int? RightValue { get; set; }
  public string? WireLeft { get; set; }
  public string? WireRight { get; set; }
  public Source(string input) {
    var leftHand = input.Split("->")[0].Trim();
    TargetWire = input.Split("->")[1].Trim();
    // STATIC
    if (!leftHand.Contains(' ')) {
      Operation = OperationType.STATIC;
      if (int.TryParse(leftHand, out int numericalValue))
        LeftValue = numericalValue;
      else
        WireLeft = leftHand;
      return;
    }
    // NOT
    if (leftHand.Count(c => c == ' ') == 1) {
      Operation = OperationType.NOT;
      var notOperand = leftHand.Split(' ')[1];
      if (int.TryParse(notOperand, out int numericalValue))
        LeftValue = numericalValue;
      else
        WireLeft = notOperand;
      return;
    }
    // REST
    if (leftHand.Split(' ') is [string leftOperand, string operation, string rightOperand]) {
      Operation = operation switch {
        "AND" => OperationType.AND,
        "OR" => OperationType.OR,
        "RSHIFT" => OperationType.RSHIFT,
        _ => OperationType.LSHIFT
      };
      if (int.TryParse(leftOperand, out int leftNumericalValue))
        LeftValue = leftNumericalValue;
      else
        WireLeft = leftOperand;
      if (int.TryParse(rightOperand, out int rightNumericalValue))
        RightValue = rightNumericalValue;
      else
        WireRight = rightOperand;
    }
  }
}

enum OperationType { STATIC, AND, OR, NOT, RSHIFT, LSHIFT }