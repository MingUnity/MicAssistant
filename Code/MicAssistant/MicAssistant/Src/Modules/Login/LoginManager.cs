using Ming.EventHub;
using System;
using System.IO;
using UnityEngine;

namespace MicAssistant
{
    /// <summary>
    /// 登录管理
    /// </summary>
    public class LoginManager : IEventListener
    {
        private string _configPath;

        public LoginManager()
        {
            _configPath = string.Format("{0}/Login.mdat", Application.persistentDataPath);
        }

        public void HandleEvent(int eventId, IEventArgs args)
        {
            switch (eventId)
            {
                case LoginEventId.LoginRequest:
                    HandleLoginRequest();
                    break;

                case LoginEventId.ChangeLogin:
                    HandleLoginChange(args as ChangeLoginStatusArgs);
                    break;
            }
        }

        /// <summary>
        /// 处理登录请求
        /// </summary>
        private void HandleLoginRequest()
        {
            if (File.Exists(_configPath))
            {
                string configContent = File.ReadAllText(_configPath);

                MEventHub.Instance.Dispatch(LoginEventId.LoginResult, new LoginResultArgs() { logined = GetDateValue() == configContent });
            }
        }

        /// <summary>
        /// 处理改变登录状态
        /// </summary>
        private void HandleLoginChange(ChangeLoginStatusArgs args)
        {
            if (args == null)
            {
                return;
            }

            if (args.login)
            {
                string dir = Path.GetDirectoryName(_configPath);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllText(_configPath, GetDateValue());
            }
            else
            {
                if (File.Exists(_configPath))
                {
                    File.Delete(_configPath);
                }
            }
        }

        private string GetDateValue()
        {
            DateTime dateTime = DateTime.Now;

            string value = string.Format("{0}_{1}_{2}", dateTime.Year, dateTime.Month, dateTime.Day);

            return value;
        }
    }

    /// <summary>
    /// 登录结果参数
    /// </summary>
    public class LoginResultArgs : IEventArgs
    {
        /// <summary>
        /// 已登录过
        /// </summary>
        public bool logined;
    }

    /// <summary>
    /// 改变登录状态参数
    /// </summary>
    public class ChangeLoginStatusArgs : IEventArgs
    {
        public bool login;
    }
}
