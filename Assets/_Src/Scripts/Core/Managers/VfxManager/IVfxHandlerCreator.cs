namespace Template.VfxManager
{
    public interface IVfxHandlerCreator
    {
        IVfxHandler CreateVfxHandler(string handlerName);
    }
}