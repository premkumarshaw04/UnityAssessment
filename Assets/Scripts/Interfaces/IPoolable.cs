namespace EndlessRunner.Interfaces
{
    /// <summary>
    /// Contract implemented by any pooled prefab component.
    /// Handles reset and re-initialization hooks upon spawning and recycling.
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// Triggered when the object is retrieved from the pool and activated in the scene.
        /// </summary>
        void OnSpawnFromPool();

        /// <summary>
        /// Triggered when the object is recycled back to the pool stack.
        /// </summary>
        void OnReturnToPool();
    }
}