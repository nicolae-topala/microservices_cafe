using HotChocolate;
using Error = MicroservicesCafe.Shared.BuildingBlocks.Result.Error;

namespace MicroservicesCafe.Shared.BuildingBlocks.GraphQL
{
    public static class GraphQLHelper
    {
        public static IError TranslateError(Error error) =>
            ErrorBuilder.New()
                .SetMessage(error.Message)
                .SetCode(error.Code)
                .Build();
    }
}
