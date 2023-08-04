using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Croscutting.Common.Configurations.Serilog;

public class SerilogOptionsSettingsSetup : IConfigureOptions<SerilogSettingsBinder>
{
    private const string _sectionName = "Serilog";
    private readonly IConfiguration _configuration;

    public SerilogOptionsSettingsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(SerilogSettingsBinder options)
    {
        _configuration
            .GetSection(_sectionName)
            .Bind(options);
    }
}
