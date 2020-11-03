using Stashbox.Resolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Stashbox.Mocking
{
    /// <summary>
    /// Represents a base class which holds shared functionality for every integration.
    /// </summary>
    public abstract class MockingBase : IDisposable
    {
        private int disposed;

        /// <summary>
        /// The used <see cref="IStashboxContainer"/> instance.
        /// </summary>
        public IStashboxContainer Container { get; }

        /// <summary>
        /// The requested NON-mock types.
        /// </summary>
        protected ISet<Type> RequestedTypes { get; }

        /// <summary>
        /// The mock types.
        /// </summary>
        protected ISet<Type> MockedTypes { get; }

        /// <summary>
        /// Constructs a <see cref="MockingBase"/>.
        /// </summary>
        /// <param name="autoMock">If true, the container resolves unknown types automatically as mock.</param>
        /// <param name="container">An optional preconfigured container.</param>
        protected MockingBase(bool autoMock = true, IStashboxContainer container = null)
        {
            this.RequestedTypes = new HashSet<Type>();
            this.MockedTypes = new HashSet<Type>();
            this.Container = container ?? new StashboxContainer();

            if(autoMock)
                this.Container.Configure(c => c.WithUnknownTypeResolution());
        }

        /// <summary>
        /// Gets a service from the container which's constructor will be selected by the given arguments.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>The resolved service.</returns>
        public TService GetWithConstructorArgs<TService>(params object[] args) where TService : class
        {
            this.AddRequestedType(typeof(TService));

            var length = args.Length;
            if (length <= 0) return this.Container.Resolve<TService>();

            var arguments = new object[length];
            for (var i = 0; i < length; i++)
            {
                var arg = args[i];
                if (arg is Type type)
                    arguments[i] = this.Container.Resolve(type);
                else
                    arguments[i] = arg;
            }

            this.Container.Register<TService>(context => context.WithConstructorByArguments(arguments));
            return this.Container.Resolve<TService>();
        }

        /// <summary>
        /// Gets a service from the container with dependency overrides.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="args">The dependency overrides.</param>
        /// <returns>The resolved service.</returns>
        public TService GetWithParamOverrides<TService>(params object[] args) where TService : class
        {
            this.AddRequestedType(typeof(TService));

            var factory = this.Container.ResolveFactory(typeof(TService), parameterTypes: args.Select(arg => arg.GetType()).ToArray());
            return (TService)factory.DynamicInvoke(args);
        }

        /// <summary>
        /// Gets a service from the container with dependency overrides.
        /// </summary>
        /// <typeparam name="TParam1">The type of the dependency override.</typeparam>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="param1">The overridden dependency.</param>
        /// <returns>The resolved service.</returns>
        public TService GetWithParamOverrides<TParam1, TService>(TParam1 param1) where TService : class
        {
            this.AddRequestedType(typeof(TService));
            return this.Container.ResolveFactory<TParam1, TService>()(param1);
        }

        /// <summary>
        /// Gets a service from the container with dependency overrides.
        /// </summary>
        /// <typeparam name="TParam1">The type of the dependency override.</typeparam>
        /// <typeparam name="TParam2">The type of the dependency override.</typeparam>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="param1">The overridden dependency.</param>
        /// <param name="param2">The overridden dependency.</param>
        /// <returns>The resolved service.</returns>
        public TService GetWithParamOverrides<TParam1, TParam2, TService>(TParam1 param1, TParam2 param2) where TService : class
        {
            this.AddRequestedType(typeof(TService));
            return this.Container.ResolveFactory<TParam1, TParam2, TService>()(param1, param2);
        }

        /// <summary>
        /// Gets a service from the container with dependency overrides.
        /// </summary>
        /// <typeparam name="TParam1">The type of the dependency override.</typeparam>
        /// <typeparam name="TParam2">The type of the dependency override.</typeparam>
        /// <typeparam name="TParam3">The type of the dependency override.</typeparam>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="param1">The overridden dependency.</param>
        /// <param name="param2">The overridden dependency.</param>
        /// <param name="param3">The overridden dependency.</param>
        /// <returns>The resolved service.</returns>
        public TService GetWithParamOverrides<TParam1, TParam2, TParam3, TService>(TParam1 param1, TParam2 param2, TParam3 param3) where TService : class
        {
            this.AddRequestedType(typeof(TService));
            return this.Container.ResolveFactory<TParam1, TParam2, TParam3, TService>()(param1, param2, param3);
        }

        /// <summary>
        /// Gets a service from the container with dependency overrides.
        /// </summary>
        /// <typeparam name="TParam1">The type of the dependency override.</typeparam>
        /// <typeparam name="TParam2">The type of the dependency override.</typeparam>
        /// <typeparam name="TParam3">The type of the dependency override.</typeparam>
        /// <typeparam name="TParam4">The type of the dependency override.</typeparam>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="param1">The overridden dependency.</param>
        /// <param name="param2">The overridden dependency.</param>
        /// <param name="param3">The overridden dependency.</param>
        /// <param name="param4">The overridden dependency.</param>
        /// <returns>The resolved service.</returns>
        public TService GetWithParamOverrides<TParam1, TParam2, TParam3, TParam4, TService>(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4) where TService : class
        {
            this.AddRequestedType(typeof(TService));
            return this.Container.ResolveFactory<TParam1, TParam2, TParam3, TParam4, TService>()(param1, param2, param3, param4);
        }

        /// <summary>
        /// Gets a service from the container, filled with mock objects.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>The resolved service.</returns>
        public TService Get<TService>() where TService : class
        {
            this.AddRequestedType(typeof(TService));
            return this.Container.Resolve<TService>();
        }

        private void AddRequestedType(Type type)
        {
            if (!this.RequestedTypes.Contains(type))
                this.RequestedTypes.Add(type);
        }

        /// <summary>
        /// Disposes the auto mock integration object.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (Interlocked.CompareExchange(ref this.disposed, 1, 0) != 0 || !disposing) return;
            this.Container.Dispose();
        }

        /// <summary>
        /// Disposes the auto mock integration object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
