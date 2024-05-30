using System.Collections.Generic;

namespace SaveSystem
{
    public class EqualityCompareProvider : IEqualityComparer<ISavedData>
    {
        public bool Equals(ISavedData x, ISavedData y)
        {
            return x != null && x.CompareRelevant(y) == y;
        }

        public int GetHashCode(ISavedData obj)
        {
            return obj.GetHashCode();
        }
    }
}