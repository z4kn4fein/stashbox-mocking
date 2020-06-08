using Stashbox.Resolution;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Stashbox.Mocking
{
    internal abstract class ResolverBase : IResolver
    {
        private readonly ISet<Type> requestedTypes;

        protected ResolverBase(ISet<Type> requestedTypes)
        {
            this.requestedTypes = requestedTypes;
        }

        public Expression GetExpression(IResolutionStrategy resolutionStrategy, TypeInformation typeInfo, ResolutionContext resolutionContext) =>
            this.GetExpressionInternal(typeInfo, resolutionContext);

        public bool CanUseForResolution(TypeInformation typeInfo, ResolutionContext resolutionContext) =>
            !this.requestedTypes.Contains(typeInfo.Type) && typeInfo.Type.CanMock();

        protected abstract Expression GetExpressionInternal(TypeInformation typeInfo, ResolutionContext resolutionContext);
    }
}
