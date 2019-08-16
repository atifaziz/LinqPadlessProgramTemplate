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

    public static readonly Lazy<string> LplessPath = new Lazy<string>(() =>
        new DirectoryInfo(TestDirectoryPath).AncestorsAndSelf()
            .Select(dir => Path.Join(dir.FullName, "lpless.cmd"))
            .First(File.Exists));

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
        from f in Directory.GetFiles(TestDirectoryPath, "*.linq")
        select new object[] { Path.GetFileName(f) };

    [Theory]
    [MemberData(nameof(TestSource))]
    public void Test(string fileName)
    {
        var path = Path.Combine(TestDirectoryPath, fileName);
        var content = File.ReadAllText(path);

        var expectedExitCode
            = Regex.Match(content, @"(?<=^//<\s*)[0-9]+(?=\s*$)", RegexOptions.Multiline).Value is string s && s.Length > 0
            ? int.Parse(s, NumberStyles.None, CultureInfo.InvariantCulture)
            : throw new FormatException("Missing expected exit code specification.");

        var expectedOutputLines =
            from m in Regex.Matches(content, @"(?<=^//\|).*", RegexOptions.Multiline)
            select m.Value.Trim();

        var cmd = Environment.GetEnvironmentVariable("COMSPEC");

        var (buildExitCode, result) =
            Spawn(cmd, new[] { "/c", "call", LplessPath.Value, "-x", path },
                  s => "STDOUT: " + s,
                  s => "STDERR: " + s);

        WriteLines(result);

        Assert.Equal(0, buildExitCode);

        var (exitCode, output) =
            Spawn(cmd, "/c", "call", LplessPath.Value, path, "foo", "bar", "baz");

        WriteLines(result);

        Assert.Equal(expectedExitCode, exitCode);
        Assert.Equal(expectedOutputLines, output);
    }
}
