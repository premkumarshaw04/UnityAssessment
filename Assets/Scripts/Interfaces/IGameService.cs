using System.Threading.Tasks;

namespace EndlessRunner.Interfaces
{
    /// <summary>
    /// Contract for all core framework services managed by the ServiceLocator.
    /// </summary>
    public interface IGameService
    {
        /// <summary>
        /// Asynchronously initializes service resources during scene bootstrapping.
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// Cleans up resources when the service is unregistered or destroyed.
        /// </summary>
        void Deinitialize();
    }
}