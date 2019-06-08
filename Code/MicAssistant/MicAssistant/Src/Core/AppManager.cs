using MFramework.UI;
using MingUnity.Common;
using MingUnity.InputModule;
using UnityEngine;

namespace MicAssistant
{
    public class AppManager : MonoBehaviour
    {
        private void Start()
        {
            Loom.Initialize();

            GenerateModules();

            Entry();
        }

        /// <summary>
        /// 构建各模块
        /// </summary>
        private void GenerateModules()
        {
            GenerateInputModule();

            GenerateFaceRecModule();

            GenerateUIModule();
        }

        /// <summary>
        /// 构建输入模块
        /// </summary>
        private void GenerateInputModule()
        {
            MInput.Instance.enable = true;

            MInput.Instance.allowUIDetection = false;
        }

        /// <summary>
        /// 构建人脸识别模块
        /// </summary>
        private void GenerateFaceRecModule()
        {
            FaceRecManager faceRecService = new FaceRecManager();
        }

        /// <summary>
        /// 构建UI模块
        /// </summary>
        private void GenerateUIModule()
        {
            UIManager.Instance.UIModule = new SimpleUIModule(new UIServiceFactory());
        }

        /// <summary>
        /// 进入
        /// </summary>
        private void Entry()
        {
            UIManager.Instance.Enter((int)ViewId.FaceRecView);
        }
    }
}
