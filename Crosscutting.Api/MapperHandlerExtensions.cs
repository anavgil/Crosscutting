using AutoMapper;

namespace Crosscutting.Api;
public class MapperHandlerExtensions
{
    #region Members

    private static IMapper _mapper;

    #endregion

    #region Constructor

    public MapperHandlerExtensions(IMapper mapper)
    {
        _mapper = mapper;
    }

    #endregion

    #region Public Methods

    public static void ConfigureAutoMapper<TSource, TDestination>()
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.CreateMap<TSource, TDestination>();
        });

        _mapper = mappingConfig.CreateMapper();
    }

    public static IMapper GetMapper()
    {
        return _mapper;
    }

    #endregion
}
