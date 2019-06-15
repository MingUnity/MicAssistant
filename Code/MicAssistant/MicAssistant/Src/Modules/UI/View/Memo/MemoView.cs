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
                    ItemMemo item = CreateItem(memoList[i]);

                    int index = i;

                    item.OnDeleteButtonClickEvent += () =>
                    {
                        _viewModel.DeleteItem(index);
                    };

                    _itemMemoList.Add(item);
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

        private ItemMemo CreateItem(string content)
        {
            ItemMemo itemMemo = _itemPool.GetItem(_scrollMemo.content);

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
    }
}
