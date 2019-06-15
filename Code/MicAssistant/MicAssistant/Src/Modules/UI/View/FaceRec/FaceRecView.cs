using MFramework.UI;
using MFramework.UI.Utility;
using MingUnity.Common;
using UnityEngine;
using UnityEngine.UI;

namespace MicAssistant
{
    /// <summary>
    /// 人脸识别界面
    /// </summary>
    public class FaceRecView : MFViewBase<IFaceRecViewModel>
    {
        private CanvasGroup _rootCanvas;

        private Image[] _recImageArr;

        private SpriteAnimator[] _recSpriteAnimators;

        private SpriteAnimator _scannerSpriteAnimator;

        private RawImage _imgCamera;

        private Image _imgScanner;

        public bool Active
        {
            get
            {
                bool res = false;

                if (_rootCanvas != null)
                {
                    res = _rootCanvas.alpha > 0;
                }

                return res;
            }
            private set
            {
                if (_rootCanvas != null)
                {
                    _rootCanvas.alpha = value ? 1 : 0;

                    _rootCanvas.blocksRaycasts = value;
                }
            }
        }

        /// <summary>
        /// 检测激活
        /// </summary>
        private bool ScannerActive
        {
            get
            {
                bool res = false;

                if (_imgScanner != null)
                {
                    res = _imgScanner.color.a > 0;
                }

                return res;
            }
            set
            {
                SetImageActive(_imgScanner, value);

                if (value)
                {
                    _scannerSpriteAnimator?.Rewind();
                }
                else
                {
                    _scannerSpriteAnimator?.Stop();
                }
            }
        }

        /// <summary>
        /// 识别图激活
        /// </summary>
        private bool RecActive
        {
            set
            {
                if (_recImageArr != null && _recSpriteAnimators != null && _recImageArr.Length == _recSpriteAnimators.Length)
                {
                    for (int i = 0; i < _recImageArr.Length; i++)
                    {
                        Image image = _recImageArr[i];

                        SpriteAnimator anim = _recSpriteAnimators[i];

                        SetImageActive(image, value);

                        if (value)
                        {
                            anim?.Rewind();
                        }
                        else
                        {
                            anim?.Stop();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 识别图不透明度
        /// </summary>
        private float RecAlpha
        {
            set
            {
                if (_recImageArr != null)
                {
                    for (int i = 0; i < _recImageArr.Length; i++)
                    {
                        SetImageAlpha(_recImageArr[i], value);
                    }
                }
            }
        }

        /// <summary>
        /// 识别图位置
        /// </summary>
        private Vector2[] RecPosition
        {
            set
            {
                if (_recImageArr != null && value != null && _recImageArr.Length == value.Length)
                {
                    for (int i = 0; i < _recImageArr.Length; i++)
                    {
                        SetImagePosition(_recImageArr[i], value[i]);
                    }
                }
            }
        }

        /// <summary>
        /// 摄像机纹理
        /// </summary>
        private Texture CamTexture
        {
            get
            {
                Texture res = null;

                if (_imgCamera != null)
                {
                    res = _imgCamera.texture;
                }

                return res;
            }
            set
            {
                if (_imgCamera != null)
                {
                    _imgCamera.texture = value;

                    AdaptCamSize();
                }
            }
        }

        /// <summary>
        /// 摄像头图片旋转
        /// </summary>
        private float ImgCamRotationZ
        {
            get
            {
                float res = 0;

                if (_imgCamera != null)
                {
                    res = _imgCamera.rectTransform.localEulerAngles.z;
                }

                return res;
            }
            set
            {
                if (_imgCamera != null)
                {
                    Vector3 eulerAngles = _imgCamera.rectTransform.localEulerAngles;

                    eulerAngles.z = value;

                    _imgCamera.rectTransform.localEulerAngles = eulerAngles;
                }
            }
        }

        /// <summary>
        /// 摄像头图片垂直翻转
        /// </summary>
        private bool ImgCamVerticalMirror
        {
            get
            {
                bool res = false;

                if (_imgCamera != null)
                {
                    res = _imgCamera.rectTransform.localScale.y < 0;
                }

                return res;
            }
            set
            {
                if (_imgCamera != null && value)
                {
                    Vector3 scale = _imgCamera.rectTransform.localScale;

                    scale.y *= -1;

                    _imgCamera.rectTransform.localScale = scale;
                }
            }
        }

        /// <summary>
        /// 摄像头画面水平翻转
        /// </summary>
        private bool ImgCamHorizontalMirror
        {
            get
            {
                bool res = false;

                if (_imgCamera != null)
                {
                    res = _imgCamera.rectTransform.localScale.x < 0;
                }

                return res;
            }
            set
            {
                if (_imgCamera != null && value)
                {
                    Vector3 scale = _imgCamera.rectTransform.localScale;

                    scale.x *= -1;

                    _imgCamera.rectTransform.localScale = scale;
                }
            }
        }

        protected override void OnCreate()
        {
            _rootCanvas = _root.GetComponent<CanvasGroup>();

            Transform rootTrans = _rootCanvas.transform;

            _imgCamera = rootTrans.Find("ImgCamera").GetComponent<RawImage>();

            _imgScanner = rootTrans.Find("ImgHeadScanner").GetComponent<Image>();

            _recImageArr = new Image[]
            {
                rootTrans.Find("PnlDetect/ImgRightEye").GetComponent<Image>(),

                rootTrans.Find("PnlDetect/ImgNose").GetComponent<Image>(),

                rootTrans.Find("PnlDetect/ImgMouth").GetComponent<Image>()
            };

            GenerateAnim();
        }

        protected override void PropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case "Active":
                    Active = _viewModel.Active;
                    break;

                case "RecActive":
                    RecActive = _viewModel.RecActive;
                    break;

                case "RecPosition":
                    RecPosition = _viewModel.RecPosition;
                    break;

                case "CamTexture":
                    CamTexture = _viewModel.CamTexture;
                    break;

                case "CamRotation":
                    ImgCamRotationZ = _viewModel.CamRotation;
                    break;

                case "CamVerticalMirror":
                    ImgCamVerticalMirror = _viewModel.CamVerticalMirror;
                    break;

                case "ScannerActive":
                    ScannerActive = _viewModel.ScannerActive;
                    break;

                case "CamHorizontalMirror":
                    ImgCamHorizontalMirror = _viewModel.CamHorizontalMirror;
                    break;

                case "RecAlpha":
                    RecAlpha = _viewModel.RecAlpha;
                    break;
            }
        }

        /// <summary>
        /// 设置图片激活
        /// </summary>
        private void SetImageActive(Image image, bool active)
        {
            if (image != null)
            {
                Color color = image.color;

                color.a = active ? 1 : 0;

                image.color = color;
            }
        }

        /// <summary>
        /// 设置图片不透明度
        /// </summary>
        private void SetImageAlpha(Image image, float alpha)
        {
            if (image != null)
            {
                Color color = image.color;

                color.a = alpha;

                image.color = color;
            }
        }

        /// <summary>
        /// 设置图片位置
        /// </summary>
        private void SetImagePosition(Image image, Vector2 pos)
        {
            if (image != null)
            {
                image.rectTransform.anchoredPosition = pos;
            }
        }

        /// <summary>
        /// 设置图片缩放
        /// </summary>
        private void SetImageScale(Image image, float scaleX, float scaleY)
        {
            if (image != null)
            {
                image.rectTransform.localScale = new Vector3(scaleX, scaleY, 1);
            }
        }

        /// <summary>
        /// 设置图片旋转
        /// </summary>
        private void SetImageRotation(Image image, float z)
        {
            if (image != null)
            {
                Vector3 eulerAngles = image.rectTransform.localEulerAngles;

                eulerAngles.z = z;

                image.rectTransform.localEulerAngles = eulerAngles;
            }
        }

        /// <summary>
        /// 自适应摄像机尺寸
        /// </summary>
        private void AdaptCamSize()
        {
            Vector2 selfAdaptBasicSize = GetAdaptBasicSize();

            if (_imgCamera != null)
            {
                _imgCamera.SetNativeSize();

                if (_imgCamera.rectTransform.sizeDelta != Vector2.zero)
                {
                    Vector2 imgSize = Vector2.zero;

                    if (_imgCamera.texture != null)
                    {
                        float texWidth = _imgCamera.texture.width;

                        float texHeight = _imgCamera.texture.height;

                        bool useWidth = selfAdaptBasicSize.x / texWidth > selfAdaptBasicSize.y / texHeight;

                        if (useWidth)
                        {
                            imgSize.x = selfAdaptBasicSize.x;

                            imgSize.y = selfAdaptBasicSize.x * texHeight / texWidth;
                        }
                        else
                        {
                            imgSize.x = selfAdaptBasicSize.y * texWidth / texHeight;

                            imgSize.y = selfAdaptBasicSize.y;
                        }
                    }
                    else
                    {
                        imgSize = selfAdaptBasicSize;
                    }

                    float scaleX = imgSize.x / _imgCamera.rectTransform.sizeDelta.x;

                    float scaleY = imgSize.y / _imgCamera.rectTransform.sizeDelta.y;

                    if (_viewModel != null)
                    {
                        scaleX = _viewModel.CamHorizontalMirror ? -scaleX : scaleX;

                        scaleY = _viewModel.CamVerticalMirror ? -scaleY : scaleY;
                    }

                    _imgCamera.rectTransform.localScale = new Vector3(scaleX, scaleY, 1);
                }
            }
        }

        /// <summary>
        /// 获取自适应基础尺寸
        /// </summary>
        private Vector2 GetAdaptBasicSize()
        {
            Vector2 res = Vector2.zero;

            if (_imgCamera != null)
            {
                float rotation = _imgCamera.rectTransform.localEulerAngles.z % 360;

                rotation = rotation < 0 ? rotation + 360 : rotation;

                float rotationValue = rotation * Mathf.Deg2Rad;

                float sinValue = Mathf.Sin(rotationValue);

                float cosValue = Mathf.Cos(rotationValue);

                float width = _root.rect.width;

                float height = _root.rect.height;

                float resWidth = 0;

                float resHeight = 0;

                if (rotation >= 0 && rotation < 90)
                {
                    resWidth = width * cosValue + height * sinValue;

                    resHeight = width * sinValue + height * cosValue;
                }
                else if (rotation >= 90 && rotation < 180)
                {
                    resWidth = -width * cosValue + height * sinValue;

                    resHeight = width * sinValue - height * cosValue;
                }
                else if (rotation >= 180 && rotation < 270)
                {
                    resWidth = -width * cosValue - height * sinValue;

                    resHeight = -width * sinValue - height * cosValue;
                }
                else if (rotation >= 270 && rotation < 360)
                {
                    resWidth = width * cosValue - height * sinValue;

                    resHeight = -width * sinValue + height * cosValue;
                }
                else
                {
                    resWidth = width;

                    resHeight = height;
                }

                res = new Vector2(resWidth, resHeight);
            }

            return res;
        }

        /// <summary>
        /// 生成动画
        /// </summary>
        private void GenerateAnim()
        {
            string abPath = string.Format("{0}/{1}", PlatformUtility.GetResStreamingAssets(), "AssetBundle/facerecviewanim.assetbundle");

            //面部检测
            _scannerSpriteAnimator = _imgScanner.gameObject.AddComponent<SpriteAnimator>();

            _scannerSpriteAnimator.FPS = 12;

            _scannerSpriteAnimator.Loop = true;

            for (int i = 0; i < 48; i++)
            {
                string assetName = string.Format("HeadScanner_{0}", i);

                _scannerSpriteAnimator?.LoadSpriteFrames(AssetBundleLoader?.GetAsset<Sprite>(abPath, assetName));
            }

            //面部分析
            if (_recImageArr != null && _recImageArr.Length >= 3)
            {
                _recSpriteAnimators = new SpriteAnimator[_recImageArr.Length];

                for (int i = 0; i < _recImageArr.Length; i++)
                {
                    _recSpriteAnimators[i] = _recImageArr[i]?.gameObject.AddComponent<SpriteAnimator>();
                }

                //眼睛
                SpriteAnimator eye = _recSpriteAnimators[0];

                eye.FPS = 12;

                for (int i = 0; i < 32; i++)
                {
                    string assetName = string.Format("EyeAnalyze_{0}", i);

                    eye?.LoadSpriteFrames(AssetBundleLoader?.GetAsset<Sprite>(abPath, assetName));
                }

                //鼻子
                SpriteAnimator nose = _recSpriteAnimators[1];

                nose.FPS = 12;

                for (int i = 0; i < 28; i++)
                {
                    string assetName = string.Format("NoseAnalyze_{0}", i);

                    nose?.LoadSpriteFrames(AssetBundleLoader?.GetAsset<Sprite>(abPath, assetName));
                }

                //嘴巴
                SpriteAnimator mouth = _recSpriteAnimators[2];

                mouth.FPS = 12;

                for (int i = 0; i < 32; i++)
                {
                    string assetName = string.Format("MouthAnalyze_{0}", i);

                    mouth?.LoadSpriteFrames(AssetBundleLoader?.GetAsset<Sprite>(abPath, assetName));
                }
            }
        }
    }
}
