using R3;

namespace SaveSystem
{
    public interface ILoadContext<out T>
    {
        ReadOnlyReactiveProperty<ISavedData> LoadingData { get; }
    }

    public interface ISaveContext<out T>
    {
        void Save();
    }

    public interface ISaveLoadContext<out T> : ISaveContext<T>, ILoadContext<T> { }
}