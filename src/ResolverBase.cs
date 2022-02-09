using Stashbox.Resolution;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Stashbox.Mocking
{
    internal abstract class ResolverBase : IServiceResolver
    {
        private readonly ISet<Type> requestedTypes;

        protected ResolverBase(ISet<Type> requestedTypes)
        {
            this.requestedTypes = requestedTypes;
        }

        public ServiceContext GetExpression(IResolutionStrategy resolutionStrategy, TypeInformation typeInfo, ResolutionContext resolutionContext) =>
            new ServiceContext(this.GetExpressionInternal(typeInfo, resolutionContext), null);

        public bool CanUseForResolution(TypeInformation typeInfo, ResolutionContext resolutionContext) =>
            !resolutionContext.NullResultAllowed && !this.requestedTypes.Contains(typeInfo.Type) && typeInfo.Type.CanMock();

        protected abstract Expression GetExpressionInternal(TypeInformation typeInfo, ResolutionContext resolutionContext);
    }
}
