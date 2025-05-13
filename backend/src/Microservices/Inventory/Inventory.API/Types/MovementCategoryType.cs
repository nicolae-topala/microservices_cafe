using MovementTypeDomain = Inventory.Domain.Entities.MovementType;

namespace Inventory.API.Types;

public class MovementCategoryType : ObjectType<MovementTypeDomain>
{
    protected override void Configure(IObjectTypeDescriptor<MovementTypeDomain> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(m => m.Id)
            .Type<NonNullType<IdType>>();

        descriptor.Field(m => m.Name)
            .Type<NonNullType<StringType>>();

        descriptor.Field(m => m.Description)
            .Type<StringType>();
    }
}
