using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;

namespace SaveSystem
{
    public class SaveController : IDisposable
    {
        private readonly IEnumerable<ISaveProvider> _saveProviders;
        private readonly IEnumerable<IInitLoadContext<ISavedData>> _saveSystems;

        private readonly IDisposable _saveSubscription;

        public SaveController(IEnumerable<IInitLoadContext<ISavedData>> saveSystems, IEnumerable<ISaveProvider> saveProviders)
        {
            _saveSystems = saveSystems;
            _saveProviders = saveProviders;

            var equalityCompareProvider = new EqualityCompareProvider();

            foreach (var saveSystem in _saveSystems)
            {
                saveSystem.Init(new ReactiveProperty<ISavedData>(saveSystem.Default, equalityCompareProvider));
                _saveSubscription = saveSystem.SaveData.Subscribe(SaveData);
            }
        }

        public void Initialize()
        {
            LoadData();
        }
        private void LoadData()
        {
            foreach (var provider in _saveProviders)
            {
                foreach (var saveSystem in _saveSystems)
                {
                    provider.Load(saveSystem.ContextType.ToString(), saveSystem.Default).ContinueWith(data => OnDataLoaded(saveSystem, data));
                }
            }
        }
        private void SaveData(ISavedData obj)
        {
            foreach (var provider in _saveProviders)
            {
                provider.TrySave(obj.GetType().ToString(), obj).Forget();
            }
        }

        private void OnDataLoaded(IInitLoadContext<ISavedData> loadContext, ISavedData newData)
        {
            ((ReactiveProperty<ISavedData>)loadContext.LoadingData).Value = newData;
        }
        public void Dispose()
        {
            _saveSubscription?.Dispose();
        }
    }
}