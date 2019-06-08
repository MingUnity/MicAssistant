using MFramework.UI;

namespace MicAssistant
{
    /// <summary>
    /// UI服务工厂
    /// </summary>
    public class UIServiceFactory : MFUIServiceFactoryBase
    {
        protected override IUIService CreateServiceById(int viewId)
        {
            IUIService result = null;

            switch ((ViewId)viewId)
            {
                case ViewId.FaceRecView:
                    result = new UIFaceRecService();
                    break;
            }

            return result;
        }
    }
}
