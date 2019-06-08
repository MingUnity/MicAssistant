using System;
using MFramework.UI;
using MingUnity.Common;

namespace MicAssistant
{
    public class UIManager : Singleton<UIManager>, IUIModule
    {
        private IUIModule _uiModule;

        public IUIModule UIModule
        {
            get
            {
                return _uiModule;
            }
            set
            {
                _uiModule = value;
            }
        }

        public void Enter(int viewId, bool anim = false, Action callback = null)
        {
            _uiModule?.Enter(viewId, anim, callback);
        }

        public void Quit(int viewId, bool anim = false, Action callback = null)
        {
            _uiModule?.Quit(viewId, anim, callback);
        }
    }
}
