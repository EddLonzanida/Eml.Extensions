using System.Composition;

namespace Assembly1
{
    [Export]
    public class Class1 : ClassBase1
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
