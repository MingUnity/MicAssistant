using MingUnity.MVVM.Model;
using System.Collections.Generic;
using System;

namespace MicAssistant
{
    /// <summary>
    /// 备忘录数据
    /// </summary>
    [Serializable]
    public class MemoModel:IModel
    {
        public List<string> memoContentList = new List<string>();
    }
}
