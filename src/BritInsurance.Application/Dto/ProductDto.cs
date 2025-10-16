using System.Text.Json.Serialization;

namespace BritInsurance.Application.Dto
{
    public abstract record BaseProductDto
    {
        public required string ProductName { get; set; }
    }

    public record GetProductDto : BaseProductDto
    {
        public int Id { get; set; }

        public required string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public required GetItemDto[] Items { get; set; }
    }

    public record CreateProductDto : BaseProductDto
    {
        public required CreateItemFromProductDto[] Items { get; set; }
    }

    public record UpdateProductDto : BaseProductDto
    {
        [JsonIgnore]
        public bool IgnoreItems { get; set; }

        public UpdateItemFromProductDto[] Items { get; set; } = [];
    }
}
