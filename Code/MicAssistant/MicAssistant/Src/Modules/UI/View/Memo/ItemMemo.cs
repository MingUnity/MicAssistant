using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MicAssistant
{
    public class ItemMemo : IDisposable
    {
        private RectTransform _root;

        private Text _txtMemo;

        private Button _btnDelete;

        private Vector2 _originSize;

        private Tween _tweenExpands;

        private Tween _tweenShrink;

        private readonly float TweenDuration = 1;

        public event Action OnDeleteButtonClickEvent;

        public string Content
        {
            get
            {
                return _txtMemo.text;
            }
            set
            {
                _txtMemo.text = value;
            }
        }

        public bool Active
        {
            get
            {
                return _root.gameObject.activeSelf;
            }
            set
            {
                _root.gameObject.SetActive(value);
            }
        }

        public ItemMemo(RectTransform root)
        {
            this._root = root;

            this._txtMemo = _root.Find("TxtMemo").GetComponent<Text>();

            this._btnDelete = _root.Find("BtnDelete").GetComponent<Button>();

            _btnDelete.onClick.AddListener(OnDeleteButtonClick);

            _originSize = _root.sizeDelta;
        }

        /// <summary>
        /// 扩展
        /// </summary>
        public void Expands(Action callback = null)
        {
            Vector2 size = _root.sizeDelta;

            _tweenExpands = DOTween.To(() => new Vector2(0, size.y), x => _root.sizeDelta = x, _originSize, TweenDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() => callback?.Invoke());
        }

        /// <summary>
        /// 收缩
        /// </summary>
        public void Shrink(Action callback = null)
        {
            Vector2 size = _root.sizeDelta;

            _tweenExpands = DOTween.To(() => size, x => _root.sizeDelta = x, new Vector2(0, _originSize.y), TweenDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() => callback?.Invoke());
        }

        public void SetAsLastSibling()
        {
            _root.SetAsLastSibling();
        }

        public void Dispose()
        {
            OnDeleteButtonClickEvent = null;
        }

        private void OnDeleteButtonClick()
        {
            OnDeleteButtonClickEvent?.Invoke();
        }
    }
}
