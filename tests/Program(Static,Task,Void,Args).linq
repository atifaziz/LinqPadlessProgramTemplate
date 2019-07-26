<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

static async Task Main(string[] args)
{
    Console.WriteLine(await Task.FromResult(Clock.Now));
    Console.WriteLine(string.Join(",", args));
}

static class Clock
{
    public static DateTime Now => DateTime.Now;
}