using MingUnity.MVVM.ViewModel;
using System.Collections.Generic;

namespace MicAssistant
{
    public interface IMemoViewModel : IViewModel
    {
        /// <summary>
        /// 视图不透明度
        /// </summary>
        float ViewAlpha { get; set; }

        /// <summary>
        /// 备忘录列表
        /// </summary>
        List<string> MemoList { get; set; }

        /// <summary>
        /// 删除元素索引
        /// </summary>
        int DeleteItemIndex { get; }

        /// <summary>
        /// 添加元素内容
        /// </summary>
        string AddItemContent { get; }

        /// <summary>
        /// 删除元素
        /// </summary>
        void DeleteItem(int index);

        /// <summary>
        /// 添加元素
        /// </summary>
        void AddItem(string content);

        /// <summary>
        /// 按下麦克风
        /// </summary>
        void MicrophonePointerDown();

        /// <summary>
        /// 抬起麦克风
        /// </summary>
        void MicrophonePointerUp();
    }
}
