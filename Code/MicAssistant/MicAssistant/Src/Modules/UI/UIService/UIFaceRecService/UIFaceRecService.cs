using MFramework.UI;
using Ming.FSM;
using System;
using UnityEngine;

namespace MicAssistant
{
    public class UIFaceRecService : MFUIServiceBase<FaceRecView, FaceRecViewModel>
    {
        /// <summary>
        /// 状态机
        /// </summary>
        private IFSMSystem _fsmSystem;

        protected override void OnCreateCompleted()
        {
            if (_viewModel != null)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    _viewModel.CamRotation = 90;

                    _viewModel.CamVerticalMirror = true;
                }
                else
                {
                    _viewModel.CamRotation = 0;

                    _viewModel.CamHorizontalMirror = true;
                }
            }
        }

        protected override void OnEnterStart()
        {
            if (_fsmSystem == null)
            {
                GenerateFSM();
            }

            _fsmSystem?.SetTransition((int)UIFaceRecTransition.Default);
        }

        protected override void EnterAnim(Action callback)
        {
            callback?.Invoke();
        }

        protected override void QuitAnim(Action callback)
        {
            callback?.Invoke();
        }

        /// <summary>
        /// 构建有限状态机
        /// </summary>
        private void GenerateFSM()
        {
            _fsmSystem = new FSMSystem();

            IFSMState defaultState = new FaceRecDefaultState(_fsmSystem, _viewModel);

            IFSMState detectState = new FaceDetectState(_fsmSystem, _viewModel);

            IFSMState analyzeState = new FaceAnalyzeState(_fsmSystem, _viewModel);

            _fsmSystem.AddState(defaultState, true);

            _fsmSystem.AddState(detectState);

            _fsmSystem.AddState(analyzeState);

            defaultState[(int)UIFaceRecTransition.Detect] = detectState;

            detectState[(int)UIFaceRecTransition.Default] = defaultState;

            detectState[(int)UIFaceRecTransition.Analyze] = analyzeState;

            analyzeState[(int)UIFaceRecTransition.Default] = defaultState;
        }
    }

    /// <summary>
    /// 面部识别界面过渡条件
    /// </summary>
    public enum UIFaceRecTransition
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default,

        /// <summary>
        /// 检测
        /// </summary>
        Detect,

        /// <summary>
        /// 分析
        /// </summary>
        Analyze
    }
}
