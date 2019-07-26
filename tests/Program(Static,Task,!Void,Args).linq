<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

static async Task<int> Main(string[] args)
{
    Console.WriteLine(Clock.Now);
    Console.WriteLine(string.Join(",", args));
    return await Task.FromResult(42);
}

static class Clock
{
    public static DateTime Now => DateTime.Now;
}