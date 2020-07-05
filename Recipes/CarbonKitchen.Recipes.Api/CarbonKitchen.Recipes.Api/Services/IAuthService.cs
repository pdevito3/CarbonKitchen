namespace CarbonKitchen.Recipes.Api.Services
{
    using CarbonKitchen.Recipes.Api.Data;

    public interface IAuthService
    {
        SecurityToken Authenticate(string key);
    }
}
