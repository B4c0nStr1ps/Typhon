using Sharpmake;

namespace Ty
{
    [Sharpmake.Generate]
    public class System : Ty.Module
    {
        public System()
            : base()
        {
            myType = ModuleType.Lib;
            mySolutionFolder = "Modules";
        }
    }
}