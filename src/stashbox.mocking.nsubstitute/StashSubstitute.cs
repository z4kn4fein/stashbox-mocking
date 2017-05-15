using System;
using NSubstitute;
using Stashbox.Mocking.FakeItEasy;
using Stashbox.Utils;

namespace Stashbox.Mocking.NSubstitute
{
    /// <summary>
    /// Represents the NSubstitute integration for Stashbox.
    /// </summary>
    public class StashSubstitute : MockingBase
    {
        private StashSubstitute()
            : base(new StashboxContainer(config => config.WithUnknownTypeResolution()))
        {
            base.Container.RegisterResolver(new NSubstituteResolver(base.RequestedTypes));
        }

        /// <summary>
        /// Creates a <see cref="StashSubstitute"/> instance.
        /// </summary>
        /// <returns>The <see cref="StashSubstitute"/> instance.</returns>
        public static StashSubstitute Create() => new StashSubstitute();

        /// <summary>
        /// Creates a Partial substitute and registers it into the container.
        /// </summary>
        /// <typeparam name="TService">The type of the substitute.</typeparam>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>The substitute object.</returns>
        public TService Partial<TService>(params object[] args) where TService : class
        {
            if (!base.Container.IsRegistered<TService>())
            {
                var mock = Substitute.ForPartsOf<TService>(args);

                base.Container.RegisterInstanceAs(mock);
                return mock;
            }

            return base.Container.Resolve<TService>();
        }

        /// <summary>
        /// Creates a substitute and registers it into the container.
        /// </summary>
        /// <typeparam name="TService">The type of the substitute.</typeparam>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>The substitute object.</returns>
        public TService Sub<TService>(params object[] args) where TService : class
        {
            if (!base.Container.IsRegistered<TService>())
            {
                var mock = Substitute.For<TService>(args);

                base.Container.RegisterInstanceAs(mock);
                return mock;
            }

            return base.Container.Resolve<TService>();
        }

        /// <summary>
        /// Creates a substitute and registers it into the container.
        /// </summary>
        /// <typeparam name="TService1">The type of the substitute.</typeparam>
        /// <typeparam name="TService2">The second type of the substitute.</typeparam>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>The substitute object.</returns>
        public TService1 Sub<TService1, TService2>(params object[] args)
            where TService1 : class
            where TService2 : class
        {
            if (!base.Container.IsRegistered<TService1>())
            {
                var mock = Substitute.For<TService1, TService2>(args);

                base.Container.RegisterInstanceAs(mock);
                return mock;
            }

            return base.Container.Resolve<TService1>();
        }

        /// <summary>
        /// Creates a substitute and registers it into the container.
        /// </summary>
        /// <typeparam name="TService1">The type of the substitute.</typeparam>
        /// <typeparam name="TService2">The second type of the substitute.</typeparam>
        /// <typeparam name="TService3">The third type of the substitute.</typeparam>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>The substitute object.</returns>
        public TService1 Sub<TService1, TService2, TService3>(params object[] args)
            where TService1 : class
            where TService2 : class
            where TService3 : class
        {
            if (!base.Container.IsRegistered<TService1>())
            {
                var mock = Substitute.For<TService1, TService2, TService3>(args);

                base.Container.RegisterInstanceAs(mock);
                return mock;
            }

            return base.Container.Resolve<TService1>();
        }

        /// <summary>
        /// Creates a substitute and registers it into the container.
        /// </summary>
        /// <param name="interfaceTypes">The implemented types of the substitute.</param>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>The substitute object.</returns>
        public object Sub(Type[] interfaceTypes, params object[] args)
        {
            Shield.EnsureNotNull(interfaceTypes, nameof(interfaceTypes));
            Shield.EnsureTrue(interfaceTypes.Length > 0, "interfaceTypes.Length is 0");

            var regType = interfaceTypes[0];

            if (!base.Container.IsRegistered(regType))
            {
                var mock = Substitute.For(interfaceTypes, args);

                base.Container.RegisterInstance(regType, mock);
                return mock;
            }

            return base.Container.Resolve(regType);
        }
    }
}
