using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Template.VfxManager
{
    public class VfxController
    {

        private IVfxAssetLoader vfxLoader;
        private IVfxHandlerCreator vfxCreator;
        private IVfxContentLoader vfxContentLoader;

        public VfxController(IVfxAssetLoader vfxLoader, IVfxHandlerCreator vfxCreator, IVfxContentLoader vfxContentLoader)
        {
            this.vfxLoader = vfxLoader;
            this.vfxCreator = vfxCreator;
            this.vfxContentLoader = vfxContentLoader;
        }

        public async Task<IVfxHandler> ShowVfx(ShowVfxData showData, int? overrideDuration = null)
        {
            try
            {
                IVfxConfig config = vfxContentLoader.GetVfxConfig(showData.vfxId);
                IVfxHandler handler = vfxCreator.CreateVfxHandler(config.GetHandlerTag());
                GameObject graphic = await vfxLoader.LoadVfx(config.GraphicPath);
                handler.AssignGraphic(graphic);
                handler.Init(showData.initPosition, showData.initRotation);
                handler.AttachTarget(showData.attachTarget, config.AttachChildName);
                handler.Show(overrideDuration ?? config.Duration);
                return handler;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}