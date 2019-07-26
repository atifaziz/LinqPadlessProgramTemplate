<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

static void Main(string[] args)
{
    Console.WriteLine(Clock.Now);
    Console.WriteLine(string.Join(",", args));
}

static class Clock
{
    public static DateTime Now => DateTime.Now;
}