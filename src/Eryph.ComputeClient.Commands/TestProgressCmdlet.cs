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
            WriteProgress(new ProgressRecord(0, "Parent", "Running")
            {
                PercentComplete = -1,
                CurrentOperation = "bla bla bla",
            });

            for (int i = 0; i < 10; i++)
            {
                WriteProgress(new ProgressRecord(1, "Parent", "Running")
                {
                    ParentActivityId = 0,
                    PercentComplete = 0,
                    CurrentOperation = "bla bla bla",
                });

                WriteProgress(new ProgressRecord(2, "Child", "Running")
                {
                    ParentActivityId = 1,
                    PercentComplete = 0
                });

                Thread.Sleep(500);



                WriteProgress(new ProgressRecord(2, "Child", "Running")
                {
                    PercentComplete = 100,
                    ParentActivityId = 1,
                    RecordType = ProgressRecordType.Completed,
                });

                Thread.Sleep(100);

                WriteProgress(new ProgressRecord(1, "Parent", "Running")
                {
                    ParentActivityId = 0,
                    PercentComplete = 100,
                    CurrentOperation = "bla bla bla",
                    RecordType = ProgressRecordType.Completed,
                });

                

                Thread.Sleep(500);
            }
        }
    }
}
