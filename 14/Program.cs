var Reindeers = File.ReadAllLines("input.txt").Select(l => new Reindeer(l)).ToList();
for (var i = 0; i < 2503; i++) {
  foreach (var r in Reindeers)
    r.Update();
  Reindeers.OrderByDescending(r => r.DistanceTraveled).First().Points++;
}
Console.WriteLine("Max distance: " + Reindeers.Max(r => r.DistanceTraveled));
Console.WriteLine("Max points: " + Reindeers.Max(r => r.Points));

class Reindeer {
  public string Name { get; set; }
  public int Speed { get; set; }
  public int FlyTime { get; set; }
  public int Rest { get; set; }
  public int DistanceTraveled { get; set; }
  public int Points { get; set; }

  private int FlownFor { get; set; }
  private int RestedFor { get; set; }
  private bool Resting { get; set; }

  public Reindeer(string input) {
    var data = input.Split(' ');
    Name = data[0];
    Speed = int.Parse(data[3]);
    FlyTime = int.Parse(data[6]);
    Rest = int.Parse(data[13]);
  }

  public void Update() {
    if (!Resting) {
      DistanceTraveled += Speed;
      FlownFor++;
      if (FlownFor == FlyTime) {
        FlownFor = 0;
        Resting = true;
      }
    } else {
      RestedFor++;
      if (RestedFor == Rest) {
        RestedFor = 0;
        Resting = false;
      }
    }
  }
}