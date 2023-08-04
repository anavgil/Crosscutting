using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Croscutting.Common.Configurations.Exception;

public class ExceptionOptionsSettingsSetup : IConfigureOptions<ExceptionSettingsBinder>
{
    private const string _sectionName = "Exception";
    private readonly IConfiguration _configuration;

    public ExceptionOptionsSettingsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(ExceptionSettingsBinder options)
    {
        _configuration
        .GetSection(_sectionName)
        .Bind(options);
    }
}
