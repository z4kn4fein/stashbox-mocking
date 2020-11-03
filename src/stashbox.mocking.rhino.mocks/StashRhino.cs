using Rhino.Mocks;
using Stashbox.Resolution;

namespace Stashbox.Mocking.Rhino.Mocks
{
    /// <summary>
    /// Represents the RhinoMocks integration for Stashbox.
    /// </summary>
    public class StashRhino : MockingBase
    {
        private StashRhino(bool useAutoMock, IStashboxContainer container)
            : base(useAutoMock, container)
        {
            if (useAutoMock)
                base.Container.RegisterResolver(new RhinoMocksResolver(RequestedTypes));
        }

        /// <summary>
        /// Creates a <see cref="StashRhino"/> instance.
        /// </summary>
        /// <param name="useAutoMock">If true, the container resolves unknown types automatically as mock.</param>
        /// <param name="container">An optional preconfigured container.</param>
        /// <returns></returns>
        public static StashRhino Create(bool useAutoMock = true, IStashboxContainer container = null) =>
            new StashRhino(useAutoMock, container);

        /// <summary>
        /// Creates a dynamic mock and registers it into the contaier.
        /// </summary>
        /// <typeparam name="TService">The type of the mock.</typeparam>
        /// <param name="onlyIfAlreadyExists">If true, the mock will be registered only, if there is an already existing service with the same type in the container.</param>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>The mock object. If <paramref name="onlyIfAlreadyExists"/> set to true and the type doesn't exist already in the container, null will be returned.</returns>
        public TService Mock<TService>(bool onlyIfAlreadyExists = false, params object[] args) where TService : class
        {
            if (base.Container.IsRegistered<TService>() && base.MockedTypes.Contains(typeof(TService)))
                return base.Container.Resolve<TService>();

            if (onlyIfAlreadyExists && !base.Container.IsRegistered<TService>())
                return null;

            var mock = MockRepository.GenerateMock<TService>(args);

            base.Container.ReMap<TService>(c => c.WithInstance(mock).WithFinalizer(m => m.VerifyAllExpectations()));
            base.MockedTypes.Add(typeof(TService));
            return mock;
        }

        /// <summary>
        /// Creates a strict mock and registers it into the contaier.
        /// </summary>
        /// <typeparam name="TService">The type of the mock.</typeparam>
        /// <param name="onlyIfAlreadyExists">If true, the mock will be registered only, if there is an already existing service with the same type in the container.</param>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>The mock object. If <paramref name="onlyIfAlreadyExists"/> set to true and the type doesn't exist already in the container, null will be returned.</returns>
        public TService Strict<TService>(bool onlyIfAlreadyExists = false, params object[] args) where TService : class
        {
            if (base.Container.IsRegistered<TService>() && base.MockedTypes.Contains(typeof(TService)))
                return base.Container.Resolve<TService>();

            if (onlyIfAlreadyExists && !base.Container.IsRegistered<TService>())
                return null;

            var mock = MockRepository.GenerateStrictMock<TService>(args);

            base.Container.ReMap<TService>(c => c.WithInstance(mock).WithFinalizer(m => m.VerifyAllExpectations()));
            base.MockedTypes.Add(typeof(TService));
            return mock;
        }

        /// <summary>
        /// Creates a partial mock and registers it into the contaier.
        /// </summary>
        /// <typeparam name="TService">The type of the mock.</typeparam>
        /// <param name="onlyIfAlreadyExists">If true, the mock will be registered only, if there is an already existing service with the same type in the container.</param>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>The mock object. If <paramref name="onlyIfAlreadyExists"/> set to true and the type doesn't exist already in the container, null will be returned.</returns>
        public TService Partial<TService>(bool onlyIfAlreadyExists = false, params object[] args) where TService : class
        {
            if (base.Container.IsRegistered<TService>() && base.MockedTypes.Contains(typeof(TService)))
                return base.Container.Resolve<TService>();

            if (onlyIfAlreadyExists && !base.Container.IsRegistered<TService>())
                return null;

            var mock = MockRepository.GeneratePartialMock<TService>(args);

            base.Container.ReMap<TService>(c => c.WithInstance(mock).WithFinalizer(m => m.VerifyAllExpectations()));
            base.MockedTypes.Add(typeof(TService));
            return mock;
        }

        /// <summary>
        /// Creates a simple stub and registers it into the contaier.
        /// </summary>
        /// <typeparam name="TService">The type of the stub.</typeparam>
        /// <param name="onlyIfAlreadyExists">If true, the mock will be registered only, if there is an already existing service with the same type in the container.</param>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>The mock object. If <paramref name="onlyIfAlreadyExists"/> set to true and the type doesn't exist already in the container, null will be returned.</returns>
        public TService Stub<TService>(bool onlyIfAlreadyExists = false, params object[] args) where TService : class
        {
            if (base.Container.IsRegistered<TService>() && base.MockedTypes.Contains(typeof(TService)))
                return base.Container.Resolve<TService>();

            if (onlyIfAlreadyExists && !base.Container.IsRegistered<TService>())
                return null;

            var mock = MockRepository.GenerateStub<TService>(args);

            base.Container.ReMap<TService>(c => c.WithInstance(mock).WithFinalizer(m => m.VerifyAllExpectations()));
            base.MockedTypes.Add(typeof(TService));
            return mock;
        }
    }
}
