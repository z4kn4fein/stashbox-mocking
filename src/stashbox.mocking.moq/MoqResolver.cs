using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Moq;
using Stashbox.Entity;
using Stashbox.Infrastructure;

namespace Stashbox.Mocking.Moq
{
    internal class MoqResolver : ResolverBase
    {
        private readonly MockRepository repository;

        private static readonly MethodInfo CreateMockMethod = typeof(MoqResolver).GetMethod(nameof(CreateMock), BindingFlags.NonPublic | BindingFlags.Instance);

        public MoqResolver(MockRepository repository, ISet<Type> requestedTypes)
            : base(requestedTypes)
        {
            this.repository = repository;
        }

        public override Expression GetExpression(IContainerContext containerContext, TypeInformation typeInfo, ResolutionInfo resolutionInfo)
        {
            var method = CreateMockMethod.MakeGenericMethod(typeInfo.Type);
            return Expression.Call(Expression.Constant(this), method);
        }

        private TService CreateMock<TService>() where TService : class
        {
            var mock = this.repository.Create<TService>();
            return mock.Object;
        }
    }
}
