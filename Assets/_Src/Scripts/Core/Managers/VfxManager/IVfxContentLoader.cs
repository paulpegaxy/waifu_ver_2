namespace Template.VfxManager
{
    public interface IVfxContentLoader
    {
        IVfxConfig GetVfxConfig(string id);
    }
}