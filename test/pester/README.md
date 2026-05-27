# Pester tests

PowerShell (Pester v5) tests for the `Eryph.ComputeClient` cmdlets, focused on
the name-based lookup feature (issue #69).

## Layers

1. **Parameter surface** — verifies cmdlets expose `-Name` in the `list`
   parameter set, that it is a string and not positional. Needs **no server**
   and is safe to run in CI.
2. **Integration** — exercises the real cmdlet → HTTP → client-side filter path
   against a running eryph. These create a throwaway project (and read existing
   catlets) to assert exact / wildcard / case-insensitive matching and the
   empty-result case, then clean up. They are **skipped automatically** when no
   eryph is reachable.

> A fully server-less end-to-end test isn't practical here: the cmdlets use the
> eryph client-credentials OAuth flow plus on-disk configuration, so faking the
> backend would mean stubbing the identity *and* compute endpoints and a config
> store. The parameter-surface layer gives deterministic CI coverage; the
> integration layer gives real-world behavior when an eryph is present.

## Running

```powershell
# build the module and run everything
./Invoke-PesterTests.ps1 -Build

# run against an already-built module
./Invoke-PesterTests.ps1
```

The integration tests use the ambient eryph credentials (e.g. the eryph-zero
system-client — run as Administrator on Windows). When no eryph is reachable
only the parameter-surface tests run.

Requires Pester 5+:

```powershell
Install-Module Pester -MinimumVersion 5.0.0 -Scope CurrentUser
```
