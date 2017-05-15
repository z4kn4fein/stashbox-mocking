using System;
using Rhino.Mocks;
using Rhino.Mocks.Exceptions;
using Stashbox.Mocking.Tests;
using Xunit;

namespace Stashbox.Mocking.Rhino.Mocks.Tests
{
    public class StashRhinoTest
    {
        [Fact]
        public void StashRhinoTest_SameMock()
        {
            using (var mock = StashRhino.Create())
            {
                var m1 = mock.Mock<IDep>();
                var m2 = mock.Mock<IDep>();
                var m3 = mock.Mock<IDep2>();
                var test = mock.Get<TestObj>();

                Assert.Same(m1, m2);
                Assert.Same(m1, test.Dep);
                Assert.Same(m3, test.Dep2);
            }
        }

        [Fact]
        public void StashRhinoTest_GetTest_GlobalStrict()
        {
            using (var mock = StashRhino.Create())
            {
                mock.Strict<IDep>().Expect(x => x.Test());
                mock.Strict<IDep>().Expect(x => x.Test2());
                mock.Strict<IDep2>().Expect(x => x.Test1()).Return(null);
                var m = mock.Get<TestObj>();
                m.Test();
                m.Test1();
            }
        }

        [Fact]
        public void StashRhinoTest_GetTest_GlobalStrict_Fail()
        {
            Assert.Throws<ExpectationViolationException>(() =>
            {
                using (var mock = StashRhino.Create())
                {
                    mock.Strict<IDep>().Expect(x => x.Test());
                    mock.Strict<IDep>().Expect(x => x.Test2());
                    mock.Strict<IDep2>();
                    var m = mock.Get<TestObj>();
                    m.Test();
                    m.Test1();
                }
            });
        }

        [Fact]
        public void StashRhinoTest_GetTest_Strict()
        {
            using (var mock = StashRhino.Create())
            {
                mock.Strict<IDep>().Expect(x => x.Test());
                mock.Strict<IDep>().Expect(x => x.Test2());
                var m = mock.Get<TestObj>();
                m.Test();
                m.Test1();
            }
        }

        [Fact]
        public void StashRhinoTest_GetTest_Strict_Fail()
        {
            Assert.Throws<ExpectationViolationException>(() =>
            {
                using (var mock = StashRhino.Create())
                {
                    mock.Strict<IDep>().Expect(x => x.Test());
                    var m = mock.Get<TestObj>();
                    m.Test();
                }
            });
        }

        [Fact]
        public void StashRhinoTest_GetTest_Loose()
        {
            using (var mock = StashRhino.Create())
            {
                mock.Mock<IDep>().Expect(x => x.Test());
                var m = mock.Get<TestObj>();
                m.Test();
                m.Test1();
            }
        }

        [Fact]
        public void StashRhinoTest_GetTest_ConstructorSelection()
        {
            using (var mock = StashRhino.Create())
            {
                var m = MockRepository.GenerateStub<IDep>();
                var test = mock.GetWithConstructorArgs<TestObj>(m);

                Assert.Same(m, test.Dep);
                Assert.Null(test.Dep2);
            }
        }

        [Fact]
        public void StashRhinoTest_GetTest_ConstructorSelection_WithAny()
        {
            using (var mock = StashRhino.Create())
            {
                var m = MockRepository.GenerateStub<IDep>();
                var test = mock.GetWithConstructorArgs<TestObj>(m, StashArg.Any<IDep2>());

                Assert.Same(m, test.Dep);
                Assert.NotNull(test.Dep2);
            }
        }

        [Fact]
        public void StashRhinoTest_GetTest_ConstructorSelection_WithAny_NonMockabe()
        {
            using (var mock = StashRhino.Create())
            {
                Assert.Throws<NonMockableTypeException>(() =>
                    mock.GetWithConstructorArgs<TestObj>(StashArg.Any<IDep>(), StashArg.Any<IDep2>(), StashArg.Any<string>()));
            }
        }

        [Fact]
        public void StashRhinoTest_GetTest_ConstructorSelection_WithOverride()
        {
            using (var mock = StashRhino.Create())
            {
                var m = MockRepository.GenerateStub<IDep>();
                var test = mock.GetWithParamOverrides<IDep, TestObj>(m);

                Assert.Same(m, test.Dep);
                Assert.NotNull(test.Dep2);
            }
        }

        [Fact]
        public void StashRhinoTest_GetTest_ConstructorSelection_WithOverride_NonGeneric()
        {
            using (var mock = StashRhino.Create())
            {
                var m = MockRepository.GenerateStub<IDep>();
                var test = mock.GetWithParamOverrides<TestObj>(m);

                Assert.Same(m, test.Dep);
                Assert.NotNull(test.Dep2);
            }
        }

        [Fact]
        public void StashRhinoTest_Mock_WithArg()
        {
            using (var mock = StashRhino.Create())
            {
                var arg = new DepArg();
                var m = mock.Mock<DepWithArg>(arg);

                Assert.Same(arg, m.Dep);
            }
        }
    }
}
