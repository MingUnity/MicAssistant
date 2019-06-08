using Ming.FSM;
using UnityEngine;
using System.Collections;
using Face = MingUnity.FaceRec.FaceDetectRes.Result.Face;
using DG.Tweening;
using MFramework.UI;
using Ming.EventHub;
using MingUnity.Common;

namespace MicAssistant
{
    /// <summary>
    /// 面部分析状态
    /// </summary>
    public class FaceAnalyzeState : FSMState
    {
        private IFaceRecViewModel _viewModel;

        private IFSMSystem _fsmSystem;

        private Tween _fadeoutTween;

        private Face _face;

        public FaceAnalyzeState(IFSMSystem fsmSystem, IFaceRecViewModel viewModel)
        {
            this._fsmSystem = fsmSystem;

            this._viewModel = viewModel;
        }

        public override void OnEnter(params object[] keys)
        {
            if (keys != null && keys.Length > 0)
            {
                _face = keys[0] as Face;

                if (_face != null)
                {
                    Vector2[] landmark = _face.landmark;

                    if (landmark != null && landmark.Length >= 4)
                    {
                        if (_viewModel != null)
                        {
                            _viewModel.RecActive = true;

                            _viewModel.RecPosition = new Vector2[]
                            {
                                ModifyPosY(landmark[1]),

                                ModifyPosY(landmark[2]),

                                ModifyPosY(landmark[3])
                            };
                        }
                    }
                }
            }

            Task.CreateTask(DelayTurnView());
        }

        public override void OnExit()
        {
            if (_fadeoutTween != null && _fadeoutTween.IsPlaying())
            {
                _fadeoutTween.Kill();
            }

            if (_viewModel != null)
            {
                _viewModel.RecActive = false;
            }

            _face = null;
        }

        public override void OnStay()
        {

        }


        /// <summary>
        /// 延迟跳转页面
        /// </summary>
        private IEnumerator DelayTurnView()
        {
            yield return new WaitForSeconds(3);

            if (_viewModel != null)
            {
                _fadeoutTween = DOTween.To(() => _viewModel.RecAlpha, x => _viewModel.RecAlpha = x, 0, 1)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        //UIManager.Instance.Enter((int)ViewId.ResResult, true, () =>
                        //  {
                        //      SingletonUIModule.Instance.Quit((int)ViewId.FaceRec);
                        //  });

                        //MEventHub.Instance.Dispatch((int)EventId.FaceDetectResult, new FaceArgs()
                        //{
                        //    face = _face
                        //});
                    });
            }
        }

        /// <summary>
        /// 修正位置Y
        /// </summary>
        private Vector2 ModifyPosY(Vector2 pos)
        {
            pos.y *= -1;

            return pos;
        }
    }
}
