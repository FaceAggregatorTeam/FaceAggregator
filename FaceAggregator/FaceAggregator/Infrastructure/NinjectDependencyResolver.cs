using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;

namespace FaceAggregator.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;
        private readonly IList<IDependencyBindingContainer> _bindingContainers; 

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            _bindingContainers = new List<IDependencyBindingContainer>();
            _kernel = kernelParam;
            AddBindingContainers();
            AddBindings();
        }

        private void AddBindingContainers()
        {
            _bindingContainers.Add(new ServicesDependencyBindingContainer(_kernel));
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        /// <summary>
        /// It allows to add new bindings for Ninject
        /// </summary>
        private void AddBindings()
        {
            foreach (var bindingContainer in _bindingContainers)
            {
                bindingContainer.ActivateBindings();
            }
        }
    }
}