using System.Security.Cryptography;

var secret = "ckczppom";
Console.WriteLine(FindSmallestAddition(secret, "00000"));
Console.WriteLine(FindSmallestAddition(secret, "000000"));

int FindSmallestAddition(string secret, string start) {
  using var md5 = MD5.Create();
  for (int i = 0; i < 10000000; i++) {
    var result = CreateMD5(md5, secret + i.ToString());
    if (result.StartsWith(start))
      return i;
  }
  return 0;
}

string CreateMD5(MD5 md5, string input) {
  byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
  byte[] hashBytes = md5.ComputeHash(inputBytes);
  return Convert.ToHexString(hashBytes);
}