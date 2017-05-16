using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Stashbox.Entity;
using FakeItEasy.Sdk;
using Stashbox.Infrastructure;

namespace Stashbox.Mocking.FakeItEasy
{
    internal class FakeItEasyResolver : ResolverBase
    {
        public FakeItEasyResolver(ISet<Type> requestedTypes)
            : base(requestedTypes)
        { }

        public override Expression GetExpression(IContainerContext containerContext, TypeInformation typeInfo, ResolutionInfo resolutionInfo) =>
            Expression.Constant(Create.Dummy(typeInfo.Type));
    }
}
