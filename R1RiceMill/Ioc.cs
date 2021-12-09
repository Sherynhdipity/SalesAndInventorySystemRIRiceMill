using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace R1RiceMill
{
    public sealed class Ioc
    {
        private static readonly Lazy<Ioc> ioc;

        static Ioc()
        {
            ioc = new Lazy<Ioc>(() => new Ioc());
        }

        private readonly ServiceCollection services;

        private ServiceProvider provider;

        private Ioc()
        {
            services = new ServiceCollection();
            provider = null;
        }

        public static Ioc Default => ioc.Value;

        public Ioc AddSingleton<T>() where T : class
        {
            services.AddSingleton<T>();
            return this;
        }

        public Ioc AddSingleton<T>(Func<IServiceProvider, T> implementationFactory) where T : class
        {
            services.AddSingleton<T>(implementationFactory);
            return this;
        }

        public Ioc AddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            services.AddSingleton<TService, TImplementation>();
            return this;
        }

        public void Build()
        {
            provider = services.BuildServiceProvider();
        }

        public T GetInstance<T>()
        {
            return provider.GetService<T>();
        }
    }
}
