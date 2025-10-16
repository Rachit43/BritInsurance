namespace BritInsurance.Application.Dto
{
    public record UserIdentityDto
    {
        public required string UserName { get; init; }

        public required string AccessToken { get; init; }

        public required string RefreshToken { get; init; }
    }
}
