using Stashbox.Entity;
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

        public ResolverBase(ISet<Type> requestedTypes)
        {
            this.requestedTypes = requestedTypes;
        }

        public Expression GetExpression(IContainerContext containerContext, TypeInformation typeInfo, ResolutionContext resolutionContext) => 
            this.GetExpressionInternal(containerContext, typeInfo, resolutionContext);

        public bool CanUseForResolution(IContainerContext containerContext, TypeInformation typeInfo, ResolutionContext resolutionContext) =>
            !this.requestedTypes.Contains(typeInfo.Type) && typeInfo.Type.CanMock();

        protected abstract Expression GetExpressionInternal(IContainerContext containerContext, TypeInformation typeInfo, ResolutionContext resolutionContext);
    }
}
