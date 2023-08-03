namespace Croscutting.Common.Configurations.Redis;
public class RedisSettingsBinder
{
    public bool UseRedis { get; set; }
    public string RedisConnectionString { get; set; }
    public string InstanceName { get; set; }
    public bool UseSSL { get; set; }
    public string RedisToken { get; set; }
}
