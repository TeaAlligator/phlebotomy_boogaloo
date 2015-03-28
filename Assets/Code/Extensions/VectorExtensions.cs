using UnityEngine;

namespace Assets.Code.Extensions
{
    public static class VectorConvertor
    {
        public static Vector3 ToWholeVector(this Vector3 input)
        {
            return new Vector3((int)input.x, (int)input.y, (int)input.z);
        }

        public static Vector2 ToWholeVector(this Vector2 input)
        {
            return new Vector2((int)input.x, (int)input.y);
        }
    }
}
