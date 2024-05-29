namespace Grwm.Module
{
    using Autofac;
    using english_learning_server.Interfaces;
    using english_learning_server.Service;

    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
            .RegisterType<TokenService>()
            .As<ITokenService>()
            .SingleInstance();

            builder
            .RegisterType<EmailService>()
            .As<IEmailService>()
            .SingleInstance();

            builder
            .RegisterType<LangChainService>()
            .As<ILangChainService>()
            .SingleInstance();

            builder
            .RegisterType<GoogleCloudService>()
            .As<IGoogleCloudService>()
            .SingleInstance();
        }
    }
}