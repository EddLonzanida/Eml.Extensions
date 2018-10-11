using System.ComponentModel.Composition;

namespace Assembly2NetFull
{
    [Export]
    public class Class1 : ClassBase
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
