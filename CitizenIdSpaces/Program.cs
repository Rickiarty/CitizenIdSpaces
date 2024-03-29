using System.Diagnostics;
using ROCCitizenIdSpace;

Stopwatch sw = new();
sw.Start();

int count = 0;
int total = 0;
foreach (string possibleId in ROCIdSpace.PossibleIds)
{
	if (
		possibleId.Any(ch => ch == '2') &&
		possibleId.Any(ch => ch == '6')
		)
	{
		Console.Write($"{possibleId} : {ROCIdSpace.A_to_District[possibleId[0].ToString()]}\n");
		count += 1;
	}
	total += 1;
}
decimal ratio = (decimal)count / (decimal)total;
Console.Write($"\ncount = {count}\ntotal = {total}\nratio = {ratio}\n");

sw.Stop();

Console.WriteLine($"Δt (time delta) = {sw.Elapsed}");