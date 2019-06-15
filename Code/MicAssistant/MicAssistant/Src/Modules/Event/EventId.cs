namespace MicAssistant
{
    public enum FaceRecEventId
    {
        /// <summary>
        /// 人脸检测请求
        /// </summary>
        FaceDetectRequest = 0,

        /// <summary>
        /// 人脸检测返回结果
        /// </summary>
        FaceDetectResult = 1,

        /// <summary>
        /// 人脸搜索请求
        /// </summary>
        FaceSearchRequest = 2,

        /// <summary>
        /// 人脸搜索返回结果
        /// </summary>
        FaceSearchResult = 3
    }

    public enum SpeechRecEventId
    {
        /// <summary>
        /// 语音识别请求
        /// </summary>
        SpeechRecRequest = 100,

        /// <summary>
        /// 语音识别返回结果
        /// </summary>
        SpeechRecResult = 101
    }
}
