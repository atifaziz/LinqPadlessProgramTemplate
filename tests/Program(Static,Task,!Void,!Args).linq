<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

static async Task<int> Main()
{
    Console.WriteLine(Clock.Now);
    return await Task.FromResult(42);
}

static class Clock
{
    public static DateTime Now => DateTime.Now;
}