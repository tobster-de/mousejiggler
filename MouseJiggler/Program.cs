using System.CommandLine;
using MouseJiggler.Properties;
using JetBrains.Annotations;
using MouseJiggler.PInvoke;

namespace MouseJiggler;

[PublicAPI]
public static class Program
{
    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    [STAThread]
    public static int Main(string[] args)
    {
        // System.Threading.Thread.CurrentThread.CurrentCulture = 
        //     System.Threading.Thread.CurrentThread.CurrentUICulture = 
        //         new System.Globalization.CultureInfo("en-US");
        
        // Attach to the parent process's console so we can display help, version information, and command-line errors.
        Kernel32.AttachConsole(Kernel32.ATTACH_PARENT_PROCESS);
        Kernel32.SetConsoleCP(Kernel32.CP_UTF8);
        Kernel32.SetConsoleOutputCP(Kernel32.CP_UTF8);

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

    private static int Run(bool jiggle, bool checkActivity, JiggleMode mode, int distance, int seconds)
    {
        var app = new App
        {
            JiggleActive = jiggle,
            CheckActivity = checkActivity,
            JigglePeriod = seconds,
            JiggleMode = mode,
            JiggleSize = distance
        };
        app.InitializeComponent();
        return app.Run();
    }

    private static RootCommand GetCommandLineParser()
    {
        // https://learn.microsoft.com/en-us/dotnet/standard/commandline/

        // Create root command.
        var rootCommand = new RootCommand(Resources.Console_Root);

        // -j --jiggle
        var optJiggling = new Option<bool>("--jiggle", "-j")
        {
            DefaultValueFactory = _ => Settings.Default.AutostartJiggle,
            Description = Resources.Console_Jiggle
        };
        rootCommand.Add(optJiggling);

        // -a --activity
        var optActivity = new Option<bool>("--activity", "-a")
        {
            DefaultValueFactory = _ => Settings.Default.CheckActivity,
            Description = Resources.Console_ActivityCheck
        };
        rootCommand.Add(optActivity);

        // -m --mode
        var optMode = new Option<JiggleMode>("--mode", "-m")
        {
            DefaultValueFactory = _ => Settings.Default.JiggleMode,
            Description = Resources.Console_JiggleMode
        };
        rootCommand.Add(optMode);

        // -d 20 --distance=20
        var optDist = new Option<int>("--distance", "-d")
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
                    if (result.GetValueOrDefault<int>() > 500)
                    {
                        result.AddError(Resources.ConsoleError_IntervalTooHigh);
                    }
                }
            }
        };
        rootCommand.Add(optDist);

        // -s 60 --seconds=60
        var optPeriod = new Option<int>("--seconds", "-s")
        {
            Description = Resources.Console_Interval,
            Validators =
            {
                result =>
                {
                    if (result.GetValueOrDefault<int>() < 1)
                    {
                        result.AddError(Resources.ConsoleError_IntervalTooLow);
                    }
                    if (result.GetValueOrDefault<int>() > 180)
                    {
                        result.AddError(Resources.ConsoleError_IntervalTooHigh);
                    }
                }
            }
        };

        rootCommand.Add(optPeriod);

        rootCommand.SetAction(parsed
            => Run(parsed.GetValue(optJiggling),
                parsed.GetValue(optActivity),
                parsed.GetValue(optMode),
                parsed.GetValue(optDist),
                parsed.GetValue(optPeriod)));

        // Build the command line parser.
        return rootCommand;
    }
}