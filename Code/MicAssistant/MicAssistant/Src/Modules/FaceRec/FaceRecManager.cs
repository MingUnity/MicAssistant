using Ming.EventHub;
using MingUnity.Common;
using MingUnity.FaceRec;
using UnityEngine;
using Face = MingUnity.FaceRec.FaceDetectRes.Result.Face;

namespace MicAssistant
{
    /// <summary>
    /// 人脸识别管理
    /// </summary>
    public class FaceRecManager : IEventListener
    {
        private IFaceDetect _faceDetect;

        public FaceRecManager()
        {
            _faceDetect = new FaceDetect(new FaceAppData("11253066", "WwnwTfmq9ulkzknDBOv9tr6s", "7DNbqdtYvhVr0nR8YMtbIUeFfwyCBVgc"));

            MEventHub.Instance.AddListener((int)FaceRecEventId.FaceDetectRequest, this);
        }

        public void HandleEvent(int eventId, IEventArgs args)
        {
            if (eventId == (int)FaceRecEventId.FaceDetectRequest && args is FaceRecResultArgs)
            {
                FaceRecResultArgs faceRecArgs = args as FaceRecResultArgs;

                _faceDetect?.AsyncDetect(faceRecArgs?.tex?.EncodeToJPG(), (res) =>
                {
                    Loom.QueueOnMainThread(() =>
                    {
                        MEventHub.Instance.Dispatch((int)FaceRecEventId.FaceDetectResult, new FaceRecResultArgs()
                        {
                            res = res
                        });
                    });
                });
            }
        }
    }

    /// <summary>
    /// 人脸参数
    /// </summary>
    public class FaceArgs : IEventArgs
    {
        public Face face;
    }

    /// <summary>
    /// 人脸识别结果参数
    /// </summary>
    public class FaceRecResultArgs : IEventArgs
    {
        public FaceDetectRes res;

        public Texture2D tex;
    }
}
