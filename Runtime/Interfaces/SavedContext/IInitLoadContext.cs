using System;
using R3;

namespace SaveSystem
{
    public interface IInitLoadContext<out T> : ISaveLoadContext<T> where T : ISavedData
    {
        internal Type ContextType { get; }
        internal ReactiveCommand<ISavedData> SaveData { get; }
        internal T Default { get; }
        internal void Init(ReadOnlyReactiveProperty<ISavedData> data);
    }
}