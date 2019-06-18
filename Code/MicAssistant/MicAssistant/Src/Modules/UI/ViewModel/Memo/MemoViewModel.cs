using System;
using System.Collections.Generic;
using MingUnity.MVVM.ViewModel;

namespace MicAssistant
{
    /// <summary>
    /// 备忘录视图模型
    /// </summary>
    public class MemoViewModel : ViewModelBase<MemoModel>, IMemoViewModel
    {
        private bool _active;

        private float _viewAlpha;

        private int _deleteItem;

        private string _addItem;

        public event Action OnStartRecordEvent;

        public event Action OnEndRecordEvent;

        public event Action OnDeleteItemEvent;

        public event Action OnAddItemEvent;

        public override bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;

                RaisePropertyChanged("Active");
            }
        }

        public float ViewAlpha
        {
            get
            {
                return _viewAlpha;
            }
            set
            {
                _viewAlpha = value;

                RaisePropertyChanged("ViewAlpha");
            }
        }

        public List<string> MemoList
        {
            get
            {
                List<string> res = null;

                if (_model != null)
                {
                    res = _model.memoContentList;
                }

                return res;
            }
            set
            {
                if (_model != null)
                {
                    _model.memoContentList = value;
                }

                RaisePropertyChanged("MemoContents");
            }
        }

        public int DeleteItemIndex
        {
            get
            {
                return _deleteItem;
            }
            private set
            {
                _deleteItem = value;

                RaisePropertyChanged("DeleteItemIndex");
            }
        }

        public string AddItemContent
        {
            get
            {
                return _addItem;
            }
            set
            {
                _addItem = value;

                RaisePropertyChanged("AddItemContent");
            }
        }

        public void DeleteItem(int index)
        {
            DeleteItemIndex = index;

            if (_model != null)
            {
                _model.memoContentList.TryRemove(index);
            }

            OnDeleteItemEvent?.Invoke();
        }

        public void MicrophonePointerDown()
        {
            OnStartRecordEvent?.Invoke();
        }

        public void MicrophonePointerUp()
        {
            OnEndRecordEvent?.Invoke();
        }

        public override void Setup()
        {
            Active = _active;

            MemoList = MemoList;
        }

        /// <summary>
        /// 添加备忘录
        /// </summary>
        public void AddItem(string content)
        {
            AddItemContent = content;

            if (_model != null)
            {
                _model.memoContentList.Add(content);
            }

            OnAddItemEvent?.Invoke();
        }
    }
}
