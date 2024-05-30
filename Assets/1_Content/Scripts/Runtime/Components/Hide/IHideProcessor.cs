namespace NJN.Runtime.Components
{
    public interface IHideProcessor : IComponent
    {
        public float HiddenAlpha { get; }
        public string GetHiddenLayer();
    }
}