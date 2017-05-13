using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaceAggregator.Services;
using Ninject;


namespace FaceAggregator.Infrastructure
{
    public class ServicesDependencyBindingContainer : IDependencyBindingContainer
    {
        private IKernel _kernel;

        public ServicesDependencyBindingContainer(IKernel kernel)
        {
            this._kernel = kernel;
        }

        public void ActivateBindings()
        {
            _kernel.Bind<IUploadService>().To<UploadService>();
        }
    }
}