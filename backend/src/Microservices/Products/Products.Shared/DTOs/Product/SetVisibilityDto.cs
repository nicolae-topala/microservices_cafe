namespace Products.Shared.DTOs.Product;

public record SetVisibilityDto(List<Guid> ProductIds, bool SetIsVisible);