using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaceAggregator.Services;
using FaceAggregator.Utils;
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
            _kernel.Bind<IImagesService>().To<ImagesService>();
            _kernel.Bind<IConfiguration>().To<WebConfiguration>();
            _kernel.Bind<IFaceRecognition>().To<HttpFaceRecognition>();
            _kernel.Bind<IFaceSimilarity>().To<FaceSimilarity>();
            _kernel.Bind<IRecognitionService>().To<RecognitionService>();
            _kernel.Bind<IAccountService>().To<AccountService>();
        }
    }
}