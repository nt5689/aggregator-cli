﻿using System.Threading;
using System.Threading.Tasks;
using CommandLine;

namespace aggregator.cli
{
    [Verb("configure.instance", HelpText = "Configures an existing Aggregator instance.")]
    internal class ConfigureInstanceCommand : CommandBase
    {
        [Option('n', "name", Required = true, HelpText = "Aggregator instance name.")]
        public string Name { get; set; }

        [ShowInTelemetry]
        [Option('l', "location", Required = true, HelpText = "Aggregator instance location (Azure region).")]
        public string Location { get; set; }

        [ShowInTelemetry(TelemetryDisplayMode.Presence)]
        [Option('g', "resourceGroup", Required = false, Default = "", HelpText = "Azure Resource Group hosting the Aggregator instance.")]
        public string ResourceGroup { get; set; }

        [ShowInTelemetry]
        [Option('a', "authentication", SetName = "auth", Required = true, HelpText = "Refresh authentication data.")]
        public bool Authentication { get; set; }

        [ShowInTelemetry]
        [Option('m', "saveMode", SetName = "save", Required = false, HelpText = "Save behaviour.")]
        public SaveMode SaveMode { get; set; }

        // TODO add --swap.slot to support App Service Deployment Slots


        internal override async Task<int> RunAsync(CancellationToken cancellationToken)
        {
            var context = await Context
                .WithAzureLogon()
                .WithDevOpsLogon() // need the token, so we can save it in the app settings
                .BuildAsync(cancellationToken);
            context.ResourceGroupDeprecationCheck(this.ResourceGroup);
            var instances = new AggregatorInstances(context.Azure, context.Logger, context.Naming);
            var instance = context.Naming.Instance(Name, ResourceGroup);
            if (Authentication)
            {
                bool ok = await instances.ChangeAppSettingsAsync(instance, Location, SaveMode, cancellationToken);
                return ok ? ExitCodes.Success : ExitCodes.Failure;
            }
            else
            {
                context.Logger.WriteError($"Unsupported command option(s)");
                return ExitCodes.InvalidArguments;
            }
        }
    }
}
