<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
    Console.WriteLine(GetType().FullName);
    Console.WriteLine(await Task.FromResult(Clock.Now));
}

static class Clock
{
    public static DateTime Now => DateTime.Now;
}