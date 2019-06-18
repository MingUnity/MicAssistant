using Ming.EventHub;
using System;

namespace MicAssistant
{
    /// <summary>
    /// 入口
    /// </summary>
    public class Entry : IEventListener
    {
        public Entry()
        {
            MEventHub.Instance.AddListener(LoginEventId.LoginResult, this);
        }

        public void Start()
        {
            MEventHub.Instance.Dispatch(LoginEventId.LoginRequest, MEventArgs.Empty);
        }

        public void HandleEvent(int eventId, IEventArgs args)
        {
            switch (eventId)
            {
                case LoginEventId.LoginResult:
                    HandleLoginResult(args as LoginResultArgs);
                    break;
            }
        }

        /// <summary>
        /// 处理登录结果
        /// </summary>
        private void HandleLoginResult(LoginResultArgs args)
        {
            if (args == null)
            {
                return;
            }

            bool result = args.logined;

            if (result)
            {
                UIManager.Instance.Enter(ViewId.MemoView);
            }
            else
            {
                UIManager.Instance.Enter(ViewId.FaceRecView);
            }
        }
    }
}
