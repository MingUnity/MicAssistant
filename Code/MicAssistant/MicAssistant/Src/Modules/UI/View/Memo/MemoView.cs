using MFramework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MicAssistant
{
    /// <summary>
    /// 备忘录视图
    /// </summary>
    public class MemoView : MFViewBase<IMemoViewModel>
    {
        private CanvasGroup _rootCanvas;

        private List<ItemMemo> _itemMemoList = new List<ItemMemo>();

        private ItemMemoPool _itemPool;

        private ScrollRect _scrollMemo;

        private PointerDownUpListener _btnMicrophone;

        private bool Active
        {
            get
            {
                return _rootCanvas.alpha > 0;
            }
            set
            {
                _rootCanvas.alpha = value ? 1 : 0;

                _rootCanvas.blocksRaycasts = value;
            }
        }

        private float RootAlpha
        {
            get
            {
                return _rootCanvas.alpha;
            }
            set
            {
                _rootCanvas.alpha = value;
            }
        }

        protected override void OnCreate()
        {
            _itemPool = new ItemMemoPool(_root.Find("ScrollMemo/Viewport/Content/ItemMemoTemplate").GetComponent<RectTransform>());

            _rootCanvas = _root.GetComponent<CanvasGroup>();

            _scrollMemo = _root.Find("ScrollMemo").GetComponent<ScrollRect>();

            _btnMicrophone = _root.Find("BtnMicrophone").gameObject.AddComponent<PointerDownUpListener>();

            _btnMicrophone.OnPointerDownEvent += OnMicrophonePointerDownEvent;

            _btnMicrophone.OnPointerUpEvent += OnMicrophonePointerUpEvent;
        }

        protected override void PropertyChanged(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                switch (propertyName)
                {
                    case "Active":
                        Active = _viewModel.Active;
                        break;

                    case "ViewAlpha":
                        RootAlpha = _viewModel.ViewAlpha;
                        break;

                    case "MemoContents":
                        RefreshContentList(_viewModel.MemoList);
                        break;

                    case "DeleteItemIndex":
                        RemoveItem(_viewModel.DeleteItemIndex);
                        break;

                    case "AddItemContent":
                        AddItem(_viewModel.AddItemContent);
                        break;
                }
            }
        }

        private void RefreshContentList(List<string> memoList)
        {
            if (memoList != null)
            {
                ClearAllItem();

                for (int i = 0; i < memoList.Count; i++)
                {
                    CreateItem(memoList[i]);
                }
            }
        }

        private void ClearAllItem()
        {
            for (int i = 0; i < _itemMemoList.Count; i++)
            {
                _itemPool.RemoveItem(_itemMemoList[i]);
            }

            _itemMemoList.Clear();
        }

        private ItemMemo CreateItem(string content, bool active = true)
        {
            ItemMemo itemMemo = _itemPool.GetItem(active, _scrollMemo.content);

            _itemMemoList.Add(itemMemo);

            itemMemo.OnDeleteButtonClickEvent += () =>
            {
                _viewModel.DeleteItem(_itemMemoList.IndexOf(itemMemo));
            };

            itemMemo.SetAsLastSibling();

            itemMemo.Content = content;

            return itemMemo;
        }

        private void OnMicrophonePointerDownEvent()
        {
            _viewModel?.MicrophonePointerDown();
        }

        private void OnMicrophonePointerUpEvent()
        {
            _viewModel?.MicrophonePointerUp();
        }

        private void RemoveItem(int index)
        {
            ItemMemo item = null;

            if (_itemMemoList.TryGetValue(index, out item))
            {
                _itemMemoList.TryRemove(index);

                item?.Shrink(() =>
                {
                    _itemPool.RemoveItem(item);
                });
            }
        }

        private void AddItem(string content)
        {
            ItemMemo item = CreateItem(content);

            item?.Expands();
        }
    }
}
