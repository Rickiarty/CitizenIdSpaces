using System.Diagnostics;
using ROCCitizenIdSpace;

Stopwatch sw = new();
sw.Start();

IEnumerable<string> filteredIds = ROCIdSpace.PossibleIds
	.Where(possibleId => 
		possibleId.Any(ch => ch == '2') &&
		possibleId.Any(ch => ch == '6')
	); // Do NOT call '.ToList()' here. 
int count = 0;
int total = 0;
foreach (string possibleId in filteredIds)
{
	Console.Write($"{possibleId} : {ROCIdSpace.A_to_District[possibleId[0].ToString()]}\n");
	count += 1;
}

sw.Stop();
TimeSpan t1 = sw.Elapsed;

sw.Restart();

total = ROCIdSpace.PossibleIds.Count();
decimal ratio = (decimal)count / (decimal)total;
Console.Write($"\ncount = {count}\ntotal = {total}\nratio = {ratio}\n\n");

sw.Stop();
TimeSpan t2 = sw.Elapsed;

Console.WriteLine($"Δt1 (time delta) = {t1}");
Console.WriteLine($"Δt2 (time delta) = {t2}");