namespace Stashbox.Mocking.Tests
{
    public interface ITest
    {
        void Test();
        object Test1();
    }

    public class TestObj : ITest
    {
        public IDep Dep { get; }
        public IDep2 Dep2 { get; }
        public string SDep { get; }

        public TestObj(IDep dep)
        {
            this.Dep = dep;
        }

        public TestObj(IDep dep, IDep2 dep2)
        {
            this.Dep = dep;
            this.Dep2 = dep2;
        }

        public TestObj(IDep dep, IDep2 dep2, string sDep)
        {
            this.Dep = dep;
            this.Dep2 = dep2;
            this.SDep = sDep;
        }

        public void Test()
        {
            this.Dep.Test();
            this.Dep.Test2();
        }

        public object Test1()
        {
            return this.Dep2.Test1();
        }
    }

    public class DepWithArg
    {
        public IArg Dep { get; }

        public DepWithArg(IArg dep)
        {
            this.Dep = dep;
        }
    }

    public class DepArg : IArg { }

    public interface IArg { }

    public interface IDep
    {
        void Test();
        void Test2();
    }

    public interface IDep2
    {
        object Test1();
    }
}
