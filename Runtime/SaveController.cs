using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;

namespace SaveSystem
{
    public class SaveController
    {
        private readonly IEnumerable<ISaveProvider> _saveProviders;
        private readonly IEnumerable<IWriteSaveContext<ISavedData>> _saveSystems;

        private Dictionary<IWriteSaveContext<ISavedData>, string> _savingKeys;

        public SaveController(IEnumerable<IWriteSaveContext<ISavedData>> saveSystems, IEnumerable<ISaveProvider> saveProviders)
        {
            _saveSystems = saveSystems;
            _saveProviders = saveProviders;

            var equalityCompareProvider = new EqualityCompareProvider();

            foreach (var saveSystem in _saveSystems)
            {
                saveSystem.SetData(new ReactiveProperty<ISavedData>(saveSystem.Default, equalityCompareProvider));
            }
        }

        public void Initialize()
        {
            _savingKeys = _saveSystems.ToDictionary(key => key,
                value => value.LoadingData.GetType().GetGenericArguments().First().FullName);
            LoadData();
        }
        private void LoadData()
        {
            foreach (var provider in _saveProviders)
            {
                foreach (var saveSystem in _saveSystems)
                {
                    provider.Load(_savingKeys[saveSystem], saveSystem.Default).ContinueWith(data => OnDataLoaded(saveSystem, data));
                }
            }
        }
        private void OnDataLoaded(IWriteSaveContext<ISavedData> saveContext, ISavedData newData)
        {
            ((ReactiveProperty<ISavedData>)saveContext.LoadingData).Value = newData;
        }
    }
}