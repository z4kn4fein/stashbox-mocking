using NSubstitute;
using Stashbox.Resolution;
using Stashbox.Utils;
using System;

namespace Stashbox.Mocking.NSubstitute
{
    /// <summary>
    /// Represents the NSubstitute integration for Stashbox.
    /// </summary>
    public class StashSubstitute : MockingBase
    {
        private StashSubstitute(bool useAutoMock)
            : base(useAutoMock)
        {
            if (useAutoMock)
                base.Container.RegisterResolver(new NSubstituteResolver(RequestedTypes));
        }

        /// <summary>
        /// Creates a <see cref="StashSubstitute"/> instance.
        /// </summary>
        /// <param name="useAutoMock">If true, the container resolves unknown types automatically as mock.</param>
        /// <returns>The <see cref="StashSubstitute"/> instance.</returns>
        public static StashSubstitute Create(bool useAutoMock = true) => new StashSubstitute(useAutoMock);

        /// <summary>
        /// Creates a Partial substitute and registers it into the container.
        /// </summary>
        /// <typeparam name="TService">The type of the substitute.</typeparam>
        /// <param name="onlyIfAlreadyExists">If true, the mock will be registered only, if there is an already existing service with the same type in the container.</param>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>The mock object. If <paramref name="onlyIfAlreadyExists"/> set to true and the type doesn't exist already in the container, null will be returned.</returns>
        public TService Partial<TService>(bool onlyIfAlreadyExists = false, params object[] args) where TService : class
        {
            if (base.Container.IsRegistered<TService>() && base.MockedTypes.Contains(typeof(TService)))
                return base.Container.Resolve<TService>();

            if (onlyIfAlreadyExists && !base.Container.IsRegistered<TService>())
                return null;

            var mock = Substitute.ForPartsOf<TService>(args);

            base.Container.ReMap<TService>(c => c.WithInstance(mock));
            base.MockedTypes.Add(typeof(TService));
            return mock;
        }

        /// <summary>
        /// Creates a substitute and registers it into the container.
        /// </summary>
        /// <typeparam name="TService">The type of the substitute.</typeparam>
        /// <param name="args">The constructor arguments.</param>
        /// <param name="onlyIfAlreadyExists">If true, the mock will be registered only, if there is an already existing service with the same type in the container.</param>
        /// <returns>The mock object. If <paramref name="onlyIfAlreadyExists"/> set to true and the type doesn't exist already in the container, null will be returned.</returns>
        public TService Sub<TService>(bool onlyIfAlreadyExists = false, params object[] args) where TService : class
        {
            if (base.Container.IsRegistered<TService>() && base.MockedTypes.Contains(typeof(TService)))
                return base.Container.Resolve<TService>();

            if (onlyIfAlreadyExists && !base.Container.IsRegistered<TService>())
                return null;

            var mock = Substitute.For<TService>(args);

            base.Container.ReMap<TService>(c => c.WithInstance(mock));
            base.MockedTypes.Add(typeof(TService));
            return mock;
        }

        /// <summary>
        /// Creates a substitute and registers it into the container.
        /// </summary>
        /// <typeparam name="TService1">The type of the substitute.</typeparam>
        /// <typeparam name="TService2">The second type of the substitute.</typeparam>
        /// <param name="onlyIfAlreadyExists">If true, the mock will be registered only, if there is an already existing service with the same type in the container.</param>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>The mock object. If <paramref name="onlyIfAlreadyExists"/> set to true and the type doesn't exist already in the container, null will be returned.</returns>
        public TService1 Sub<TService1, TService2>(bool onlyIfAlreadyExists = false, params object[] args)
            where TService1 : class
            where TService2 : class
        {
            if (base.Container.IsRegistered<TService1>() && base.MockedTypes.Contains(typeof(TService1)))
                return base.Container.Resolve<TService1>();

            if (onlyIfAlreadyExists && !base.Container.IsRegistered<TService1>())
                return null;

            var mock = Substitute.For<TService1, TService2>(args);

            base.Container.ReMap<TService1>(c => c.WithInstance(mock));
            base.MockedTypes.Add(typeof(TService1));
            return mock;
        }

        /// <summary>
        /// Creates a substitute and registers it into the container.
        /// </summary>
        /// <typeparam name="TService1">The type of the substitute.</typeparam>
        /// <typeparam name="TService2">The second type of the substitute.</typeparam>
        /// <typeparam name="TService3">The third type of the substitute.</typeparam>
        /// <param name="onlyIfAlreadyExists">If true, the mock will be registered only, if there is an already existing service with the same type in the container.</param>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>The mock object. If <paramref name="onlyIfAlreadyExists"/> set to true and the type doesn't exist already in the container, null will be returned.</returns>
        public TService1 Sub<TService1, TService2, TService3>(bool onlyIfAlreadyExists = false, params object[] args)
            where TService1 : class
            where TService2 : class
            where TService3 : class
        {
            if (base.Container.IsRegistered<TService1>() && base.MockedTypes.Contains(typeof(TService1)))
                return base.Container.Resolve<TService1>();

            if (onlyIfAlreadyExists && !base.Container.IsRegistered<TService1>())
                return null;

            var mock = Substitute.For<TService1, TService2, TService3>(args);

            base.Container.ReMap<TService1>(c => c.WithInstance(mock));
            base.MockedTypes.Add(typeof(TService1));
            return mock;
        }

        /// <summary>
        /// Creates a substitute and registers it into the container.
        /// </summary>
        /// <param name="interfaceTypes">The implemented types of the substitute.</param>
        /// <param name="onlyIfAlreadyExists">If true, the mock will be registered only, if there is an already existing service with the same type in the container.</param>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>The mock object. If <paramref name="onlyIfAlreadyExists"/> set to true and the type doesn't exist already in the container, null will be returned.</returns>
        public object Sub(Type[] interfaceTypes, bool onlyIfAlreadyExists = false, params object[] args)
        {
            Shield.EnsureNotNull(interfaceTypes, nameof(interfaceTypes));
            Shield.EnsureTrue(interfaceTypes.Length > 0, "interfaceTypes.Length is 0");

            var regType = interfaceTypes[0];

            if (base.Container.IsRegistered(regType) && base.MockedTypes.Contains(regType))
                return base.Container.Resolve(regType);

            if (onlyIfAlreadyExists && !base.Container.IsRegistered(regType))
                return null;

            var mock = Substitute.For(interfaceTypes, args);

            base.Container.ReMap(regType, c => c.WithInstance(mock));
            base.MockedTypes.Add(regType);
            return mock;
        }
    }
}
