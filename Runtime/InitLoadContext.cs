using System;
using R3;

namespace SaveSystem
{
    public sealed class InitLoadContext<T> : IInitLoadContext<T> where T : class, ISavedData, new()
    {
        private readonly ReactiveCommand<ISavedData> _saveData = new ReactiveCommand<ISavedData>();

        private ReactiveProperty<ISavedData> _loadingData;
        public ReadOnlyReactiveProperty<ISavedData> LoadingData => _loadingData;

        Type IInitLoadContext<T>.ContextType => typeof(T);
        ReactiveCommand<ISavedData> IInitLoadContext<T>.SaveData => _saveData;
        T IInitLoadContext<T>.Default => new T();


        void IInitLoadContext<T>.Init(ReadOnlyReactiveProperty<ISavedData> data)
        {
            _loadingData = (ReactiveProperty<ISavedData>)data;
        }
        public void Save()
        {
            _saveData.Execute(_loadingData.CurrentValue);
        }
    }
}