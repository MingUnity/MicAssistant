using MFramework.UI;
using System.IO;
using UnityEngine;
using MingUnity.VoiceInput;
using Ming.EventHub;
using System.Collections.Generic;
using System;
using DG.Tweening;

namespace MicAssistant
{
    public class UIMemoService : MFUIServiceBase<MemoView, MemoViewModel>, IEventListener
    {
        private string _dataPath;

        private VoiceReceiver _voiceReceiver;

        protected override void OnCreateCompleted()
        {
            _viewModel.OnStartRecordEvent += ViewModel_OnStartRecordEvent;

            _viewModel.OnEndRecordEvent += ViewModel_OnEndRecordEvent;

            _viewModel.OnDeleteItemEvent += ViewModel_OnDeleteItemEvent;

            _dataPath = Path.Combine(Application.persistentDataPath, "MingMemo.mdat");

            _voiceReceiver = VoiceReceiver.Default;

            MEventHub.Instance.AddListener((int)SpeechRecEventId.SpeechRecResult, this);
        }

        protected override void OnEnterStart()
        {
            ReadData();
        }

        protected override void OnQuitStart()
        {
            MEventHub.Instance.RemoveListener((int)SpeechRecEventId.SpeechRecResult, this);

            _viewModel.OnStartRecordEvent -= ViewModel_OnStartRecordEvent;

            _viewModel.OnEndRecordEvent -= ViewModel_OnEndRecordEvent;
        }

        protected override void EnterAnim(Action callback)
        {
            _viewModel.ViewAlpha = 0;

            DOTween.To(() => _viewModel.ViewAlpha, x => _viewModel.ViewAlpha = x, 1, 1)
                .OnComplete(() => { callback?.Invoke(); });
        }

        public void HandleEvent(int eventId, IEventArgs args)
        {
            SpeechRecEventId id = (SpeechRecEventId)eventId;

            switch (id)
            {
                case SpeechRecEventId.SpeechRecResult:
                    HandleSpeechRecResult(args as SpeechRecResultArgs);
                    break;
            }
        }

        private void ViewModel_OnEndRecordEvent()
        {
            _voiceReceiver.StopRecord((byte[] buffer) =>
            {
                MEventHub.Instance.Dispatch((int)SpeechRecEventId.SpeechRecRequest, new SpeechRecRequestArgs()
                {
                    speechBuffer = buffer
                });
            });
        }

        private void ViewModel_OnStartRecordEvent()
        {
            _voiceReceiver.StartRecord(10);
        }

        private void ReadData()
        {
            if (File.Exists(_dataPath))
            {
                _viewModel.Model = BinaryConverter.Deserialize<MemoModel>(File.ReadAllBytes(_dataPath));
            }
            else
            {
                _viewModel.Model = new MemoModel();
            }
        }

        private void SaveData()
        {
            string dir = Path.GetDirectoryName(_dataPath);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            byte[] buffer = BinaryConverter.Serialize(_viewModel.Model);

            if (buffer != null)
            {
                File.WriteAllBytes(_dataPath, buffer);
            }
        }

        private void HandleSpeechRecResult(SpeechRecResultArgs args)
        {
            if (args == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(args.content))
            {
                List<string> contents = _viewModel.MemoList;

                contents?.Add(args.content?.TrimEnd('。'));

                _viewModel.MemoList = contents;

                SaveData();
            }
        }

        private void ViewModel_OnDeleteItemEvent(int index)
        {
            List<string> list = _viewModel.MemoList;

            if (index >= 0 && list != null && list.Count > index)
            {
                list.RemoveAt(index);

                _viewModel.MemoList = list;

                SaveData();
            }
        }
    }
}
