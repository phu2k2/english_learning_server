namespace Grwm.Module
{
    using Autofac;
    using english_learning_server.Interfaces;
    using english_learning_server.Repository;
    using english_learning_server.Service;

    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
            .RegisterType<TokenService>()
            .As<ITokenService>()
            .SingleInstance();
        }
    }
}