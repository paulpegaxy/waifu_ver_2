namespace Template.VfxManager
{
    public interface IVfxConfig
    {
        string ID { get; }
        string GraphicPath { get; }
        int Duration { get; }
        string AttachChildName { get; }

        string GetHandlerTag();
    }
}