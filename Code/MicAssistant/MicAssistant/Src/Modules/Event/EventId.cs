namespace MicAssistant
{
    public static class FaceRecEventId
    {
        /// <summary>
        /// 人脸检测请求
        /// </summary>
        public const int FaceDetectRequest = 0;

        /// <summary>
        /// 人脸检测返回结果
        /// </summary>
        public const int FaceDetectResult = 1;

        /// <summary>
        /// 人脸搜索请求
        /// </summary>
        public const int FaceSearchRequest = 2;

        /// <summary>
        /// 人脸搜索返回结果
        /// </summary>
        public const int FaceSearchResult = 3;
    }

    public static class SpeechRecEventId
    {
        /// <summary>
        /// 语音识别请求
        /// </summary>
        public const int SpeechRecRequest = 100;

        /// <summary>
        /// 语音识别返回结果
        /// </summary>
        public const int SpeechRecResult = 101;
    }

    public static class LoginEventId
    {
        /// <summary>
        /// 登录请求
        /// </summary>
        public const int LoginRequest = 200;

        /// <summary>
        /// 登录返回结果
        /// </summary>
        public const int LoginResult = 201;

        /// <summary>
        /// 改变登录状态
        /// </summary>
        public const int ChangeLogin = 202;
    }
}
