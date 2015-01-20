
namespace Sigflow.Module
{
    public interface IMasterModule : IModule
    {
        bool Start();

        void BeforeStop();
        void AfterStop();
    }
}
