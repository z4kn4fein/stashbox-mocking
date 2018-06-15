using NSubstitute;
using Stashbox.Entity;
using Stashbox.Resolution;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Stashbox.Mocking.NSubstitute
{
    internal class NSubstituteResolver : ResolverBase
    {
        public NSubstituteResolver(ISet<Type> requestedTypes)
            : base(requestedTypes)
        { }

        protected override Expression GetExpressionInternal(IContainerContext containerContext, TypeInformation typeInfo, ResolutionContext resolutionInfo) =>
            Expression.Constant(Substitute.For(new[] { typeInfo.Type }, new object[] { }));
    }
}
