namespace Crosscutting.Common.Configurations.Global;

public class GlobalSettings
{
    public bool UseRateLimit { get; internal set; }
    public bool UseHealthChecks { get; internal set; }
    public bool UseCarter { get; internal set; }
    public bool UseOpenTelemetry { get; internal set; }

    public GlobalSettings()
    {
        UseRateLimit = false;
        UseHealthChecks = false;
        UseCarter = true;
        UseOpenTelemetry = false;
    }

    public GlobalSettings SetRateLimit()
    {
        this.UseRateLimit = true;

        return this;
    }

    public GlobalSettings SetHealthChecks()
    {
        this.UseHealthChecks = true;
        return this;
    }

    public GlobalSettings UnSetCarter()
    {
        this.UseCarter = false;
        return this;
    }

    public GlobalSettings SetOpenTelemetry()
    {
        this.UseOpenTelemetry = true;
        return this;
    }
}
