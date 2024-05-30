using Cysharp.Threading.Tasks;

namespace SaveSystem
{
    public interface ISaveProvider
    {
        UniTask<bool> TrySave<T>(string key, T obj);

        UniTask<T> Load<T>(string key, T defaultValue = default) where T : ISavedData;
    }
}