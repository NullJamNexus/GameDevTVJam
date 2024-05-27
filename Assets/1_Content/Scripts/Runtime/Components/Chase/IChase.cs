namespace NJN.Runtime.Components
{
    public interface IChase : IComponent
    {
        public void StartChase();

        public void UpdateChase();

        public void CancellChase();
    }
}
