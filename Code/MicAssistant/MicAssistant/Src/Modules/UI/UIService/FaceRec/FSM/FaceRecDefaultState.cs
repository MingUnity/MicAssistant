using Ming.FSM;
using MingUnity.InputModule;
using MingUnity.WebCamera;

namespace MicAssistant
{
    /// <summary>
    /// 面部识别默认状态
    /// </summary>
    public class FaceRecDefaultState : FSMState
    {
        /// <summary>
        /// 摄像头宽度
        /// </summary>
        private readonly int CamWidth = 1280;

        /// <summary>
        /// 摄像头高度
        /// </summary>
        private readonly int CamHeight = 720;

        /// <summary>
        /// 视图模型
        /// </summary>
        private IFaceRecViewModel _viewModel;

        /// <summary>
        /// 状态机系统
        /// </summary>
        private IFSMSystem _fsmSystem;

        /// <summary>
        /// 网络摄像头
        /// </summary>
        private WebCamera _webCam;

        public FaceRecDefaultState(IFSMSystem fsmSystem, IFaceRecViewModel viewModel)
        {
            this._viewModel = viewModel;

            this._fsmSystem = fsmSystem;

            _webCam = WebCamera.GetFrontFacing();
        }

        public override void OnEnter(params object[] keys)
        {
            _webCam?.Open(CamWidth, CamHeight);

            if (_viewModel != null)
            {
                _viewModel.CamTexture = _webCam?.CamTexture;

                _viewModel.RecActive = false;

                _viewModel.ScannerActive = false;
            }

            MInput.OnSimpleTap += MInput_OnSimpleTap;
        }
        
        public override void OnExit()
        {
            MInput.OnSimpleTap -= MInput_OnSimpleTap;
        }

        public override void OnStay()
        {

        }

        private void MInput_OnSimpleTap(MGesture gesture)
        {
            _fsmSystem?.SetTransition((int)UIFaceRecTransition.Detect);
        }
    }
}
