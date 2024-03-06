using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Crosscutting.Common.Configurations.Serilog;

public class SerilogOptionsSettingsSetup(IConfiguration configuration) : IConfigureOptions<SerilogSettingsBinder>
{
    private const string _sectionName = "Serilog";

    public void Configure(SerilogSettingsBinder options)
    {
        configuration
            .GetSection(_sectionName)
            .Bind(options);
    }
}
