using Rocks;
using Rocks.Exceptions;
using Stashbox.Mocking.Tests;
using System;
using Xunit;

namespace Stashbox.Mocking.Rocks.Tests
{
    public class StashRocksTests
    {
        [Fact]
        public void StashRocksTests_SameMock()
        {
            using (var mock = StashRocks.Create())
            {
                var m1 = mock.Make<IDep>();
                var m2 = mock.Make<IDep>();
                var m3 = mock.Make<IDep2>();

                var test = mock.Get<TestObj>();

                Assert.Same(m1, m2);
                Assert.Same(m1, test.Dep);
                Assert.Same(m3, test.Dep2);
            }
        }

        [Fact]
        public void StashRocksTests_GetTest_GlobalStrict()
        {
            using (var mock = StashRocks.Create())
            {
                mock.Mock<IDep>().Handle(x => x.Test());
                mock.Mock<IDep>().Handle(x => x.Test2());
                mock.Mock<IDep2>().Handle(x => x.Test1()).Returns(new object());

                mock.MakeAll();

                var m = mock.Get<TestObj>();
                m.Test();
                m.Test1();
            }
        }

        [Fact]
        public void StashRocksTests_GetTest_GlobalStrict_Fail()
        {
            using (var mock = StashRocks.Create())
            {
                mock.Mock<IDep>().Handle(x => x.Test());
                mock.Mock<IDep>().Handle(x => x.Test2());
                mock.Mock<IDep2>();

                mock.MakeAll();

                var m = mock.Get<TestObj>();
                m.Test();
                Assert.Throws<ExpectationException>(() => m.Test1());
            }
        }

        [Fact]
        public void StashRocksTests_GetTest_Strict()
        {
            using (var mock = StashRocks.Create())
            {
                mock.Mock<IDep>().Handle(x => x.Test());
                mock.Mock<IDep>().Handle(x => x.Test2());

                mock.MakeAll();

                var m = mock.Get<TestObj>();
                m.Test();
                m.Test1();
            }
        }

        [Fact]
        public void StashRocksTests_GetTest_Strict_Fail()
        {
            Assert.Throws<VerificationException>(() =>
            {
                using (var mock = StashRocks.Create())
                {
                    mock.Mock<IDep>().Handle(x => x.Test(), 2);

                    mock.MakeAll();

                    var m = mock.Get<TestObj>();
                    m.Test();
                }
            });
        }

        [Fact]
        public void StashRocksTests_GetTest_ConstructorSelection()
        {
            using (var mock = StashRocks.Create())
            {
                var m = Rock.Create<IDep>();
                var m1 = m.Make();
                var test = mock.GetWithConstructorArgs<TestObj>(m1);

                Assert.Same(m1, test.Dep);
                Assert.Null(test.Dep2);
            }
        }

        [Fact]
        public void StashRocksTests_GetTest_ConstructorSelection_WithAny()
        {
            using (var mock = StashRocks.Create())
            {
                var m = Rock.Create<IDep>();
                var m1 = m.Make();
                var test = mock.GetWithConstructorArgs<TestObj>(m1, StashArg.Any<IDep2>());

                Assert.Same(m1, test.Dep);
                Assert.NotNull(test.Dep2);
            }
        }

        [Fact]
        public void StashRocksTests_GetTest_ConstructorSelection_WithAny_NonMockabe()
        {
            using (var mock = StashRocks.Create())
            {
                Assert.Throws<NonMockableTypeException>(() =>
                    mock.GetWithConstructorArgs<TestObj>(StashArg.Any<IDep>(), StashArg.Any<IDep2>(), StashArg.Any<string>()));
            }
        }

        [Fact]
        public void StashRocksTests_GetTest_ConstructorSelection_WithOverride()
        {
            using (var mock = StashRocks.Create())
            {
                var m = Rock.Create<IDep>();
                var m1 = m.Make();
                var test = mock.GetWithParamOverrides<IDep, TestObj>(m1);

                Assert.Same(m1, test.Dep);
                Assert.NotNull(test.Dep2);
            }
        }

        [Fact]
        public void StashRocksTests_GetTest_ConstructorSelection_WithOverride_NonGeneric()
        {
            using (var mock = StashRocks.Create())
            {
                var m = Rock.Create<IDep>();
                var m1 = m.Make();
                var test = mock.GetWithParamOverrides<TestObj>(m1);

                Assert.Same(m1, test.Dep);
                Assert.NotNull(test.Dep2);
            }
        }

        [Fact]
        public void StashRocksTests_Mock_WithArg()
        {
            using (var mock = StashRocks.Create())
            {
                var arg = new DepArg();
                var m = mock.Mock<DepWithArg>();
                var m1 = m.Make(new object[] { arg });

                Assert.Same(arg, m1.Dep);
            }
        }
    }
}
