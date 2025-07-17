using DataLib.Resources;
using Microsoft.Extensions.Localization;


namespace CommonLib.Localization
{
    public static class LocalizedErrorHelper
    {
        private static IStringLocalizer<SharedResource>? _localizer;

        public static void Configure(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public static ErrorResponse Create(ErrorCode code, string key, params object[] args)
        {
            if (_localizer == null)
                throw new InvalidOperationException("LocalizedErrorHelper not initialized. Call Configure() first.");

            var message = _localizer[key, args];
            return new ErrorResponse
            {
                ErrorCode = code,
                ErrorMessage = message
            };
        }

    }
}
