using FakeItEasy.Sdk;
using Stashbox.Resolution;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Stashbox.Mocking.FakeItEasy
{
    internal class FakeItEasyResolver : ResolverBase
    {
        public FakeItEasyResolver(ISet<Type> requestedTypes)
            : base(requestedTypes)
        { }

        protected override Expression GetExpressionInternal(TypeInformation typeInfo, ResolutionContext resolutionInfo) =>
            Expression.Constant(Create.Dummy(typeInfo.Type));
    }
}
