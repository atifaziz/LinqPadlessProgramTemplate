<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

int Main(string[] args)
{
    Console.WriteLine(GetType().FullName);
    Console.WriteLine(Clock.Now);
    return args.Length;
}

static class Clock
{
    public static DateTime Now => DateTime.Now;
}