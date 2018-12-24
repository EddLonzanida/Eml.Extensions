using System.Composition;

namespace Assembly1
{
    [Export]
    public class Class2 : ClassBase1
    {
        [Import]
        public int PropertyInt { get; set; }

        [Import]
        public int PropertyString { get; set; }

        public void Method2(string param1, int param2)
        {
        }
    }
}
