using Codes.ServiceModules.GameService.Provider;

namespace Codes.ServiceModules.GameService
{
    internal static class GameServiceFactory
    {
        public static IGameServiceProvider CreateGameServiceProvider()
        {
            #if HMS_BUILD
                return new HMSGameServiceProvider();
            #else
                return new GooglePlayGameServiceProvider();                        
            #endif
        }
    }
}