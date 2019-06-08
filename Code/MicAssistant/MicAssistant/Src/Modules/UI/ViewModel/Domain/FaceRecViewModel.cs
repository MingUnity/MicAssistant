using MingUnity.MVVM.Model;
using MingUnity.MVVM.ViewModel;
using System;
using UnityEngine;

namespace MicAssistant
{
    /// <summary>
    /// 人脸识别视图模型
    /// </summary>
    public class FaceRecViewModel : ViewModelBase<IModel>, IFaceRecViewModel
    {
        private bool _active;

        private bool _recActive;

        private float _recAlpha;

        private Texture _camTexture;

        private Vector2[] _recPosArr;

        private float _camRotation;

        private bool _camVerticalMirror;

        private bool _scannerActive;

        private bool _camHorizontalMirror;

        /// <summary>
        /// 激活
        /// </summary>
        public override bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;

                RaisePropertyChanged("Active");
            }
        }

        /// <summary>
        /// 检测激活
        /// </summary>
        public bool ScannerActive
        {
            get
            {
                return _scannerActive;
            }
            set
            {
                _scannerActive = value;

                RaisePropertyChanged("ScannerActive");
            }
        }

        /// <summary>
        /// 识别激活
        /// </summary>
        public bool RecActive
        {
            get
            {
                return _recActive;
            }
            set
            {
                _recActive = value;

                _recAlpha = value ? 1 : 0;

                RaisePropertyChanged("RecActive");
            }
        }

        /// <summary>
        /// 识别不透明度
        /// </summary>
        public float RecAlpha
        {
            get
            {
                return _recAlpha;
            }
            set
            {
                _recAlpha = value;

                RaisePropertyChanged("RecAlpha");
            }
        }

        /// <summary>
        /// 识别位置
        /// </summary>
        public Vector2[] RecPosition
        {
            get
            {
                return _recPosArr;
            }
            set
            {
                _recPosArr = value;

                RaisePropertyChanged("RecPosition");
            }
        }

        /// <summary>
        /// 摄像头纹理
        /// </summary>
        public Texture CamTexture
        {
            get
            {
                return _camTexture;
            }
            set
            {
                _camTexture = value;

                RaisePropertyChanged("CamTexture");
            }
        }

        /// <summary>
        /// 摄像头旋转
        /// </summary>
        public float CamRotation
        {
            get
            {
                return _camRotation;
            }
            set
            {
                _camRotation = value;

                RaisePropertyChanged("CamRotation");
            }
        }

        /// <summary>
        /// 摄像头垂直翻转
        /// </summary>
        public bool CamVerticalMirror
        {
            get
            {
                return _camVerticalMirror;
            }
            set
            {
                _camVerticalMirror = value;

                RaisePropertyChanged("CamVerticalMirror");
            }
        }

        /// <summary>
        /// 摄像头水平翻转
        /// </summary>
        public bool CamHorizontalMirror
        {
            get
            {
                return _camHorizontalMirror;
            }
            set
            {
                _camHorizontalMirror = value;

                RaisePropertyChanged("CamHorizontalMirror");
            }
        }

        public override void Setup()
        {
            Active = _active;

            RecPosition = _recPosArr;

            RecActive = _recActive;

            CamTexture = _camTexture;

            CamRotation = _camRotation;

            CamVerticalMirror = _camVerticalMirror;

            ScannerActive = _scannerActive;

            RecAlpha = _recAlpha;
        }
    }
}
