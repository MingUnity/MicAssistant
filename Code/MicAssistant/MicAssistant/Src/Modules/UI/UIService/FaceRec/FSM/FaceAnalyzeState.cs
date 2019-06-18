using Ming.FSM;
using UnityEngine;
using System.Collections;
using Face = MingUnity.FaceRec.FaceDetectRes.Result.Face;
using DG.Tweening;
using MFramework.UI;
using Ming.EventHub;
using MingUnity.Common;
using User = MingUnity.FaceRec.FaceSearchRes.Result.User;

namespace MicAssistant
{
    /// <summary>
    /// 面部分析状态
    /// </summary>
    public class FaceAnalyzeState : FSMState, IEventListener
    {
        private IFaceRecViewModel _viewModel;

        private IFSMSystem _fsmSystem;

        private Tween _fadeoutTween;

        private Face _face;

        private bool _permit;

        public FaceAnalyzeState(IFSMSystem fsmSystem, IFaceRecViewModel viewModel)
        {
            this._fsmSystem = fsmSystem;

            this._viewModel = viewModel;

            MEventHub.Instance.AddListener(FaceRecEventId.FaceSearchResult, this);
        }

        public override void OnEnter(params object[] keys)
        {
            _permit = false;

            if (keys != null && keys.Length > 1)
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

                Texture2D faceTex = keys[1] as Texture2D;

                if (faceTex != null)
                {
                    MEventHub.Instance.Dispatch(FaceRecEventId.FaceSearchRequest, new FaceRecRequestArgs()
                    {
                        tex = faceTex
                    });
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

        public void HandleEvent(int eventId, IEventArgs args)
        {
            switch (eventId)
            {
                case FaceRecEventId.FaceSearchResult:
                    HandleSearchResult(args as FaceSearchResultArgs);
                    break;
            }
        }

        private void HandleSearchResult(FaceSearchResultArgs args)
        {
            if (args == null)
            {
                return;
            }

            if (args.res != null && args.res.result != null && args.res.result.user_list != null && args.res.result.user_list.Count > 0)
            {
                User user = args.res.result.user_list[0];

                //<Ming> TODO Handle face rec result id
                if (user != null && user.user_id == "CM" && user.score >= 80)
                {
                    _permit = true;
                }
            }
        }

        /// <summary>
        /// 延迟跳转页面
        /// </summary>
        private IEnumerator DelayTurnView()
        {
            yield return new WaitForSeconds(3);

            if (_permit)
            {
                if (_viewModel != null)
                {
                    _fadeoutTween = DOTween.To(() => _viewModel.RecAlpha, x => _viewModel.RecAlpha = x, 0, 1)
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            UIManager.Instance.Enter(ViewId.MemoView, true, () =>
                            {
                                UIManager.Instance.Quit(ViewId.FaceRecView, false, null, true);
                            });
                        });
                }
            }
            else
            {
                _fsmSystem.TurnDefault();
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
