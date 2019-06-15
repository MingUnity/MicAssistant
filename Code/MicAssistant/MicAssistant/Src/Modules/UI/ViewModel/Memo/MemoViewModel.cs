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

        public event Action OnStartRecordEvent;

        public event Action OnEndRecordEvent;

        public event Action<int> OnDeleteItemEvent;

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

        public void DeleteItem(int index)
        {
            OnDeleteItemEvent?.Invoke(index);
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
    }
}
