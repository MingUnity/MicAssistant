using Ming.EventHub;
using Ming.Speech;
using MingUnity.Common;

namespace MicAssistant
{
    public class SpeechRecManager : IEventListener
    {
        private ISpeechRec _speechRec;

        public SpeechRecManager()
        {
            _speechRec = new SpeechRecer(new SpeechAppData("15470261", "Dlrj6PcxtjXbzaKAO2Fp0ZuA", "oYt0ED8bwrkXKh6WR1c8XYdMt4EWiEYZ"));
        }

        public void HandleEvent(int eventId, IEventArgs args)
        {
            switch (eventId)
            {
                case SpeechRecEventId.SpeechRecRequest:
                    HandleSpeechRecRequest(args as SpeechRecRequestArgs);
                    break;
            }
        }

        private void HandleSpeechRecRequest(SpeechRecRequestArgs args)
        {
            if (args == null)
            {
                return;
            }

            _speechRec.AsyncSpeechRec(args.speechBuffer, (res) =>
            {
                Loom.QueueOnMainThread(() =>
                {
                    MEventHub.Instance.Dispatch(SpeechRecEventId.SpeechRecResult, new SpeechRecResultArgs()
                    {
                        content = res?.PriorResult
                    });
                });
            });
        }
    }

    /// <summary>
    /// 语音识别请求参数
    /// </summary>
    public class SpeechRecRequestArgs : IEventArgs
    {
        public byte[] speechBuffer;
    }

    /// <summary>
    /// 语音识别返回结果参数
    /// </summary>
    public class SpeechRecResultArgs : IEventArgs
    {
        public string content;
    }
}
