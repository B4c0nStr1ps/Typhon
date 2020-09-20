using Sharpmake;

[module: Sharpmake.Include("sharpmake/Common.sharpmake.cs")]
[module: Sharpmake.Include("sandbox/Sandbox.sharpmake.cs")]

public static class Project
{
    [Sharpmake.Generate]
    public class Solution : Ty.BaseSolution
    {
        public Solution()
            : base()
        {
            Name = "Sandbox";
        }

        public override void RegisterModules()
        {
            RegisterModule<Ty.Sandbox>();
        }
    }

    [Sharpmake.Main]
    public static void SharpmakeMain(Sharpmake.Arguments arguments)
    {
        arguments.Generate<Solution>();
    }
}