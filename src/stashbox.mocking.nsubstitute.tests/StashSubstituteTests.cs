using System;
using NSubstitute;
using NSubstitute.Exceptions;
using Stashbox.Mocking.Tests;
using Xunit;

namespace Stashbox.Mocking.NSubstitute.Tests
{
    public class StashSubstituteTests
    {
        [Fact]
        public void StashSubstituteTests_SameMock()
        {
            using (var mock = StashSubstitute.Create())
            {
                var m1 = mock.Sub<IDep>();
                var m2 = mock.Sub<IDep>();
                var m3 = mock.Sub<IDep2>();
                var test = mock.Get<TestObj>();

                Assert.Same(m1, m2);
                Assert.Same(m1, test.Dep);
                Assert.Same(m3, test.Dep2);
            }
        }

        [Fact]
        public void StashSubstituteTests_GetTest_Recieved_Success()
        {
            using (var mock = StashSubstitute.Create())
            {
                var m1 = mock.Sub<IDep>();
                var m2 = mock.Sub<IDep>();
                var m3 = mock.Sub<IDep2>();

                var m = mock.Get<TestObj>();
                m.Test();
                m.Test1();

                m1.Received().Test();
                m2.Received().Test2();
                m3.Received().Test1();
            }
        }

        [Fact]
        public void StashSubstituteTests_GetTest_Recieved_Fail()
        {
            using (var mock = StashSubstitute.Create())
            {
                var m1 = mock.Sub<IDep>();
                var m2 = mock.Sub<IDep>();
                var m3 = mock.Sub<IDep2>();
                var m = mock.Get<TestObj>();
                m.Test();

                m1.Received().Test();
                m2.Received().Test2();

                Assert.Throws<ReceivedCallsException>(() => m3.Received().Test1());
            }
        }

        [Fact]
        public void StashSubstituteTests_GetTest_ConstructorSelection()
        {
            using (var mock = StashSubstitute.Create())
            {
                var m = Substitute.For<IDep>();
                var test = mock.GetWithConstructorArgs<TestObj>(m);

                Assert.Same(m, test.Dep);
                Assert.Null(test.Dep2);
            }
        }

        [Fact]
        public void StashSubstituteTests_GetTest_ConstructorSelection_WithAny()
        {
            using (var mock = StashSubstitute.Create())
            {
                var m = Substitute.For<IDep>();
                var test = mock.GetWithConstructorArgs<TestObj>(m, StashArg.Any<IDep2>());

                Assert.Same(m, test.Dep);
                Assert.NotNull(test.Dep2);
            }
        }

        [Fact]
        public void StashSubstituteTests_GetTest_ConstructorSelection_WithAny_NonMockabe()
        {
            using (var mock = StashSubstitute.Create())
            {
                Assert.Throws<NonMockableTypeException>(() =>
                    mock.GetWithConstructorArgs<TestObj>(StashArg.Any<IDep>(), StashArg.Any<IDep2>(), StashArg.Any<string>()));
            }
        }

        [Fact]
        public void StashSubstituteTests_GetTest_ConstructorSelection_WithOverride()
        {
            using (var mock = StashSubstitute.Create())
            {
                var m = Substitute.For<IDep>();
                var test = mock.GetWithParamOverrides<IDep, TestObj>(m);

                Assert.Same(m, test.Dep);
                Assert.NotNull(test.Dep2);
            }
        }

        [Fact]
        public void StashSubstituteTests_GetTest_ConstructorSelection_WithOverride_NonGeneric()
        {
            using (var mock = StashSubstitute.Create())
            {
                var m = Substitute.For<IDep>();
                var test = mock.GetWithParamOverrides<TestObj>(m);

                Assert.Same(m, test.Dep);
                Assert.NotNull(test.Dep2);
            }
        }

        [Fact]
        public void StashSubstituteTests_Mock_WithArg()
        {
            using (var mock = StashSubstitute.Create())
            {
                var arg = new DepArg();
                var m = mock.Sub<DepWithArg>(arg);

                Assert.Same(arg, m.Dep);
            }
        }

        [Fact]
        public void StashSubstituteTests_Mock_WithoutArg_NonGeneric()
        {
            using (var mock = StashSubstitute.Create())
            {
                var m = mock.Sub(new[] { typeof(IDep) });

                Assert.IsAssignableFrom<IDep>(m);
            }
        }

        [Fact]
        public void StashSubstituteTests_Mock_WithArg_NonGeneric()
        {
            using (var mock = StashSubstitute.Create())
            {
                var arg = new DepArg();
                var m = mock.Sub(new[] { typeof(DepWithArg) }, arg);

                Assert.Same(arg, ((DepWithArg)m).Dep);
            }
        }
    }
}
