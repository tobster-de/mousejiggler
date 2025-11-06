using System.CommandLine;
using System.CommandLine.Invocation;
using MouseJiggler.Properties;
using JetBrains.Annotations;
using MouseJiggler.PInvoke;

namespace MouseJiggler;

[PublicAPI]
public static class Program
{
    private static Option<int> _optPeriod = null!;
    private static Option<int> _optDist = null!;
    private static Option<JiggleMode> _optMode = null!;
    private static Option<bool> _optActivity = null!;
    private static Option<bool> _optJiggling = null!;

    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    [STAThread]
    public static int Main(string[] args)
    {
        // Attach to the parent process's console so we can display help, version information, and command-line errors.
        Kernel32.AttachConsole(Kernel32.ATTACH_PARENT_PROCESS);
        // Ensure that we are the only instance of the Mouse Jiggler currently running.
        var instance = new Mutex(initiallyOwned: false, name: "single instance: nospace.MouseJiggler");

        try
        {
            if (instance.WaitOne(millisecondsTimeout: 0))
            {
                // Parse arguments and do the appropriate thing.
                ParseResult parseResult = Program.GetCommandLineParser().Parse(args: args);
                return parseResult.Invoke();
            }
            else
            {
                Console.WriteLine(Resources.ConsoleError_AlreadyRunning);

                return 1;
            }
        }
        finally
        {
            instance.Close();

            // Detach from the parent console.
            Kernel32.FreeConsole();
        }
    }

    private static int Run(ParseResult parseResult)
    {
        var app = new App
        {
            JiggleActive = parseResult.GetValue(_optJiggling),
            CheckActivity = parseResult.GetValue(_optActivity),
            JigglePeriod = parseResult.GetValue(_optPeriod),
            JiggleMode = parseResult.GetValue(_optMode),
            JiggleSize = parseResult.GetValue(_optDist),
        };
        app.InitializeComponent();
        return app.Run();
    }

    private static RootCommand GetCommandLineParser()
    {
        // https://learn.microsoft.com/en-us/dotnet/standard/commandline/

        // Create root command.
        var rootCommand = new RootCommand(Resources.Console_Root);
        rootCommand.SetAction(Run);

        // -j --jiggle
        _optJiggling = new Option<bool>("--jiggle", "-j")
        {
            DefaultValueFactory = _ => Settings.Default.AutostartJiggle,
            Description = Resources.Console_Jiggle
        };
        rootCommand.Options.Add(_optJiggling);

        // -a --activity
        _optActivity = new Option<bool>("--activity", "-a")
        {
            DefaultValueFactory = _ => Settings.Default.CheckActivity,
            Description = Resources.Console_ActivityCheck
        };
        rootCommand.Options.Add(_optActivity);

        // -m --mode
        _optMode = new Option<JiggleMode>("--mode", "-m")
        {
            DefaultValueFactory = _ => Settings.Default.JiggleMode,
            Description = Resources.Console_JiggleMode
        };
        rootCommand.Options.Add(_optMode);

        // -d 20 --distance=20
        _optDist = new Option<int>("--distance", "-d")
        {
            DefaultValueFactory = _ => Settings.Default.JiggleSize,
            Description = Resources.Console_Distance,
            Validators =
            {
                result =>
                {
                    if (result.GetValueOrDefault<int>() < 10)
                    {
                        result.AddError(Resources.ConsoleError_IntervalTooLow);
                    }
                },
                result =>
                {
                    if (result.GetValueOrDefault<int>() > 500)
                    {
                        result.AddError(Resources.ConsoleError_IntervalTooHigh);
                    }
                }
            }
        };
        rootCommand.Options.Add(_optDist);

        // -s 60 --seconds=60
        _optPeriod = new Option<int>("--seconds", "-s")
        {
            DefaultValueFactory = _ => Settings.Default.JiggleInterval,
            Description = Resources.Console_Interval,
            Validators =
            {
                result =>
                {
                    if (result.GetValueOrDefault<int>() < 1)
                    {
                        result.AddError(Resources.ConsoleError_IntervalTooLow);
                    }
                },
                result =>
                {
                    if (result.GetValueOrDefault<int>() > 180)
                    {
                        result.AddError(Resources.ConsoleError_IntervalTooHigh);
                    }
                }
            }
        };
        rootCommand.Options.Add(_optPeriod);

        // Build the command line parser.
        return rootCommand;
    }
}