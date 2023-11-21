using System.Text.RegularExpressions;

var instructions = File.ReadAllLines("input.txt").Select(l => new Instruction(l)).ToList();

var Grid = new int[1000,1000];
foreach (var instruction in instructions)
  instruction.Apply(Grid);
Console.WriteLine(CountLights(Grid));

Grid = new int[1000,1000];
foreach (var instruction in instructions)
  instruction.ApplyWeird(Grid);
Console.WriteLine(CountTotalLight(Grid));

int CountTotalLight(int[,] grid) {
  var counter = 0;
  foreach (var light in grid)
    counter += light;
  return counter;
}

int CountLights(int[,] grid) {
  var counter = 0;
  foreach (var light in grid)
    if (light == 1) counter++;
  return counter;
}

enum Action {
  ON, OFF, TOGGLE
}

class Instruction {
  public Action Action { get; set; }
  public (int x, int y) StartCoordinate { get; set; }
  public (int x, int y) EndCoordinate { get; set; }
  public Instruction(string input) {
    if (input.StartsWith("toggle"))
      Action = Action.TOGGLE;
    else if (input.StartsWith("turn on"))
      Action = Action.ON;
    else
      Action = Action.OFF;

    var match = Regex.Match(input, @"(\d+),(\d+) through (\d+),(\d+)");
    StartCoordinate = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
    EndCoordinate = (int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
  }
  public void Apply(int[,] grid) {
    for (var x = StartCoordinate.x; x <= EndCoordinate.x; x++) {
      for (var y = StartCoordinate.y; y <= EndCoordinate.y; y++) {
        if (Action == Action.ON)
          grid[x,y] = 1;
        else if (Action == Action.OFF)
          grid[x,y] = 0;
        else
          grid[x,y] = grid[x,y] == 1 ? 0 : 1;
      }
    }
  }

  public void ApplyWeird(int[,] grid) {
    for (var x = StartCoordinate.x; x <= EndCoordinate.x; x++) {
      for (var y = StartCoordinate.y; y <= EndCoordinate.y; y++) {
        if (Action == Action.ON)
          grid[x,y]++;
        else if (Action == Action.OFF)
          grid[x,y] = grid[x,y] > 0 ? grid[x,y]-1 : grid[x,y];
        else
          grid[x,y] += 2;
      }
    }
  }
}