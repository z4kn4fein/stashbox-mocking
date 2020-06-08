using System.Threading;
using Moq;

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

        private StashMoq(MockRepository repository, MockBehavior behavior, bool verifyAll)
            : base(new StashboxContainer(config => config.WithUnknownTypeResolution()))
        {
            this.repository = repository;
            this.behavior = behavior;
            this.verifyAll = verifyAll;
            base.Container.RegisterResolver(new MoqResolver(this.repository, base.RequestedTypes));
        }

        /// <summary>
        /// Creates a <see cref="StashMoq"/> instance.
        /// </summary>
        /// <param name="behavior">The global mock behavior used by the injected mocks.</param>
        /// <param name="verifyAll">If true the disposal of this object will trigger a .VerifyAll() on the mock repository.</param>
        /// <returns>The <see cref="StashMoq"/> instance.</returns>
        public static StashMoq Create(MockBehavior behavior = MockBehavior.Default, bool verifyAll = false) =>
            new StashMoq(new MockRepository(behavior), behavior, verifyAll);

        /// <summary>
        /// Creates a mock object and registers it into the container.
        /// </summary>
        /// <typeparam name="TService">The type of the mock.</typeparam>
        /// <param name="mockBehavior">The behavior of the mock.</param>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>The mock object.</returns>
        public Mock<TService> Mock<TService>(MockBehavior? mockBehavior = null, params object[] args) where TService : class
        {
            if (!base.Container.IsRegistered<TService>())
            {
                var mBehavior = mockBehavior ?? this.behavior;

                var mock = args.Length > 0 ? this.repository.Create<TService>(mBehavior, args) :
                    this.repository.Create<TService>(mBehavior);

                base.Container.RegisterInstance(mock.Object);
                return mock;
            }

            var resolved = (IMocked<TService>)base.Container.Resolve<TService>();
            return resolved.Mock;
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
