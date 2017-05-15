using Rhino.Mocks;

namespace Stashbox.Mocking.Rhino.Mocks
{
    /// <summary>
    /// Represents the RhinoMocks integration for Stashbox.
    /// </summary>
    public class StashRhino : MockingBase
    {
        private StashRhino()
            : base(new StashboxContainer(config => config.WithUnknownTypeResolution()))
        {
            base.Container.RegisterResolver(new RhinoMocksResolver(base.RequestedTypes));
        }

        /// <summary>
        /// Creates a <see cref="StashRhino"/> instance.
        /// </summary>
        /// <returns></returns>
        public static StashRhino Create() =>
            new StashRhino();

        /// <summary>
        /// Creates a dynamic mock and registers it into the contaier.
        /// </summary>
        /// <typeparam name="TService">The type of the mock.</typeparam>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>The mock object.</returns>
        public TService Mock<TService>(params object[] args) where TService : class
        {
            if (base.Container.IsRegistered<TService>())
                return base.Container.Resolve<TService>();


            var mock = MockRepository.GenerateMock<TService>(args);
            base.Container.RegisterInstanceAs(mock, finalizerDelegate: m => m.VerifyAllExpectations());
            return mock;
        }

        /// <summary>
        /// Creates a strict mock and registers it into the contaier.
        /// </summary>
        /// <typeparam name="TService">The type of the mock.</typeparam>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>The mock object.</returns>
        public TService Strict<TService>(params object[] args) where TService : class
        {
            if (base.Container.IsRegistered<TService>())
                return base.Container.Resolve<TService>();


            var mock = MockRepository.GenerateStrictMock<TService>(args);
            base.Container.RegisterInstanceAs(mock, finalizerDelegate: m => m.VerifyAllExpectations());
            return mock;
        }

        /// <summary>
        /// Creates a partial mock and registers it into the contaier.
        /// </summary>
        /// <typeparam name="TService">The type of the mock.</typeparam>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>The mock object.</returns>
        public TService Partial<TService>(params object[] args) where TService : class
        {
            if (base.Container.IsRegistered<TService>())
                return base.Container.Resolve<TService>();


            var mock = MockRepository.GeneratePartialMock<TService>(args);
            base.Container.RegisterInstanceAs(mock, finalizerDelegate: m => m.VerifyAllExpectations());
            return mock;
        }

        /// <summary>
        /// Creates a simple stub and registers it into the contaier.
        /// </summary>
        /// <typeparam name="TService">The type of the stub.</typeparam>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>The stub object.</returns>
        public TService Stub<TService>(params object[] args) where TService : class
        {
            if (base.Container.IsRegistered<TService>())
                return base.Container.Resolve<TService>();


            var mock = MockRepository.GenerateStub<TService>(args);
            base.Container.RegisterInstanceAs(mock);
            return mock;
        }
    }
}
