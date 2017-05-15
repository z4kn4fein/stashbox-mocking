using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Stashbox.Entity;
using System.Reflection;
using FakeItEasy.Sdk;
using Stashbox.Infrastructure;
using Stashbox.Infrastructure.Resolution;

namespace Stashbox.Mocking.FakeItEasy
{
    internal class FakeItEasyResolver : Resolver
    {
        private readonly ISet<Type> requestedTypes;

        public FakeItEasyResolver(ISet<Type> requestedTypes)
        {
            this.requestedTypes = requestedTypes;
        }

        public override Expression GetExpression(IContainerContext containerContext, TypeInformation typeInfo, ResolutionInfo resolutionInfo) =>
            Expression.Constant(Create.Dummy(typeInfo.Type));

        public override bool CanUseForResolution(IContainerContext containerContext, TypeInformation typeInfo, ResolutionInfo resolutionInfo) =>
            !this.requestedTypes.Contains(typeInfo.Type) && typeInfo.Type.CanMock();
    }
}
