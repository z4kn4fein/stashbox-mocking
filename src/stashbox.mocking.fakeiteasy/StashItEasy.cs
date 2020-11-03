using System;
using FakeItEasy;
using FakeItEasy.Creation;
using Stashbox.Resolution;

namespace Stashbox.Mocking.FakeItEasy
{
    /// <summary>
    /// Represents the FakeItEasy integration for Stashbox.
    /// </summary>
    public class StashItEasy : MockingBase
    {
        private readonly Action<IFakeOptions> globalOptions;

        private StashItEasy(Action<IFakeOptions> globalOptions, bool useAutoMock, IStashboxContainer container)
            : base(useAutoMock, container)
        {
            this.globalOptions = globalOptions;

            if (useAutoMock)
                base.Container.RegisterResolver(new FakeItEasyResolver(RequestedTypes));
        }

        /// <summary>
        /// Creates a <see cref="StashItEasy"/> instance.
        /// </summary>
        /// <param name="globalOptions">The global options for the created mocks.</param>
        /// <param name="useAutoMock">If true, the container resolves unknown types automatically as mock.</param>
        /// <param name="container">An optional preconfigured container.</param>
        /// <returns>The <see cref="StashItEasy"/> instance.</returns>
        public static StashItEasy Create(Action<IFakeOptions> globalOptions = null, bool useAutoMock = true, IStashboxContainer container = null) =>
            new StashItEasy(globalOptions, useAutoMock, container);

        /// <summary>
        /// Creates a fake object and registers it into the container.
        /// </summary>
        /// <typeparam name="TService">The type of the fake.</typeparam>
        /// <param name="optionsBuilder">The options for the created fake.</param>
        /// <param name="onlyIfAlreadyExists">If true, the mock will be registered only, if there is an already existing service with the same type in the container.</param>
        /// <returns>The mock object. If <paramref name="onlyIfAlreadyExists"/> set to true and the type doesn't exist already in the container, null will be returned.</returns>
        public TService Fake<TService>(Action<IFakeOptions> optionsBuilder = null, bool onlyIfAlreadyExists = false) where TService : class
        {
            if (base.Container.IsRegistered<TService>() && base.MockedTypes.Contains(typeof(TService)))
                return base.Container.Resolve<TService>();

            if (onlyIfAlreadyExists && !base.Container.IsRegistered<TService>())
                return null;

            optionsBuilder = optionsBuilder ?? this.globalOptions;

            var fake = optionsBuilder == null ? A.Fake<TService>() : (TService)global::FakeItEasy.Sdk.Create.Fake(typeof(TService), optionsBuilder);
            base.Container.ReMap<TService>(c => c.WithInstance(fake));
            base.MockedTypes.Add(typeof(TService));
            return fake;
        }
    }
}
