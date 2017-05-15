using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Stashbox.Entity;
using System.Reflection;
using NSubstitute;
using Stashbox.Infrastructure;
using Stashbox.Infrastructure.Resolution;

namespace Stashbox.Mocking.FakeItEasy
{
    internal class NSubstituteResolver : Resolver
    {
        private readonly ISet<Type> requestedTypes;

        public NSubstituteResolver(ISet<Type> requestedTypes)
        {
            this.requestedTypes = requestedTypes;
        }

        public override Expression GetExpression(IContainerContext containerContext, TypeInformation typeInfo, ResolutionInfo resolutionInfo) =>
            Expression.Constant(Substitute.For(new[] { typeInfo.Type }, new object[] { }));

        public override bool CanUseForResolution(IContainerContext containerContext, TypeInformation typeInfo, ResolutionInfo resolutionInfo) =>
            !this.requestedTypes.Contains(typeInfo.Type) && typeInfo.Type.CanMock();
    }
}
