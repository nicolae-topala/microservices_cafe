using Price.Domain.Entities;


namespace Price.API.Types;

public class ChannelType : ObjectType<Channel>
{
    protected override void Configure(IObjectTypeDescriptor<Channel> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(c => c.Id)
            .Type<NonNullType<IdType>>();

        descriptor.Field(c => c.Name)
            .Type<NonNullType<StringType>>();

        descriptor.Field(c => c.Description)
            .Type<StringType>();
    }
}
