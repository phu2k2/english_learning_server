namespace Grwm.Module
{
  using Autofac;
  using english_learning_server.Interfaces;
  using english_learning_server.Repository;

  public class RepositoryModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder
        .RegisterGeneric(typeof(Repository<>))
        .As(typeof(IRepository<>))
        .InstancePerLifetimeScope();

      builder
        .RegisterType<ProfileRepository>()
        .As<IProfileRepository>()
        .InstancePerLifetimeScope();
    }
  }
}
