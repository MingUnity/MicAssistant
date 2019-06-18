using Ming.EventHub;
using MingUnity.Common;
using MingUnity.FaceRec;
using UnityEngine;

namespace MicAssistant
{
    /// <summary>
    /// 人脸识别管理
    /// </summary>
    public class FaceRecManager : IEventListener
    {
        private IFaceDetect _faceDetect;

        private IFaceSearch _faceSearch;

        public FaceRecManager()
        {
            FaceAppData appData = new FaceAppData("11253066", "WwnwTfmq9ulkzknDBOv9tr6s", "7DNbqdtYvhVr0nR8YMtbIUeFfwyCBVgc");

            _faceDetect = new FaceDetect(appData);

            _faceSearch = new FaceSearch("FaceRec", appData);
        }

        public void HandleEvent(int eventId, IEventArgs args)
        {
            switch (eventId)
            {
                case FaceRecEventId.FaceDetectRequest:
                    HandleDetectRequest(args as FaceRecRequestArgs);
                    break;

                case FaceRecEventId.FaceSearchRequest:
                    HandleSearchRequest(args as FaceRecRequestArgs);
                    break;
            }
        }

        private void HandleDetectRequest(FaceRecRequestArgs args)
        {
            if (args == null)
            {
                return;
            }

            _faceDetect?.AsyncDetect(args.tex?.EncodeToJPG(), (res) =>
            {
                Loom.QueueOnMainThread(() =>
                {
                    MEventHub.Instance.Dispatch(FaceRecEventId.FaceDetectResult, new FaceDetectResultArgs()
                    {
                        res = res
                    });
                });
            });
        }

        private void HandleSearchRequest(FaceRecRequestArgs args)
        {
            if (args == null)
            {
                return;
            }

            _faceSearch?.AsyncSearch(args.tex?.EncodeToJPG(), (res) =>
            {
                Loom.QueueOnMainThread(() =>
                {
                    MEventHub.Instance.Dispatch(FaceRecEventId.FaceSearchResult, new FaceSearchResultArgs()
                    {
                        res = res
                    });
                });
            });
        }
    }
}

/// <summary>
/// 人脸识别请求参数
/// </summary>
public class FaceRecRequestArgs : IEventArgs
{
    public Texture2D tex;
}

/// <summary>
/// 人脸检测返回结果参数
/// </summary>
public class FaceDetectResultArgs : IEventArgs
{
    public FaceDetectRes res;
}

/// <summary>
/// 人脸搜索返回结果参数
/// </summary>
public class FaceSearchResultArgs : IEventArgs
{
    public FaceSearchRes res;
}
