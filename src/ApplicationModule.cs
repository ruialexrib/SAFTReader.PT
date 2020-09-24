using Ninject.Modules;
using Programatica.Framework.Core.Adapter;
using SAFT_Reader.Adapter;

namespace SAFT_Reader
{
    public class ApplicationModule : NinjectModule
    {
        public override void Load()
        {
            //Bind(typeof(IRepository<>)).To(typeof(GenericRepository<>));
            Bind<IJsonSerializerAdapter>().To<JsonSerializerAdapter>();
            Bind<IFileStreamAdapter>().To<FileStreamAdapter>();
            Bind<IXmlSerializerAdapter>().To<XmlSerializerAdapter>();
        }
    }
}
