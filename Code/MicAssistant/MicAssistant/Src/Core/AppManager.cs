using MFramework.UI;
using Ming.EventHub;
using MingUnity.Common;
using MingUnity.InputModule;
using MingUnity.VoiceInput;
using MingUnity.WebCamera;
using UnityEngine;

namespace MicAssistant
{
    public class AppManager : MonoBehaviour
    {
        private void Start()
        {
            Loom.Initialize();

            WebCamera.Initialize();

            VoiceReceiver.Initialize();

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

            GenerateSpeechRecModule();

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

            MEventHub.Instance.AddListener(FaceRecEventId.FaceDetectRequest, faceRecService);

            MEventHub.Instance.AddListener(FaceRecEventId.FaceSearchRequest, faceRecService);
        }

        private void GenerateSpeechRecModule()
        {
            SpeechRecManager speechRecManager = new SpeechRecManager();

            MEventHub.Instance.AddListener(SpeechRecEventId.SpeechRecRequest, speechRecManager);
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
            UIManager.Instance.Enter(ViewId.FaceRecView);
        }
    }
}
