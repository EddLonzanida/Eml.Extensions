using System.ComponentModel.Composition;

namespace Assembly1NetFull
{
    [Export]
    public class Class2 : ClassBase
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
