namespace SaveSystem
{
    public interface ISavedData
    {
        /// <summary>
        ///     Method must compare data with new data
        /// </summary>
        /// <param name="other">New data</param>
        /// <returns>More relevant data you need</returns>
        ISavedData CompareRelevant(ISavedData other);
    }
}