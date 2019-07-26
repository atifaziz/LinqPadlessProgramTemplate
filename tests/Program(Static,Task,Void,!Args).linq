<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

static async Task Main()
{
    Console.WriteLine(await Task.FromResult(Clock.Now));
}

static class Clock
{
    public static DateTime Now => DateTime.Now;
}