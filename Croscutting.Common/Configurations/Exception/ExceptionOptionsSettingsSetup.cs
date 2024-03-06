using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Crosscutting.Common.Configurations.Exception;

public class ExceptionOptionsSettingsSetup(IConfiguration configuration) : IConfigureOptions<ExceptionSettingsBinder>
{
    private const string _sectionName = "Exception";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(ExceptionSettingsBinder options)
    {
        _configuration
        .GetSection(_sectionName)
        .Bind(options);
    }
}
