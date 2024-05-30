using R3;

namespace SaveSystem
{
    public interface ISaveContext<out T>
    {
        ReadOnlyReactiveProperty<ISavedData> LoadingData { get; }
    }
}