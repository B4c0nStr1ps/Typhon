using Sharpmake;

namespace Ty
{
    [Sharpmake.Generate]
    public class Sandbox : Ty.Module
    {
        public Sandbox()
            : base()
        {
            myType = ModuleType.Executable;
            mySolutionFolder = "Exes";
        }
    }
}