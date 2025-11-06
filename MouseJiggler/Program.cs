using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
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
        // Attach to the parent process's console so we can display help, version information, and command-line errors.
        Kernel32.AttachConsole(Kernel32.ATTACH_PARENT_PROCESS);

        // Ensure that we are the only instance of the Mouse Jiggler currently running.
        var instance = new Mutex(initiallyOwned: false, name: "single instance: nospace.MouseJiggler");

        try
        {
            if (instance.WaitOne(millisecondsTimeout: 0))
            {
                // Parse arguments and do the appropriate thing.
                return Program.GetCommandLineParser().Invoke(args: args);
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
        // Create root command.
        var rootCommand = new RootCommand(Resources.Console_Root)
        {
            Handler = CommandHandler.Create(Program.Run)
        };

        // -j --jiggle
        Option optJiggling = new Option<bool>(aliases: ["--jiggle", "-j"],
            getDefaultValue: () => Settings.Default.AutostartJiggle,
            description: Resources.Console_Jiggle);
        rootCommand.AddOption(option: optJiggling);

        // -a --activity
        Option optActivity = new Option<bool>(aliases: ["--activity", "-a"],
            getDefaultValue: () => Settings.Default.CheckActivity,
            description: Resources.Console_ActivityCheck);
        rootCommand.AddOption(option: optActivity);

        // -m --mode
        Option optMode = new Option<JiggleMode>(aliases: ["--mode", "-m"],
            getDefaultValue: () => Settings.Default.JiggleMode,
            description: Resources.Console_JiggleMode);
        rootCommand.AddOption(option: optMode);

        // -d 20 --distance=20
        Option optDist = new Option<int>(aliases: ["--distance", "-d"],
            getDefaultValue: () => Settings.Default.JiggleSize,
            description: Resources.Console_Distance);
        rootCommand.AddOption(option: optDist);

        optDist.AddValidator(r =>
                             {
                                 if (r.GetValueOrDefault<int>() < 10)
                                 {
                                     r.ErrorMessage = Resources.ConsoleError_IntervalTooLow;
                                 }
                             });

        optDist.AddValidator(r =>
                             {
                                 if (r.GetValueOrDefault<int>() > 500)
                                 {
                                     r.ErrorMessage = Resources.ConsoleError_IntervalTooHigh;
                                 }
                             });

        // -s 60 --seconds=60
        Option optPeriod = new Option<int>(aliases: ["--seconds", "-s"],
            getDefaultValue: () => Settings.Default.JiggleInterval,
            description: Resources.Console_Interval);

        optPeriod.AddValidator(r =>
                               {
                                   if (r.GetValueOrDefault<int>() < 1)
                                   {
                                       r.ErrorMessage = Resources.ConsoleError_IntervalTooLow;
                                   }
                               });

        optPeriod.AddValidator(r =>
                               {
                                   if (r.GetValueOrDefault<int>() > 180)
                                   {
                                       r.ErrorMessage = Resources.ConsoleError_IntervalTooHigh;
                                   }
                               });

        rootCommand.AddOption(option: optPeriod);


        // Build the command line parser.
        return rootCommand;
    }
}