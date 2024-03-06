using Croscutting.Common.Configurations.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Crosscutting.Common.Configurations.Redis;


public class RedisOptionsSettingsSetup(IConfiguration configuration) : IConfigureOptions<RedisSettingsBinder>
{
    private const string _sectionName = "Redis";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(RedisSettingsBinder options)
    {
        _configuration
        .GetSection(_sectionName)
        .Bind(options);
    }
}
