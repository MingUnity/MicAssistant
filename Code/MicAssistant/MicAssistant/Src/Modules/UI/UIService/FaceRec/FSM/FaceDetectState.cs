using Ming.EventHub;
using Ming.FSM;
using MingUnity.WebCamera;
using UnityEngine;
using System.Collections;
using Face = MingUnity.FaceRec.FaceDetectRes.Result.Face;
using System;
using MingUnity.Common;

namespace MicAssistant
{
    /// <summary>
    /// 检测状态
    /// </summary>
    public class FaceDetectState : FSMState, IEventListener
    {
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

        /// <summary>
        /// 面部识别结果
        /// </summary>
        private Face _face;

        /// <summary>
        /// 缓存纹理
        /// </summary>
        private Texture2D _cacheTex;

        public FaceDetectState(IFSMSystem fsmSystem, IFaceRecViewModel viewModel)
        {
            this._fsmSystem = fsmSystem;

            this._viewModel = viewModel;

            _webCam = WebCamera.GetFrontFacing();
        }

        public override void OnEnter(params object[] keys)
        {
            _face = null;

            MEventHub.Instance.AddListener(FaceRecEventId.FaceDetectResult, this);

            MEventHub.Instance.AddListener(FaceRecEventId.FaceSearchResult, this);

            if (_viewModel != null)
            {
                _viewModel.ScannerActive = true;

                _webCam?.Pause();

                _webCam?.Snapshot((Texture2D tex) =>
                {
                    _viewModel.CamTexture = tex;
                });
            }

            Task.CreateTask(SnapshotScreen((tex) =>
            {
                Task.CreateTask(DelayHandleFace(tex));

                MEventHub.Instance.Dispatch(FaceRecEventId.FaceDetectRequest, new FaceRecRequestArgs()
                {
                    tex = tex
                });
            }));
        }

        public override void OnExit()
        {
            _face = null;

            MEventHub.Instance.RemoveListener(FaceRecEventId.FaceDetectResult, this);

            if (_viewModel != null)
            {
                _viewModel.ScannerActive = false;
            }
        }

        public override void OnStay()
        {

        }

        public void HandleEvent(int eventId, IEventArgs args)
        {
            switch (eventId)
            {
                case FaceRecEventId.FaceDetectResult:
                    HandleDetectResult(args as FaceDetectResultArgs);
                    break;
            }
        }

        /// <summary>
        /// 延迟处理面部结果
        /// </summary>
        private IEnumerator DelayHandleFace(Texture2D faceTex)
        {
            yield return new WaitForSeconds(4);

            if (_face != null)
            {
                _webCam?.Close();

                _fsmSystem?.SetTransition((int)UIFaceRecTransition.Analyze, _face, faceTex);
            }
            else
            {
                _fsmSystem?.SetTransition((int)UIFaceRecTransition.Default);
            }
        }

        /// <summary>
        /// 截屏
        /// </summary>
        private IEnumerator SnapshotScreen(Action<Texture2D> callback)
        {
            yield return new WaitForEndOfFrame();

            int width = Screen.width;

            int height = Screen.height;

            if (_cacheTex == null)
            {
                _cacheTex = new Texture2D(width, height);
            }
            else
            {
                if (width != _cacheTex.width || height != _cacheTex.height)
                {
                    _cacheTex.Resize(width, height);
                }
            }

            _cacheTex.ReadPixels(new Rect(0, 0, width, height), 0, 0);

            _cacheTex.Apply();

            callback?.Invoke(_cacheTex);
        }

        /// <summary>
        /// 处理检测结果
        /// </summary>
        private void HandleDetectResult(FaceDetectResultArgs args)
        {
            if (args.res != null && args.res.result != null && args.res.result.face_num > 0)
            {
                _face = args.res.result.face_list[0];
            }
        }
    }
}
