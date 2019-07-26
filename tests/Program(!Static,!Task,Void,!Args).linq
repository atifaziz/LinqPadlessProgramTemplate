<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
    Console.WriteLine(GetType().FullName);
    Console.WriteLine(Clock.Now);
}

static class Clock
{
    public static DateTime Now => DateTime.Now;
}