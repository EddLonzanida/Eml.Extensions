using System.Composition;

namespace Assembly2
{
    [Export]
    public class Class2 : ClassBase2
    {
        [Import]
        public int PropertyInt { get; set; }

        [Import]
        public int PropertyString { get; set; }

        public void Method1(string param1, int param2)
        {
        }
    }
}
