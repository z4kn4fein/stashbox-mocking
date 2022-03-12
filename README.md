# stashbox-mocking 
[![Appveyor build status](https://img.shields.io/appveyor/ci/pcsajtai/stashbox-mocking/master.svg?logo=appveyor&logoColor=white)](https://ci.appveyor.com/project/pcsajtai/stashbox-mocking/branch/master) 
[![GitHub Workflow Status](https://img.shields.io/github/workflow/status/z4kn4fein/stashbox-mocking/Build%20on%20Linux%20and%20macOS?logo=GitHub)](https://github.com/z4kn4fein/stashbox-mocking/actions/workflows/linux-macOS-CI.yml) 
[![Sourcelink](https://img.shields.io/badge/sourcelink-enabled-brightgreen.svg)](https://github.com/dotnet/sourcelink)

Mocking framework integrations for [Stashbox](https://github.com/z4kn4fein/stashbox) that provide automatic mock creation for your services in unit tests.

[Moq](https://github.com/moq/moq4) | [FakeItEasy](https://github.com/FakeItEasy/FakeItEasy) | [NSubstitute](https://github.com/nsubstitute/NSubstitute) | [RhinoMocks](https://github.com/hibernating-rhinos/rhino-mocks)
--- | --- | --- | ---
[![NuGet Version](https://buildstats.info/nuget/Stashbox.Moq)](https://www.nuget.org/packages/Stashbox.Moq/) | [![NuGet Version](https://buildstats.info/nuget/Stashbox.FakeItEasy)](https://www.nuget.org/packages/Stashbox.FakeItEasy/) | [![NuGet Version](https://buildstats.info/nuget/Stashbox.NSubstitute)](https://www.nuget.org/packages/Stashbox.NSubstitute/) | [![NuGet Version](https://buildstats.info/nuget/Stashbox.RhinoMocks)](https://www.nuget.org/packages/Stashbox.RhinoMocks/)

## Moq
You can use the auto mock framework by creating a `StashMoq` instance wrapped in a using statement, on its disposal it will call `Verify()` on all the configured expectations.
```c#
//begin a test scope
using (var stash = StashMoq.Create())
{
    //configure a mock dependency
    stash.Mock<IDependency>().Setup(m => m.Test()).Returns("test");
    
    //configure the mock again
    //this call will get the same mock back as the first request
    stash.Mock<IDependency>().Setup(m => m.Test2());
    
    //get the tested service filled with auto created mocks (except the configured ones)
    var service = stash.Get<IService>();
    
    //call the tested method, imagine that this will invoke the Test() method of an IDependency
    var result = service.Test();
    
    //check the result
    Assert.Equal("test", result);
    
} //StashMoq will call the Verify() method on all configured expectations on its dispose
```
> You can also set the `verifyAll` parameter of `StashMoq` with that it will call the `VerifyAll()` on the used mock repository.
`StashMoq.Create(verifyAll: true)`

### Mock behavior
You can set which mock behavior should be used by the framework by default.
```c#
using (var stash = StashMoq.Create(MockBehavior.Strict)) //the default will be strict
{
    //this mock will be strict
    stash.Mock<IDependency>().Setup(m => m.Test()).Returns("test");
    
    //you can also override the default config, this mock will be loose
    stash.Mock<IDependency2>(MockBehavior.Loose).Setup(...);
}
```

## FakeItEasy
You can use the auto mock framework by creating a `StashItEasy` instance wrapped in a using statement.
```c#
//begin a test scope
using (var stash = StashItEasy.Create())
{
    //configure a mock dependency
    var fake = stash.Fake<IDependency>();
    
    //configure the call
    A.CallTo(() => fake.Test()).Returns("test");
    
    //get the tested service filled with auto created fakes (except the configured ones)
    var service = stash.Get<IService>();
    
    //call the tested method, imagine that this will invoke the Test() method of the IDependency
    var result = service.Test();
    
    //check the call
    A.CallTo(() => fake.Test()).MustHaveHappened();
    
    //check the result
    Assert.Equal("test", result);    
}
```

### Options
You can set what fake options should be used by the framework by default.
```c#
using (var stash = StashItEasy.Create(x => x.Strict())) //the default will be strict
{
    //this fake will be strict
    stash.Fake<IDependency>();
    
    //you can also override the default config
    stash.Fake<IDependency>(x => x.Implements<IDependency3>());
}
```

## NSubstitute
You can use the auto mock framework by creating a `StashSubstitute` instance wrapped in a using statement.
```c#
//begin a test scope
using (var stash = StashSubstitute.Create())
{
    //configure a mock dependency
    var sub = stash.Sub<IDependency>(); //for multiple interface implementations use the overloads of this method
    sub.Test().Returns("test");
    
    //get the tested service filled with auto created mocks (except the configured ones)
    var service = stash.Get<IService>();
    
    //call the tested method, imagine that this will invoke the Test() method of an IDependency
    var result = service.Test();
    
    //check the call
    sub.Recieved().Test();
    
    //check the result
    Assert.Equal("test", result);   
}
```
> You can also get a partial mock with the `stash.Partial<IDependency>()` call.

## RhinoMocks
You can use the auto mock framework by creating a `StashRhino` instance wrapped in a using statement, on its disposal it will call `VerifyAllExpectations()` on all the configured expectations.
```c#
//begin a test scope
using (var stash = StashRhino.Create())
{
    //configure a mock dependency
    stash.Mock<IDependency>().Expect(x => x.Test()).Returns("test");
    
    //configure the mock again
    //this call will get the same mock back as the first request
    stash.Mock<IDependency>().Expect(m => m.Test2());
    
    //get the tested service filled with auto created mocks (except the configured ones)
    var service = stash.Get<IService>();
    
    //call the tested method, imagine that this will invoke the Test() method of an IDependency
    var result = service.Test();
    
    //check the result
    Assert.Equal("test", result);   
    
} //StashRhino will call the VerifyAllExpectations() method on all configured expectations on its dispose
```

### Mock types
You can also request different mock types from `StashRhino`:
```c#
using (var stash = StashRhino.Create())
{
    //this will create a dynamic mock
    stash.Mock<IDependency>();
    
    //this will create a strict mock
    stash.Strict<IDependency>();
    
    //this will create a partial mock
    stash.Partial<IDependency>();
}
```

## Further things that each package offers
- All package allows the service instantiation by a selected constructor with pre-evaluated arguments:
```c#
var service = stash.GetWithConstructorArgs<Service>(mockObject1, mockObject2);

//you can also use a placeholder argument where you don't want to set a concrete object
var service = stash.GetWithConstructorArgs<Service>(StashArg.Any<IMock>(), mockObject2);
```
> If you use an argument placeholder with a non-mockable type, the framework will throw a `NonMockableTypeException`.

- All package allows the dependency override with pre-evaluated dependencies:
```c#
//this will inject the `mockObject1` into the created `Service` everywhere it fits by its type
var service = stash.GetWithParamOverrides<Service>(mockObject1);
```
