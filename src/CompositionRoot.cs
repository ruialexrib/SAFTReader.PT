using Ninject;
using Ninject.Modules;

namespace SAFT_Reader
{
    /// <summary>
    /// A class responsible for managing the dependency injection container and resolving dependencies.
    /// </summary>
    public class CompositionRoot
    {
        private static IKernel _ninjectKernel;

        /// <summary>
        /// Wires the application with the specified Ninject module to configure dependency bindings.
        /// </summary>
        /// <param name="module">The Ninject module responsible for defining bindings.</param>
        public static void Wire(INinjectModule module)
        {
            _ninjectKernel = new StandardKernel(module);
        }

        /// <summary>
        /// Resolves and returns an instance of the specified type from the dependency injection container.
        /// </summary>
        /// <typeparam name="T">The type of the object to resolve.</typeparam>
        /// <returns>An instance of the specified type.</returns>
        public static T Resolve<T>()
        {
            return _ninjectKernel.Get<T>();
        }
    }

}