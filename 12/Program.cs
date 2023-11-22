using Newtonsoft.Json;

var json = File.ReadAllText("input.txt");

// Sum all numbers
JsonTextReader reader = new(new StringReader(json));
long sum = 0;
while (reader.Read()) {
  if (reader.TokenType == JsonToken.Integer && reader.Value is long v)
    sum += v;
}
Console.WriteLine(sum);

// Sum all objects, ignoring objects with red
reader = new(new StringReader(json));
long sum2 = 0;
while (reader.Read()) {
  if (reader.TokenType == JsonToken.StartObject)
    sum2 += SumObject(reader);
}
Console.WriteLine(sum2);

static long SumObject(JsonTextReader reader) {
  // Read until End Object
  long sum = 0;
  bool foundRed = false;
  while (reader.Read()) {
    // If runs into red, return 0, then run until end object
    if (reader.TokenType == JsonToken.String && reader.Value?.ToString() == "red") {
      if (!reader.Path.EndsWith(']')) // Skip "red" in arrays
        foundRed = true;
    }
    else if (reader.TokenType == JsonToken.Integer && reader.Value is long v)
      sum += v;

    // This object is done, time to return sum
    if (reader.TokenType == JsonToken.EndObject)
      break;

    // If run into child object, recurse
    if (reader.TokenType == JsonToken.StartObject)
      sum += SumObject(reader);
  }
  if (foundRed) return 0;
  return sum;
}