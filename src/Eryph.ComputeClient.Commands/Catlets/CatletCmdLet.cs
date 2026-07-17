using System;
using System.Collections.Generic;
using System.Linq;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    public abstract class CatletCmdLet : ComputeCmdLet
    {
        /// <summary>Resource kind used in catlet not-found / ambiguity messages.</summary>
        protected const string CatletResourceKind = "catlet";

        /// <summary>
        /// Hint shown when a catlet name is ambiguous, naming the parameter that
        /// disambiguates it (a catlet name is unique only per project + environment).
        /// </summary>
        protected const string EnvironmentAmbiguityHint = "-Environment";

        /// <summary>
        /// The <see cref="EnvironmentAmbiguityHint"/>, but only when <paramref name="environment"/>
        /// was not already supplied. Once -Environment has been applied it did not resolve the
        /// ambiguity, so suggesting it again would be misleading; return null in that case.
        /// </summary>
        protected static string EnvironmentHintIfUnset(string environment) =>
            string.IsNullOrWhiteSpace(environment) ? EnvironmentAmbiguityHint : null;

        protected Catlet GetSingleCatlet(string id)
        {
            return Factory.CreateCatletsClient().Get(id);
        }

        /// <summary>
        /// Lists the catlets of a project, optionally narrowed to a single environment.
        /// A catlet's name is unique only per project + environment, so a lookup by name
        /// can match several catlets across environments; passing an environment restricts
        /// the listing to that environment (case-insensitive exact match). An empty/null
        /// environment applies no filter and lists every environment. As the server has no
        /// environment filter, this is applied client-side, mirroring Get-VNetwork and
        /// Get-CatletDisk. An explicit id (a GUID) bypasses this listing entirely.
        /// </summary>
        protected IEnumerable<Catlet> ListCatlets(string projectId, string environment)
        {
            IEnumerable<Catlet> catlets = Factory.CreateCatletsClient().List(projectId: projectId);
            if (!string.IsNullOrWhiteSpace(environment))
                catlets = catlets.Where(c => string.Equals(c.Environment, environment, StringComparison.OrdinalIgnoreCase));
            return catlets;
        }

        protected void WaitForOperation(
            Operation operation,
            bool noWait,
            bool alwaysWriteMachine,
            string knownMachineId = null)
        {
            if (noWait)
            {
                if (knownMachineId == null || !alwaysWriteMachine)
                    WriteObject(operation);
                else
                    WriteObject(GetSingleCatlet(knownMachineId));
                return;
            }

            var completedOperation = WaitForOperation(operation);
            WriteResources(completedOperation, ResourceType.Catlet);
        }
    }
}