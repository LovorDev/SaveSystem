namespace SaveSystem
{
    public static class Extension
    {
        public static void Save<T>(this T savedData, ISaveLoadContext<T> saveLoadContext) where T : ISavedData
        {
            saveLoadContext.Save();
        }
    }
}