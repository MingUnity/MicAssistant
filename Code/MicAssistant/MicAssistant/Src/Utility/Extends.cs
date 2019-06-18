using System.Collections.Generic;

namespace MicAssistant
{
    /// <summary>
    /// 扩展
    /// </summary>
    public static class Extends
    {
        /// <summary>
        /// 尝试从列表中移除对象
        /// </summary>
        public static bool TryRemove<T>(this List<T> list, int index)
        {
            bool result = false;

            if (index >= 0 && list != null && list.Count > index)
            {
                list.RemoveAt(index);

                result = true;
            }

            return result;
        }

        /// <summary>
        /// 尝试从列表中获取对象
        /// </summary>
        public static bool TryGetValue<T>(this List<T> list, int index, out T t)
        {
            bool result = false;

            if (index >= 0 && list != null && list.Count > index)
            {
                t = list[index];

                result = true;
            }
            else
            {
                t = default(T);
            }

            return result;
        }
    }
}
