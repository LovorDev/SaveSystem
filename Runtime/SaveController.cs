using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;

namespace SaveSystem
{
    public class SaveController
    {
        private readonly IEnumerable<ISaveProvider> _saveProviders;
        private readonly IEnumerable<IInitLoadContext<ISavedData>> _saveSystems;

        private Dictionary<Type, string> _savingKeys;

        public SaveController(IEnumerable<IInitLoadContext<ISavedData>> saveSystems, IEnumerable<ISaveProvider> saveProviders)
        {
            _saveSystems = saveSystems;
            _saveProviders = saveProviders;

            var equalityCompareProvider = new EqualityCompareProvider();

            foreach (var saveSystem in _saveSystems)
            {
                saveSystem.Init(new ReactiveProperty<ISavedData>(saveSystem.Default, equalityCompareProvider));
                saveSystem.SaveData.Subscribe(SaveData);
            }
        }

        public void Initialize()
        {
            _savingKeys = _saveSystems.ToDictionary(key => key.ContextType,
                value => value.LoadingData.GetType().GetGenericArguments().First().FullName);
            LoadData();
        }
        private void LoadData()
        {
            foreach (var provider in _saveProviders)
            {
                foreach (var saveSystem in _saveSystems)
                {
                    provider.Load(_savingKeys[saveSystem.ContextType], saveSystem.Default).ContinueWith(data => OnDataLoaded(saveSystem, data));
                }
            }
        }
        private void SaveData(ISavedData obj)
        {
            foreach (var provider in _saveProviders)
            {
                provider.TrySave(_savingKeys[obj.GetType()], obj).Forget();
            }
        }

        private void OnDataLoaded(IInitLoadContext<ISavedData> loadContext, ISavedData newData)
        {
            ((ReactiveProperty<ISavedData>)loadContext.LoadingData).Value = newData;
        }
    }
}