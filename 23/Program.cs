var cpu = new CPU("input.txt");
Console.WriteLine(cpu.Run());

class CPU(string programInput) {
  public int CurrentInstruction { get; set; } = 0;
  public ulong RegisterA { get; set; } = 1; // Set depending on step
  public ulong RegisterB { get; set; } = 0;
  public List<string> Program { get; set; } = File.ReadAllLines(programInput).ToList();

  public ulong Run() {
    while (true) {
      var command = Program[CurrentInstruction];
      var cmdData = command.Split(" ");
      switch (cmdData[0]) {
        case "hlf":
          Hlf(cmdData[1][0]);
          break;
        case "tpl":
          Tpl(cmdData[1][0]);
          break;
        case "inc":
          Inc(cmdData[1][0]);
          break;
        case "jmp":
          Jmp(cmdData[1]);
          break;
        case "jie":
          Jie(cmdData[1].Split(',')[0][0], cmdData[1].Split(',')[1].Trim());
          break;
        case "jio":
          Jio(cmdData[1].Split(',')[0][0], cmdData[1].Split(',')[1].Trim());
          break;
        default:
          throw new Exception($"INVALID OPERATION {RegisterB}");
      }
      CurrentInstruction++;
      if (CurrentInstruction > Program.Count - 1)
        return RegisterB;
    }
  }

  public void Hlf(char reg) {
    if (reg == 'a') RegisterA /= 2;
    else if (reg == 'b') RegisterB /= 2;
    else throw new Exception($"INVALID OPERATION {RegisterB}");
  }
  public void Tpl(char reg) {
    if (reg == 'a') RegisterA *= 3;
    else if (reg == 'b') RegisterB *= 3;
    else throw new Exception($"INVALID OPERATION {RegisterB}");
  }
  public void Inc(char reg) {
    if (reg == 'a') RegisterA++;
    else if (reg == 'b') RegisterB++;
    else throw new Exception($"INVALID OPERATION {RegisterB}");
  }
  public void Jmp(string offset) {
    if (int.TryParse(offset, out int index))
      CurrentInstruction += index - 1;
    else throw new Exception($"INVALID OPERATION {RegisterB}");
  }
  public void Jie(char reg, string offset) {
    if ((reg == 'a' && RegisterA % 2 == 0) || (reg == 'b' && RegisterB % 2 == 0)) {
      if (int.TryParse(offset, out int index))
        CurrentInstruction += index - 1;
      else throw new Exception($"INVALID OPERATION {RegisterB} {reg} {offset}");
    }
    if (reg != 'a' && reg != 'b') throw new Exception("INVALID OPERATION");
  }
  public void Jio(char reg, string offset) {
    if ((reg == 'a' && RegisterA == 1) || (reg == 'b' && RegisterB == 1)) {
      if (int.TryParse(offset, out int index))
        CurrentInstruction += index - 1;
      else throw new Exception($"INVALID OPERATION {RegisterB}");
    }
    if (reg != 'a' && reg != 'b') throw new Exception("INVALID OPERATION");
  }
}