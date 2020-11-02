using System;
using FakeItEasy;
using Stashbox.Mocking.Tests;
using Xunit;

namespace Stashbox.Mocking.FakeItEasy.Tests
{
    public class StashItEasyTests
    {
        [Fact]
        public void StashItEasyTests_SameMock()
        {
            using (var mock = StashItEasy.Create())
            {
                var m1 = mock.Fake<IDep>();
                var m2 = mock.Fake<IDep>();
                var m3 = mock.Fake<IDep2>();
                var test = mock.Get<TestObj>();

                Assert.Same(m1, m2);
                Assert.Same(m1, test.Dep);
                Assert.Same(m3, test.Dep2);
            }
        }

        [Fact]
        public void StashItEasyTests_GetTest_Strict()
        {
            using (var mock = StashItEasy.Create(x => x.Strict()))
            {
                var f1 = mock.Fake<IDep>();
                var f2 = mock.Fake<IDep2>();

                A.CallTo(() => f1.Test()).DoesNothing();
                A.CallTo(() => f1.Test2()).DoesNothing();
                A.CallTo(() => f2.Test1()).Returns(null);
                var m = mock.Get<TestObj>();
                m.Test();
                m.Test1();

                A.CallTo(() => f1.Test()).MustHaveHappened();
                A.CallTo(() => f1.Test2()).MustHaveHappened();
                A.CallTo(() => f2.Test1()).MustHaveHappened();
            }
        }

        [Fact]
        public void StashItEasyTests_GetTest_GlobalStrict_Fail()
        {
            using (var mock = StashItEasy.Create(x => x.Strict()))
            {
                var f = mock.Fake<IDep>();
                mock.Fake<IDep2>();
                A.CallTo(() => f.Test()).DoesNothing();
                A.CallTo(() => f.Test2()).DoesNothing();

                var m = mock.Get<TestObj>();
                m.Test();
                Assert.Throws<ExpectationException>(() => m.Test1());
            }
        }

        [Fact]
        public void StashItEasyTests_GetTest_Strict_Fail()
        {
            using (var mock = StashItEasy.Create())
            {
                mock.Fake<IDep>(x => x.Strict());
                var m = mock.Get<TestObj>();
                Assert.Throws<ExpectationException>(() => m.Test());
            }
        }

        [Fact]
        public void StashItEasyTests_GetTest_Loose()
        {
            using (var mock = StashItEasy.Create())
            {
                var f = mock.Fake<IDep>();
                var m = mock.Get<TestObj>();
                m.Test();
                m.Test1();

                A.CallTo(() => f.Test()).MustHaveHappened();
            }
        }

        [Fact]
        public void StashItEasyTests_GetTest_ConstructorSelection()
        {
            using (var mock = StashItEasy.Create())
            {
                var m = A.Fake<IDep>();
                var test = mock.GetWithConstructorArgs<TestObj>(m);

                Assert.Same(m, test.Dep);
                Assert.Null(test.Dep2);
            }
        }

        [Fact]
        public void StashItEasyTests_GetTest_ConstructorSelection_WithAny()
        {
            using (var mock = StashItEasy.Create())
            {
                var m = A.Fake<IDep>();
                var test = mock.GetWithConstructorArgs<TestObj>(m, StashArg.Any<IDep2>());

                Assert.Same(m, test.Dep);
                Assert.NotNull(test.Dep2);
            }
        }

        [Fact]
        public void StashItEasyTests_GetTest_ConstructorSelection_WithAny_NonMockabe()
        {
            using (var mock = StashItEasy.Create())
            {
                Assert.Throws<NonMockableTypeException>(() =>
                    mock.GetWithConstructorArgs<TestObj>(StashArg.Any<IDep>(), StashArg.Any<IDep2>(), StashArg.Any<string>()));
            }
        }

        [Fact]
        public void StashItEasyTests_GetTest_ConstructorSelection_WithOverride()
        {
            using (var mock = StashItEasy.Create())
            {
                var m = A.Fake<IDep>();
                var test = mock.GetWithParamOverrides<IDep, TestObj>(m);

                Assert.Same(m, test.Dep);
                Assert.NotNull(test.Dep2);
            }
        }

        [Fact]
        public void StashItEasyTests_GetTest_ConstructorSelection_WithOverride_NonGeneric()
        {
            using (var mock = StashItEasy.Create())
            {
                var m = A.Fake<IDep>();
                var test = mock.GetWithParamOverrides<TestObj>(m);

                Assert.Same(m, test.Dep);
                Assert.NotNull(test.Dep2);
            }
        }

        [Fact]
        public void StashItEasyTests_Mock_WithArg()
        {
            using (var mock = StashItEasy.Create())
            {
                var arg = new DepArg();
                var m = mock.Fake<DepWithArg>(x => x.WithArgumentsForConstructor(new[] { arg }));

                Assert.Same(arg, m.Dep);
            }
        }

        [Fact]
        public void StashItEasyTests_Mock_Only_If_Exists()
        {
            using (var mock = StashItEasy.Create())
            {
                Assert.Null(mock.Fake<ITest>(onlyIfAlreadyExists: true));

                mock.Container.Register<ITest, TestObj>();

                var r = mock.Fake<ITest>(onlyIfAlreadyExists: true);
                Assert.NotNull(r);
                Assert.IsNotType<TestObj>(r);
            }
        }
    }
}
