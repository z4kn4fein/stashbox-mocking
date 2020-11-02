using Moq;
using Stashbox.Mocking.Tests;
using Xunit;
using System;

namespace Stashbox.Mocking.Moq.Tests
{
    public class StashMoqTests
    {
        [Fact]
        public void StashMoqTests_SameMock()
        {
            using (var mock = StashMoq.Create())
            {
                var m1 = mock.Mock<IDep>();
                var m2 = mock.Mock<IDep>();
                var m3 = mock.Mock<IDep2>();
                var test = mock.Get<TestObj>();

                Assert.Same(m1, m2);
                Assert.Same(m1.Object, test.Dep);
                Assert.Same(m3.Object, test.Dep2);
            }
        }

        [Fact]
        public void StashMoqTests_GetTest_GlobalStrict()
        {
            using (var mock = StashMoq.Create(MockBehavior.Strict))
            {
                mock.Mock<IDep>().Setup(x => x.Test());
                mock.Mock<IDep>().Setup(x => x.Test2());
                mock.Mock<IDep2>().Setup(x => x.Test1()).Returns(new object());
                var m = mock.Get<TestObj>();
                m.Test();
                m.Test1();
            }
        }

        [Fact]
        public void StashMoqTests_GetTest_GlobalStrict_LocalLoose()
        {
            using (var mock = StashMoq.Create(MockBehavior.Strict))
            {
                mock.Mock<IDep>(MockBehavior.Loose).Setup(x => x.Test());
                mock.Mock<IDep2>().Setup(x => x.Test1()).Returns(new object());
                var m = mock.Get<TestObj>();
                m.Test();
                m.Test1();
            }
        }

        [Fact]
        public void StashMoqTests_GetTest_GlobalStrict_Fail()
        {
            using (var mock = StashMoq.Create(MockBehavior.Strict))
            {
                mock.Mock<IDep>().Setup(x => x.Test());
                mock.Mock<IDep>().Setup(x => x.Test2());
                mock.Mock<IDep2>();
                var m = mock.Get<TestObj>();
                m.Test();
                Assert.Throws<MockException>(() => m.Test1());
            }
        }

        [Fact]
        public void StashMoqTests_GetTest_Strict()
        {
            using (var mock = StashMoq.Create())
            {
                mock.Mock<IDep>(MockBehavior.Strict).Setup(x => x.Test());
                mock.Mock<IDep>().Setup(x => x.Test2());
                var m = mock.Get<TestObj>();
                m.Test();
                m.Test1();
            }
        }

        [Fact]
        public void StashMoqTests_GetTest_Strict_Fail()
        {
            using (var mock = StashMoq.Create())
            {
                mock.Mock<IDep>(MockBehavior.Strict).Setup(x => x.Test());
                var m = mock.Get<TestObj>();
                Assert.Throws<MockException>(() => m.Test());
            }
        }

        [Fact]
        public void StashMoqTests_GetTest_Loose()
        {
            using (var mock = StashMoq.Create())
            {
                mock.Mock<IDep>().Setup(x => x.Test()).Verifiable();
                var m = mock.Get<TestObj>();
                m.Test();
                m.Test1();
            }
        }

        [Fact]
        public void StashMoqTests_GetTest_ConstructorSelection()
        {
            using (var mock = StashMoq.Create())
            {
                var m = new Mock<IDep>();
                var test = mock.GetWithConstructorArgs<TestObj>(m.Object);

                Assert.Same(m.Object, test.Dep);
                Assert.Null(test.Dep2);
            }
        }

        [Fact]
        public void StashMoqTests_GetTest_ConstructorSelection_WithAny()
        {
            using (var mock = StashMoq.Create())
            {
                var m = new Mock<IDep>();
                var test = mock.GetWithConstructorArgs<TestObj>(m.Object, StashArg.Any<IDep2>());

                Assert.Same(m.Object, test.Dep);
                Assert.NotNull(test.Dep2);
            }
        }

        [Fact]
        public void StashMoqTests_GetTest_ConstructorSelection_WithAny_NonMockabe()
        {
            using (var mock = StashMoq.Create())
            {
                Assert.Throws<NonMockableTypeException>(() =>
                    mock.GetWithConstructorArgs<TestObj>(StashArg.Any<IDep>(), StashArg.Any<IDep2>(), StashArg.Any<string>()));
            }
        }

        [Fact]
        public void StashMoqTests_GetTest_ConstructorSelection_WithOverride()
        {
            using (var mock = StashMoq.Create())
            {
                var m = new Mock<IDep>();
                var test = mock.GetWithParamOverrides<IDep, TestObj>(m.Object);

                Assert.Same(m.Object, test.Dep);
                Assert.NotNull(test.Dep2);
            }
        }

        [Fact]
        public void StashMoqTests_GetTest_ConstructorSelection_WithOverride_NonGeneric()
        {
            using (var mock = StashMoq.Create())
            {
                var m = new Mock<IDep>();
                var test = mock.GetWithParamOverrides<TestObj>(m.Object);

                Assert.Same(m.Object, test.Dep);
                Assert.NotNull(test.Dep2);
            }
        }

        [Fact]
        public void StashMoqTests_Mock_WithArg()
        {
            using (var mock = StashMoq.Create())
            {
                var arg = new DepArg();
                var m = mock.Mock<DepWithArg>(args: arg);

                Assert.Same(arg, m.Object.Dep);
            }
        }

        [Fact]
        public void StashMoqTests_Mock_Only_If_Exists()
        {
            using (var mock = StashMoq.Create())
            {
                Assert.Null(mock.Mock<ITest>(onlyIfAlreadyExists: true));

                mock.Container.Register<ITest, TestObj>();

                var r = mock.Mock<ITest>(onlyIfAlreadyExists: true);
                Assert.NotNull(r);                
                Assert.IsNotType<TestObj>(r);
            }
        }
    }
}
