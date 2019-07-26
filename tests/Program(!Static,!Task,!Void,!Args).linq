<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

int Main()
{
    Console.WriteLine(GetType().FullName);
    Console.WriteLine(Clock.Now);
    return 42;
}

static class Clock
{
    public static DateTime Now => DateTime.Now;
}