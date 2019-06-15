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
