using Mapster;
using Shared.BuildingBlocks.Result;
using Shared.Models;

namespace Shared.Mappings;

public abstract class ResultResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .ForType(typeof(Result<>), typeof(Response<>))
            .Map("IsSuccessful", "IsSuccess")
            .Map("Payload", "Value")
            .Map("ErrorCode", "Error.Code")
            .Map("ErrorMessage", "Error.Message");
    }
}