using Ninject.Modules;

using Programatica.Framework.Core.Adapter;

using SAFT_Reader.Adapter;
using SAFT_Reader.Services;

namespace SAFT_Reader
{
    /// <summary>
    /// Represents the Ninject module responsible for configuring dependency injection bindings in the application.
    /// </summary>
    public class ApplicationModule : NinjectModule
    {
        /// <summary>
        /// Loads the dependency injection bindings for the application.
        /// </summary>
        public override void Load()
        {
            // Bind interfaces to their respective implementations
            Bind<IJsonSerializerAdapter>().To<JsonSerializerAdapter>();
            Bind<IFileStreamAdapter>().To<FileStreamAdapter>();
            Bind<IXmlSerializerAdapter>().To<XmlSerializerAdapter>();
            Bind<IAuditService>().To<AuditService>();
        }
    }
}