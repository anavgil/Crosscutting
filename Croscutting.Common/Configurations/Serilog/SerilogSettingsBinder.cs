namespace Croscutting.Common.Configurations.Serilog;

public class SerilogSettingsBinder
{
    public LogLevelNode LogLevel { get; set; }
}

public class LogLevelNode
{
    public string Default { get; set; }
    public string Microsoft { get; set; }
}
