﻿using System;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsLifecycle.Disable, "PowerShellTelemetry")]
    public class DisablePowerShellTelemetry : PSCmdlet
    {
        [Parameter(Mandatory = false)]
        public SwitchParameter Force;

        protected override void ProcessRecord()
        {
            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var telemetryFile = System.IO.Path.Combine(userProfile, ".pnppowershelltelemetry");
            if (Force || ShouldContinue("Do you want to disable telemetry for PnP PowerShell?", "Confirm"))
            {
                System.IO.File.WriteAllText(telemetryFile, "disallow");
                if (PnPConnection.CurrentConnection != null)
                {
                    PnPConnection.CurrentConnection.ApplicationInsights = null;
                }
                WriteObject("Telemetry disabled");
            }
            else
            {
                var enabled = false;
                if (System.IO.File.Exists(telemetryFile))
                {
                    enabled = System.IO.File.ReadAllText(telemetryFile).ToLower() == "allow";
                }
                WriteObject($"Telemetry setting unchanged: currently {(enabled ? "enabled" : "disabled")}");
            }
        }
    }
}