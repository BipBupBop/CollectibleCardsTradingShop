namespace CollectibleCardsTradingShopProject.Middleware
{
    public static class DbInitializerExtensions
    {
        public static IApplicationBuilder UseDbInitializer(this IApplicationBuilder app)
        {
            return app.UseMiddleware<DbInitializerMiddleware>();
        }
    }
}
