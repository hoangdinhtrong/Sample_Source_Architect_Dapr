namespace SampeDapr.Application.Shared.Extensions
{
    public static class ObjectCompareExtension
    {
        public static bool DeepCompare(this object? obj, object? another, List<string>? skipProperties)
        {
            if (ReferenceEquals(obj, another)) return true;
            if ((obj is null) || (another is null)) return false;
            if (obj.GetType() != another.GetType()) return false;
            bool result = true;

            foreach (System.Reflection.PropertyInfo property in obj.GetType().GetProperties())
            {
                if (skipProperties?.Count > 0 && CheckInList(property.Name, skipProperties)) continue;
                object? objValue = property.GetValue(obj);
                object? anotherValue = property.GetValue(another);
                if (objValue?.Equals(anotherValue) == false)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        private static bool CheckInList(string skipProperty, List<string>? skipProperties)
        {
            if (skipProperties is null || skipProperties.Count <= 0) return false;
            if (skipProperties.Find(x => x.ToLower().Trim() == skipProperty.ToLower().Trim()) is null) return false;
            return true;
        }
    }
}
