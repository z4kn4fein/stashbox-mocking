using System.Threading;
using Moq;
using Stashbox.Resolution;

namespace Stashbox.Mocking.Moq
{
    /// <summary>
    /// Represents the Moq integration for Stashbox.
    /// </summary>
    public class StashMoq : MockingBase
    {
        private readonly MockRepository repository;
        private readonly MockBehavior behavior;
        private readonly bool verifyAll;
        private int disposed;

        private StashMoq(MockRepository repository, MockBehavior behavior, bool verifyAll, bool useAutoMock, IStashboxContainer container)
            : base(useAutoMock, container)
        {
            this.repository = repository;
            this.behavior = behavior;
            this.verifyAll = verifyAll;

            if (useAutoMock)
                base.Container.RegisterResolver(new MoqResolver(repository, RequestedTypes));
        }

        /// <summary>
        /// Creates a <see cref="StashMoq"/> instance.
        /// </summary>
        /// <param name="behavior">The global mock behavior used by the injected mocks.</param>
        /// <param name="verifyAll">If true the disposal of this object will trigger a .VerifyAll() on the mock repository.</param>
        /// <param name="useAutoMock">If true, the container resolves unknown types automatically as mock.</param>
        /// <param name="container">An optional preconfigured container.</param>
        /// <returns>The <see cref="StashMoq"/> instance.</returns>
        public static StashMoq Create(MockBehavior behavior = MockBehavior.Default, bool verifyAll = false, bool useAutoMock = true, IStashboxContainer container = null) =>
            new StashMoq(new MockRepository(behavior), behavior, verifyAll, useAutoMock, container);

        /// <summary>
        /// Creates a mock object and registers it into the container.
        /// </summary>
        /// <typeparam name="TService">The type of the mock.</typeparam>
        /// <param name="mockBehavior">The behavior of the mock.</param>
        /// <param name="onlyIfAlreadyExists">If true, the mock will be registered only, if there is an already existing service with the same type in the container.</param>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>The mock object. If <paramref name="onlyIfAlreadyExists"/> set to true and the type doesn't exist already in the container, null will be returned.</returns>
        public Mock<TService> Mock<TService>(MockBehavior? mockBehavior = null, bool onlyIfAlreadyExists = false, params object[] args) where TService : class
        {
            if(base.Container.IsRegistered<TService>() && base.MockedTypes.Contains(typeof(TService)))
                return ((IMocked<TService>)base.Container.Resolve<TService>()).Mock;

            if (onlyIfAlreadyExists && !base.Container.IsRegistered<TService>())
                return null;

            var mBehavior = mockBehavior ?? this.behavior;
            var mock = args.Length > 0 ? this.repository.Create<TService>(mBehavior, args) :
                this.repository.Create<TService>(mBehavior);

            base.Container.ReMap<TService>(c => c.WithInstance(mock.Object));
            base.MockedTypes.Add(typeof(TService));
            return mock;
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (Interlocked.CompareExchange(ref this.disposed, 1, 0) != 0 || !disposing) return;

            try
            {
                if (this.verifyAll)
                    this.repository.VerifyAll();
                else
                    this.repository.Verify();
            }
            finally
            {
                base.Dispose(true);
            }
        }
    }
}
