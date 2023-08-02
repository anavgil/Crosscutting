using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
