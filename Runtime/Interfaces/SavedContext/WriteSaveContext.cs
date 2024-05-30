using R3;

namespace SaveSystem
{
    public sealed class WriteSaveContext<T> : IWriteSaveContext<T> where T : class, ISavedData,  new()
    {
        public T Default => new T();
        public ReadOnlyReactiveProperty<ISavedData> LoadingData { get; private set; }
        public void SetData(ReadOnlyReactiveProperty<ISavedData> data)
        {
            LoadingData = data;
        }
    }
}