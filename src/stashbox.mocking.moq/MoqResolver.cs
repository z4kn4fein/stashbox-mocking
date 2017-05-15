using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Moq;
using Stashbox.Entity;
using Stashbox.Infrastructure;
using Stashbox.Infrastructure.Resolution;

namespace Stashbox.Mocking.Moq
{
    internal class MoqResolver : Resolver
    {
        private readonly MockRepository repository;
        private readonly ISet<Type> requestedTypes;

        private static readonly MethodInfo CreateMockMethod = typeof(MoqResolver).GetMethod(nameof(CreateMock), BindingFlags.NonPublic | BindingFlags.Instance);

        public MoqResolver(MockRepository repository, ISet<Type> requestedTypes)
        {
            this.repository = repository;
            this.requestedTypes = requestedTypes;
        }

        public override Expression GetExpression(IContainerContext containerContext, TypeInformation typeInfo, ResolutionInfo resolutionInfo)
        {
            var method = CreateMockMethod.MakeGenericMethod(typeInfo.Type);
            return Expression.Call(Expression.Constant(this), method);
        }

        public override bool CanUseForResolution(IContainerContext containerContext, TypeInformation typeInfo, ResolutionInfo resolutionInfo) =>
            !this.requestedTypes.Contains(typeInfo.Type) && typeInfo.Type.CanMock();

        private TService CreateMock<TService>() where TService : class
        {
            var mock = this.repository.Create<TService>();
            return mock.Object;
        }
    }
}
