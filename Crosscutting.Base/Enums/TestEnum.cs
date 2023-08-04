namespace Crosscutting.Base.Enums
{
    internal class TestEnum : Enumeration
    {
        public static TestEnum Tipo1 = new(1, nameof(Tipo1));
        public static TestEnum Tipo2 = new(2, nameof(Tipo2));
        public static TestEnum Tipe3 = new(3, nameof(Tipe3));

        public TestEnum(int id, string name) : base(id, name)
        {
        }
    }
}
