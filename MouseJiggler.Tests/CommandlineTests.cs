using System.CommandLine;
using System.Reflection;
using MouseJiggler.Properties;

namespace MouseJiggler.Tests;

public class CommandLineTests
{
    [Test]
    public void Parse_WithLongArguments_MapsValuesCorrectly()
    {
        // Arrange
        string[] arguments = ["--jiggle", "--activity", "WinApiIdleTime", "--mode", "Circle", "--distance", "120", "--seconds", "90"];
        RootCommand parser = GetParser();

        // Act
        ParseResult parseResult = parser.Parse(arguments);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(parseResult.Errors, Is.Empty);
            Assert.That(GetOptionValue<bool>(parseResult, "--jiggle"), Is.True);
            Assert.That(GetOptionValue<ActivityDetectionMode>(parseResult, "--activity"),
                Is.EqualTo(ActivityDetectionMode.WinApiIdleTime));
            Assert.That(GetOptionValue<JiggleMode>(parseResult, "--mode"), Is.EqualTo(JiggleMode.Circle));
            Assert.That(GetOptionValue<int>(parseResult, "--distance"), Is.EqualTo(120));
            Assert.That(GetOptionValue<int>(parseResult, "--seconds"), Is.EqualTo(90));
        }
    }

    [Test]
    public void Parse_WithShortArguments_MapsValuesCorrectly()
    {
        // Arrange
        string[] arguments = ["-j", "-a", "MouseMovement", "-m", "Horizontal", "-d", "45", "-s", "10"];
        RootCommand parser = GetParser();

        // Act
        ParseResult parseResult = parser.Parse(arguments);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(parseResult.Errors, Is.Empty);
            Assert.That(GetOptionValue<bool>(parseResult, "--jiggle"), Is.True);
            Assert.That(GetOptionValue<ActivityDetectionMode>(parseResult, "--activity"),
                Is.EqualTo(ActivityDetectionMode.MouseMovement));
            Assert.That(GetOptionValue<JiggleMode>(parseResult, "--mode"), Is.EqualTo(JiggleMode.Horizontal));
            Assert.That(GetOptionValue<int>(parseResult, "--distance"), Is.EqualTo(45));
            Assert.That(GetOptionValue<int>(parseResult, "--seconds"), Is.EqualTo(10));
        }
    }

    [Test]
    public void Parse_WithoutArguments_UsesConfiguredDefaults()
    {
        // Arrange
        RootCommand parser = GetParser();
        bool expectedJiggle = (bool)GetSettingsDefaultValue("AutostartJiggle");
        ActivityDetectionMode expectedActivity = (ActivityDetectionMode)GetSettingsDefaultValue("ActivityDetectionMode");
        JiggleMode expectedMode = (JiggleMode)GetSettingsDefaultValue("JiggleMode");
        int expectedDistance = (int)GetSettingsDefaultValue("JiggleSize");
        int expectedSeconds = (int)GetSettingsDefaultValue("JiggleInterval");

        // Act
        ParseResult parseResult = parser.Parse([]);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(parseResult.Errors, Is.Empty);
            Assert.That(GetOptionValue<bool>(parseResult, "--jiggle"), Is.EqualTo(expectedJiggle));
            Assert.That(GetOptionValue<ActivityDetectionMode>(parseResult, "--activity"), Is.EqualTo(expectedActivity));
            Assert.That(GetOptionValue<JiggleMode>(parseResult, "--mode"), Is.EqualTo(expectedMode));
            Assert.That(GetOptionValue<int>(parseResult, "--distance"), Is.EqualTo(expectedDistance));
            Assert.That(GetOptionValue<int>(parseResult, "--seconds"), Is.EqualTo(expectedSeconds));
        }
    }

    [TestCaseSource(nameof(ActivityModeCases))]
    public void Parse_WithAllActivityEnumValues_MapsValueCorrectly(string optionAlias, ActivityDetectionMode expectedMode)
    {
        // Arrange
        RootCommand parser = GetParser();
        string[] arguments = [optionAlias, expectedMode.ToString()];

        // Act
        ParseResult parseResult = parser.Parse(arguments);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(parseResult.Errors, Is.Empty);
            Assert.That(GetOptionValue<ActivityDetectionMode>(parseResult, "--activity"), Is.EqualTo(expectedMode));
        }
    }

    [TestCaseSource(nameof(JiggleModeCases))]
    public void Parse_WithAllJiggleEnumValues_MapsValueCorrectly(string optionAlias, JiggleMode expectedMode)
    {
        // Arrange
        RootCommand parser = GetParser();
        string[] arguments = [optionAlias, expectedMode.ToString()];

        // Act
        ParseResult parseResult = parser.Parse(arguments);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(parseResult.Errors, Is.Empty);
            Assert.That(GetOptionValue<JiggleMode>(parseResult, "--mode"), Is.EqualTo(expectedMode));
        }
    }

    [TestCase("--activity", "invalid-activity-value")]
    [TestCase("-a", "invalid-activity-value")]
    [TestCase("--mode", "invalid-jiggle-mode")]
    [TestCase("-m", "invalid-jiggle-mode")]
    public void Parse_WithInvalidEnumValue_AddsParseError(string optionAlias, string invalidValue)
    {
        // Arrange
        RootCommand parser = GetParser();
        string[] arguments = [optionAlias, invalidValue];

        // Act
        ParseResult parseResult = parser.Parse(arguments);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(parseResult.Errors, Is.Not.Empty);
            Assert.That(parseResult.Errors.Select(error => error.Message), Has.Some.Contains(invalidValue));
        }
    }

    [TestCase("--distance", "9", true)]
    [TestCase("--distance", "501", false)]
    [TestCase("--seconds", "0", true)]
    [TestCase("--seconds", "181", false)]
    public void Parse_WithOutOfRangeNumericArguments_AddsValidationError(string argument, string value, bool tooLow)
    {
        // Arrange
        RootCommand parser = GetParser();
        string expectedError
            = tooLow ? GetResourceString("ConsoleError_IntervalTooLow") : GetResourceString("ConsoleError_IntervalTooHigh");

        // Act
        ParseResult parseResult = parser.Parse([argument, value]);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(parseResult.Errors, Has.Count.EqualTo(1));
            Assert.That(parseResult.Errors[0].Message, Is.EqualTo(expectedError));
        }
    }

    private static IEnumerable<TestCaseData> ActivityModeCases()
    {
        foreach (ActivityDetectionMode mode in Enum.GetValues<ActivityDetectionMode>())
        {
            yield return new TestCaseData("--activity", mode)
                .SetName($"Parse_WithAllActivityEnumValues_MapsValueCorrectly_LongOption_{mode}");
            yield return new TestCaseData("-a", mode)
                .SetName($"Parse_WithAllActivityEnumValues_MapsValueCorrectly_ShortOption_{mode}");
        }
    }

    private static IEnumerable<TestCaseData> JiggleModeCases()
    {
        foreach (JiggleMode mode in Enum.GetValues<JiggleMode>())
        {
            yield return new TestCaseData("--mode", mode)
                .SetName($"Parse_WithAllJiggleEnumValues_MapsValueCorrectly_LongOption_{mode}");
            yield return new TestCaseData("-m", mode)
                .SetName($"Parse_WithAllJiggleEnumValues_MapsValueCorrectly_ShortOption_{mode}");
        }
    }

    private static RootCommand GetParser()
    {
        MethodInfo? parserMethod = typeof(Program).GetMethod("GetCommandLineParser", BindingFlags.Static | BindingFlags.NonPublic);
        Assert.That(parserMethod, Is.Not.Null, "Program.GetCommandLineParser could not be found.");
        return (RootCommand)parserMethod!.Invoke(null, null)!;
    }

    private static T GetOptionValue<T>(ParseResult parseResult, string alias)
    {
        string normalizedAlias = alias.TrimStart('-');
        Option[] options = parseResult.CommandResult.Command.Options.OfType<Option>().ToArray();
        Option? option = options
            .FirstOrDefault(candidate =>
                                candidate.Name.TrimStart('-').Equals(normalizedAlias, StringComparison.OrdinalIgnoreCase) ||
                                candidate.Aliases.Any(optionAlias =>
                                                          optionAlias.TrimStart('-').Equals(normalizedAlias,
                                                              StringComparison.OrdinalIgnoreCase)));

        Assert.That(option, Is.Not.Null,
            $"Option '{alias}' not found. Available options: {string.Join(", ", options.Select(candidate => $"{candidate.Name} [{string.Join("|", candidate.Aliases)}]"))}");

        return parseResult.GetValue((dynamic)option!);
    }

    private static object GetSettingsDefaultValue(string propertyName)
    {
        Type? settingsType = typeof(Program).Assembly.GetType("MouseJiggler.Properties.Settings");
        Assert.That(settingsType, Is.Not.Null, "Settings type could not be found.");

        PropertyInfo? defaultProperty = settingsType!.GetProperty("Default", BindingFlags.Public | BindingFlags.Static);
        Assert.That(defaultProperty, Is.Not.Null, "Settings.Default property could not be found.");

        object? settingsInstance = defaultProperty!.GetValue(null);
        Assert.That(settingsInstance, Is.Not.Null, "Settings.Default instance is null.");

        PropertyInfo? valueProperty = settingsType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        Assert.That(valueProperty, Is.Not.Null, $"Settings property '{propertyName}' could not be found.");

        return valueProperty!.GetValue(settingsInstance)!;
    }

    private static string GetResourceString(string propertyName)
    {
        Type? resourcesType = typeof(Program).Assembly.GetType("MouseJiggler.Properties.Resources");
        Assert.That(resourcesType, Is.Not.Null, "Resources type could not be found.");

        PropertyInfo? property
            = resourcesType!.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        Assert.That(property, Is.Not.Null, $"Resource property '{propertyName}' could not be found.");

        return (string)property!.GetValue(null)!;
    }
}