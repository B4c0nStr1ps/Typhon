using System;
using System.Collections.Generic;
using Sharpmake;

namespace Ty
{
    public static class Context
    {
        public static string OutputRootDirectory { get; set; }
        public static string IntermediateRootDirectory { get; set; }
    }


    [Sharpmake.Generate]
    public class Module : Sharpmake.Project
    {
        public enum ModuleType
        {
            Lib,
            Executable
        }
        public string mySolutionFolder = string.Empty;

        public string OutputDir = string.Empty;

        public string IntermediateDir = string.Empty;

        public ModuleType myType = ModuleType.Lib;

        public Module() : base()
        {
            OutputDir = Context.OutputRootDirectory;
            IntermediateDir = Context.IntermediateRootDirectory;

            AddTargets(new Target(
                Platform.win64,
                DevEnv.vs2019,
                Optimization.Debug | Optimization.Release));
        }

        [Sharpmake.Configure]
        public void ConfigureAll(Configuration conf, Target target)
        {
            conf.SolutionFolder = @"[project.mySolutionFolder]";
            conf.Output = myType == ModuleType.Lib ? Configuration.OutputType.Lib : Configuration.OutputType.Exe;
            conf.ProjectName = myType == ModuleType.Executable ? "[project.Name]_Exec" : "[project.Name]_Module";
            conf.ProjectFileName = myType == ModuleType.Executable ? "[project.Name]_Exec" : "[project.Name]_Module";
            conf.ProjectPath = @"[project.SourceRootPath]";
            conf.TargetPath = @"[project.OutputDir]/[project.Name]/[conf.Name]_[target.Platform]_[target.DevEnv]";
            conf.IntermediatePath = @"[project.IntermediateDir]/[project.Name]/[conf.Name]_[target.Platform]_[target.DevEnv]";

            // if not set, no precompile option will be used.
            conf.PrecompHeader = null;
            conf.PrecompSource = null;
        }
    }

    [Sharpmake.Generate]
    public class BaseSolution : Sharpmake.Solution
    {
        public string RootPath = @"[solution.SharpmakeCsPath]";

        public BaseSolution()
            : base()
        {
            Context.OutputRootDirectory = ResolveString(@"[solution.SharpmakeCsPath]/build/sm_out");
            Context.IntermediateRootDirectory = ResolveString(@"[solution.SharpmakeCsPath]/build/sm_tmp");

            AddTargets(new Target(
                Platform.win64,
                DevEnv.vs2019,
                Optimization.Debug | Optimization.Release));
        }

        public virtual void RegisterModules() { }

        [Sharpmake.Configure]
        public void Configure(Configuration conf, Target target)
        {
            conf.SolutionPath = @"[solution.RootPath]";
            conf.SolutionFileName = "[solution.Name]_[target.DevEnv]";

            if (target.Platform == Platform.win64)
                conf.PlatformName = "win64";

            RegisterModules();

            foreach (var project in myModules)
            {
                conf.AddProject(project, target);
            }
        }

        public void RegisterModule<ModuleType>()
        where ModuleType : Ty.Module
        {
            myModules.Add(typeof(ModuleType));
        }

        private List<Type> myModules = new List<Type>();
    }
}