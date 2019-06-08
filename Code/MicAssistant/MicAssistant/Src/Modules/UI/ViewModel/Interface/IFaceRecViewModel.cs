using MingUnity.MVVM.ViewModel;
using UnityEngine;

namespace MicAssistant
{
    /// <summary>
    /// 人脸识别视图模型接口
    /// </summary>
    public interface IFaceRecViewModel : IViewModel
    {
        /// <summary>
        /// 识别图激活
        /// </summary>
        bool RecActive { get; set; }

        /// <summary>
        /// 识别图不透明度
        /// </summary>
        float RecAlpha { get; set; }

        /// <summary>
        /// 识别图位置
        /// </summary>
        Vector2[] RecPosition { get; set; }

        /// <summary>
        /// 摄像机纹理
        /// </summary>
        Texture CamTexture { get; set; }

        /// <summary>
        /// 摄像头旋转
        /// </summary>
        float CamRotation { get; set; }

        /// <summary>
        /// 摄像头垂直翻转
        /// </summary>
        bool CamVerticalMirror { get; set; }

        /// <summary>
        /// 摄像头水平翻转
        /// </summary>
        bool CamHorizontalMirror { get; set; }

        /// <summary>
        /// 检测激活
        /// </summary>
        bool ScannerActive { get; set; }
    }
}
