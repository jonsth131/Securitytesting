using System;
using System.Collections.Generic;
using NZap.Entities;

namespace Securitytesting.Helpers
{
    public static class AlertHelper
    {
        public static void PrintAlertsToConsole(ICollection<IAlertResult> alerts)
        {
            foreach (var alert in alerts)
            {
                Console.WriteLine(alert.Alert
                    + Environment.NewLine
                    + alert.Cweid
                    + Environment.NewLine
                    + alert.Url
                    + Environment.NewLine
                    + alert.Wascid
                    + Environment.NewLine
                    + alert.Evidence
                    + Environment.NewLine
                    + alert.Param
                    + Environment.NewLine
                );
            }
        }
    }
}
