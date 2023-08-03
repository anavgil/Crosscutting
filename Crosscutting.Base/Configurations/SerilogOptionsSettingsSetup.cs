using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Crosscutting.Base.Configurations
{
    public class SerilogOptionsSettingsSetup: IConfigureOptions<SerilogSettingsBinder>
    {
        private readonly string _sectionName;
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
}
