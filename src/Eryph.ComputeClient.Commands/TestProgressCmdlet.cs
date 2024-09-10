using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eryph.ComputeClient.Commands
{
    [Cmdlet(VerbsCommon.Show, "Test")]
    public class ShowTestCmdlet : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteProgress(new ProgressRecord(0, "Root", "Running")
            {
                PercentComplete = -1,
            });

            for (var i = 0; i < 10; i++)
            {
                WriteProgress(new ProgressRecord(1, $"Parent {i}", "Running")
                {
                    ParentActivityId = 0,
                    PercentComplete = 0,
                });

                Thread.Sleep(100);

                WriteProgress(new ProgressRecord(2, $"Child {i}", "Running")
                {
                    ParentActivityId = 1,
                    PercentComplete = 0
                });

                Thread.Sleep(500);

                WriteProgress(new ProgressRecord(2, $"Child {i}", "Done")
                {
                    PercentComplete = 100,
                    ParentActivityId = 1,
                    RecordType = ProgressRecordType.Completed,
                });

                Thread.Sleep(100);

                WriteProgress(new ProgressRecord(1, $"Parent {i}", "Done")
                {
                    ParentActivityId = 0,
                    PercentComplete = 100,
                    RecordType = ProgressRecordType.Completed,
                });

                Thread.Sleep(500);
            }
        }
    }
}
