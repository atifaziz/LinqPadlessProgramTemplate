<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

static void Main()
{
    Console.WriteLine(Clock.Now);
}

static class Clock
{
    public static DateTime Now => DateTime.Now;
}