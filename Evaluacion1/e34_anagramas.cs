Console.Write("Primera palabra: ");
string a = (Console.ReadLine() ?? "").ToLower();
Console.Write("Segunda palabra: ");
string b = (Console.ReadLine() ?? "").ToLower();

char[] letrasA = a.ToCharArray();
char[] letrasB = b.ToCharArray();
Array.Sort(letrasA);
Array.Sort(letrasB);

if (new string(letrasA) == new string(letrasB))
    Console.WriteLine("Son anagramas.");
else
    Console.WriteLine("No son anagramas.");