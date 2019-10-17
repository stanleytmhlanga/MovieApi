[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MovieWebApplication.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(MovieWebApplication.App_Start.NinjectWebCommon), "Stop")]

namespace MovieWebApplication.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Dependencies;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using MoviesWebApi.Interfaces;
    using MoviesWebApi.Repositories;
    using MoviesWebApi.UnitOfWork;
    using MoviesWebAPI.Domain;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using RestSharp;
    using RestSharp.Deserializers;
    using RestSharp.Serializers;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                var httpResolver = new NinjectHttpDependencyResolver(kernel);
                GlobalConfiguration.Configuration.DependencyResolver = httpResolver;
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
         
            kernel.Bind<DbContext>().To<MovieContext>();
            kernel.Bind<IDeserializer, ISerializer>().To<SerializerRepositoty>().InSingletonScope();
            kernel.Bind<ICacheService>().To<MemoryCacheRepository>();
            kernel.Bind<ILogger>().To<LoggerRepository>();
            //kernel.Bind<RestClient>().To<ClientRepository>();
            kernel.Bind(typeof(RestClient)).To(typeof(ClientRepository));
            kernel.Bind<IMovieRepository>().To<MovieRepository>();
            kernel.Bind<IDisposable>().To<UnitOfWork>();


            //kernel.Bind(typeof(ILogRepository)).To(typeof(Data.LogRepository));
            //kernel.Bind(typeof(IExternalAPI)).To(typeof(ExternalAPI));
            //kernel.Bind<WhateverClass>().ToSelf().InSingletonScope()



        }


    }


    public class NinjectHttpDependencyResolver : IDependencyResolver, IDependencyScope
    {
        private readonly IKernel _kernel;
        public NinjectHttpDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }
        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
            //Do nothing
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
    }
}