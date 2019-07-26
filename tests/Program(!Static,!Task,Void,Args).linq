<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main(string[] args)
{
    Console.WriteLine(GetType().FullName);
    Console.WriteLine(Clock.Now);
    Console.WriteLine(string.Join(",", args));
}

static class Clock
{
    public static DateTime Now => DateTime.Now;
}