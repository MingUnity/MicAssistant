using System.Collections.Generic;
using UnityEngine;

namespace MicAssistant
{
    /// <summary>
    /// 备忘录元素池
    /// </summary>
    public class ItemMemoPool
    {
        private RectTransform _template;

        private Queue<ItemMemo> _items = new Queue<ItemMemo>();

        public ItemMemoPool(RectTransform template)
        {
            this._template = template;
        }

        public ItemMemo GetItem(bool active = true, Transform parent = null)
        {
            ItemMemo item = null;

            if (_items.Count > 0)
            {
                item = _items.Dequeue();
            }
            else
            {
                RectTransform root = RectTransform.Instantiate(_template, parent);

                root.name = "ItemMemo";

                item = new ItemMemo(root);
            }

            item.Active = active;

            return item;
        }

        public void RemoveItem(ItemMemo item)
        {
            item.Dispose();

            item.Active = false;

            _items.Enqueue(item);
        }
    }
}
