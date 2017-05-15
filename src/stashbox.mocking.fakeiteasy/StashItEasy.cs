using System;
using FakeItEasy;
using FakeItEasy.Creation;

namespace Stashbox.Mocking.FakeItEasy
{
    /// <summary>
    /// Represents the FakeItEasy integration for Stashbox.
    /// </summary>
    public class StashItEasy : MockingBase
    {
        private readonly Action<IFakeOptions> globalOptions;

        private StashItEasy(Action<IFakeOptions> globalOptions)
            : base(new StashboxContainer(config => config.WithUnknownTypeResolution()))
        {
            this.globalOptions = globalOptions;
            base.Container.RegisterResolver(new FakeItEasyResolver(base.RequestedTypes));
        }

        /// <summary>
        /// Creates a <see cref="StashItEasy"/> instance.
        /// </summary>
        /// <param name="globalOptions">The global options for the created mocks.</param>
        /// <returns>The <see cref="StashItEasy"/> instance.</returns>
        public static StashItEasy Create(Action<IFakeOptions> globalOptions = null) =>
            new StashItEasy(globalOptions);

        /// <summary>
        /// Creates a fake object and registers it into the container.
        /// </summary>
        /// <typeparam name="TService">The type of the fake.</typeparam>
        /// <param name="optionsBuilder">The options for the created fake.</param>
        /// <returns>The fake object.</returns>
        public TService Fake<TService>(Action<IFakeOptions> optionsBuilder = null) where TService : class
        {
            if (base.Container.IsRegistered<TService>())
                return base.Container.Resolve<TService>();

            optionsBuilder = optionsBuilder ?? this.globalOptions;

            var fake = optionsBuilder == null ? A.Fake<TService>() : (TService)global::FakeItEasy.Sdk.Create.Fake(typeof(TService), optionsBuilder);
            base.Container.RegisterInstanceAs(fake);
            return fake;
        }
    }
}
