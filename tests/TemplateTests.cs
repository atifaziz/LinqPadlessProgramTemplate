using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Process;

public class TemplateTests
{
    public static readonly string TestDirectoryPath =
        Path.GetDirectoryName(typeof(TemplateTests).Assembly.Location);

    public static readonly string LinqDirectoryPath =
        Path.Join(TestDirectoryPath, "linq");

    public static readonly Lazy<string> LplessPath = new Lazy<string>(() =>
    {
        var isWindows =    Environment.GetEnvironmentVariable("WINDIR") != null
                        && Environment.GetEnvironmentVariable("COMSPEC") != null;

        return new DirectoryInfo(TestDirectoryPath)
            .AncestorsAndSelf()
            .Select(dir => Path.Join(dir.FullName, "lpless" + (isWindows ? ".cmd" : ".sh")))
            .First(File.Exists);
    });

    readonly ITestOutputHelper _testOutput;

    public TemplateTests(ITestOutputHelper output) =>
        _testOutput = output;

    void WriteLine(string s) =>
        _testOutput.WriteLine(s);

    void WriteLines(IEnumerable<string> source)
    {
        foreach (var s in source)
            WriteLine(s);
    }

    public static readonly IEnumerable<object[]> TestSource =
        from f in Directory.GetFiles(LinqDirectoryPath, "*.linq")
        select new object[] { Path.GetFileName(f) };

    [Theory]
    [MemberData(nameof(TestSource))]
    public void Test(string fileName)
    {
        var path = Path.Combine(LinqDirectoryPath, fileName);
        var content = File.ReadAllText(path);

        var expectedExitCode
            = Regex.Match(content, @"(?<=^//<\s*)[0-9]+(?=\s*$)", RegexOptions.Multiline).Value is string s && s.Length > 0
            ? int.Parse(s, NumberStyles.None, CultureInfo.InvariantCulture)
            : throw new FormatException("Missing expected exit code specification.");

        var expectedOutputLines =
            from m in Regex.Matches(content, @"(?<=^//\|).*", RegexOptions.Multiline)
            select m.Value.Trim();

        var cmd = Environment.GetEnvironmentVariable("COMSPEC");
        var program = cmd ?? "/usr/bin/env";

        var shargs = cmd != null ? new[] { "/c", "call" } : new[] { "bash" };

        string[] Arguments(params string[] args) =>
            shargs.Concat(args).ToArray();

        var (buildExitCode, result) =
            Spawn(program, Arguments(LplessPath.Value, "-x", path),
                  s => "STDOUT: " + s,
                  s => "STDERR: " + s);

        WriteLines(result);

        Assert.Equal(0, buildExitCode);

        var (exitCode, output) =
            Spawn(program, Arguments(LplessPath.Value, path, "foo", "bar", "baz"));

        WriteLines(result);

        Assert.Equal(expectedExitCode, exitCode);
        Assert.Equal(expectedOutputLines, output);
    }
}
