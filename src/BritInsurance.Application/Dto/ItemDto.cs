namespace BritInsurance.Application.Dto
{
    public abstract record BaseItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public record GetItemDto : BaseItemDto
    {
        public int Id { get; set; }
    }

    public record UpdateItemDto : BaseItemDto
    {
    }

    public record CreateItemDto : BaseItemDto
    {
    }

    public record CreateItemFromProductDto
    {
        public int Quantity { get; set; }
    }

    public record UpdateItemFromProductDto
    {
        public int? Id { get; set; }

        public int Quantity { get; set; }
    }
}