<Query Kind="Program" />

void Main()
{
    Print(NestedClass.StaticMethod      );
    Print(NestedStruct.StaticMethod     );
    Print(NestedStaticClass.StaticMethod);
    Print(default(object).Extension     );
    Print(Extensions.StaticMethod       );
}

static void Print(Action a) =>
    Console.WriteLine($"{a.Method.DeclaringType.FullName}::{a.Method.Name}");

struct NestedStruct
{
    public static void StaticMethod() {}
}

class NestedClass
{
    public static void StaticMethod() {}
}

static class NestedStaticClass
{
    public static void StaticMethod() {}
}

static class Extensions
{
    public static void StaticMethod() {}
    public static void Extension<T>(this T _) {}
}

//< 0
//| UserQuery+NestedClass::StaticMethod
//| UserQuery+NestedStruct::StaticMethod
//| UserQuery+NestedStaticClass::StaticMethod
//| Extensions::Extension
//| Extensions::StaticMethod
