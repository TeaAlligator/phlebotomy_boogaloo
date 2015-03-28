using System.Collections.Generic;

namespace Assets.Code.Extensions
{
    public static class ListExtensions
    {
        public static T GetRandomItem<T>(this List<T> input)
        {
            var index = UnityEngine.Random.Range(0, input.Count);
            return input[index > input.Count - 1 ? input.Count - 1 : index];
        }
    }
}
