using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace curldotnet.Commands
{
    [Description("Curl imitation.")]
    internal sealed class CurlCmd : Command<CurlCmd.Settings>
    {
        public sealed class Settings : CommandSettings
        {
            [CommandOption("-s|--startdate [STARTDATE]")]
            [Description("Begin scanning at this date. Defaults to '[grey]DateTime.Min[/]'.")]
            public FlagValue<DateTime>? StartDate { get; init; } = new FlagValue<DateTime>() { Value = DateTime.MinValue };

            [CommandOption("-e|--enddate [ENDDATE]")]
            [Description("Begin scanning at this date. Defaults to '[grey]DateTime.Now[/]'.")]
            public FlagValue<DateTime>? EndDate { get; init; } = new FlagValue<DateTime>() { Value = DateTime.Now };
        }


        public override int Execute(CommandContext context, Settings settings)
        {
            Console.WriteLine("Executing scan-logs");

            return 0;
        }
    }
}
