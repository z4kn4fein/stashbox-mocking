using System;
using System.Collections.Concurrent;
using System.Reflection;
using Rocks;
using Rocks.Options;

namespace Stashbox.Mocking.Rocks
{
    /// <summary>
    /// Represents the Rocks integration of Stashbox.
    /// </summary>
    public class StashRocks : MockingBase
    {
        private readonly RockOptions globalOptions;
        private readonly ConcurrentDictionary<Type, object> repository;
        private static readonly MethodInfo MakeMethod = typeof(StashRocks).GetMethod(nameof(RegisterRock), BindingFlags.NonPublic | BindingFlags.Instance);

        private StashRocks(RockOptions globalOptions)
            : base(new StashboxContainer(config => config.WithUnknownTypeResolution()))
        {
            this.globalOptions = globalOptions;
            this.repository = new ConcurrentDictionary<Type, object>();
            base.Container.RegisterResolver(new RocksResolver(base.RequestedTypes));
        }

        /// <summary>
        /// Creates a <see cref="StashRocks"/> instance.
        /// </summary>
        /// <param name="globalOptions">The global options of the created mocks.</param>
        /// <returns>The <see cref="StashRocks"/> instance.</returns>
        public static StashRocks Create(RockOptions globalOptions = null) =>
            new StashRocks(globalOptions);

        /// <summary>
        /// Creates a mock object and registers it into the container.
        /// </summary>
        /// <typeparam name="TService">The type of the mock.</typeparam>
        /// <param name="options">The options of the mock.</param>
        /// <returns>The created mock object.</returns>
        public IRock<TService> Mock<TService>(RockOptions options = null) where TService : class
        {
            if (this.repository.TryGetValue(typeof(TService), out object value))
                return (IRock<TService>)value;

            options = options ?? this.globalOptions;

            var mock = options == null ? Rock.Create<TService>() : Rock.Create<TService>(options);
            this.repository.AddOrUpdate(typeof(TService), mock, (key, val) => mock);
            return mock;
        }

        /// <summary>
        /// Creates a simple dummy object and registers it into the container.
        /// </summary>
        /// <typeparam name="TService">The type of the mock.</typeparam>
        /// <param name="options">The options of the mock.</param>
        /// <returns>The created mock object.</returns>
        public TService Make<TService>(RockOptions options = null) where TService : class
        {
            if (base.Container.IsRegistered<TService>())
                return base.Container.Resolve<TService>();

            options = options ?? this.globalOptions;

            var mock = options == null ? Rock.Make<TService>() : Rock.Make<TService>(options);
            base.Container.RegisterInstanceAs(mock);
            return mock;
        }

        /// <summary>
        /// Calls the <see cref="IRock{T}.Make()"/> method on all the requested mock objects and registers them into the container.
        /// </summary>
        public void MakeAll()
        {
            foreach (var value in this.repository)
            {
                var method = MakeMethod.MakeGenericMethod(value.Key);
                method.Invoke(this, new object[] { value.Value });
            }
        }

        private void RegisterRock<TService>(IRock<TService> rock) where TService : class
        {
            var maked = rock.Make();
            base.Container.RegisterInstanceAs(maked, finalizerDelegate: m => rock.Verify());
        }
    }
}
