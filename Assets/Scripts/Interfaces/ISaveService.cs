namespace EndlessRunner.Interfaces
{
    /// <summary>
    /// Contract for persistent data storage operations.
    /// Supports clean testing mocks and hot-swappable save drivers.
    /// </summary>
    public interface ISaveService : IGameService
    {
        /// <summary> Writes data to persistent storage under the designated key. </summary>
        void Save<T>(string key, T data);

        /// <summary> Loads data for a given key, or returns defaultValue if not present. </summary>
        T Load<T>(string key, T defaultValue);

        /// <summary> Checks if a key exists in storage. </summary>
        bool HasKey(string key);

        /// <summary> Flushes cached data directly to persistent storage. </summary>
        void SaveToDisk();
    }
}