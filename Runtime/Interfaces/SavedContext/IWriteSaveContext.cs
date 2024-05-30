using R3;

namespace SaveSystem
{
    public interface IWriteSaveContext<out T> : ISaveContext<T> where T : ISavedData
    {
        T Default { get; }
        void SetData(ReadOnlyReactiveProperty<ISavedData> data);
    }
}