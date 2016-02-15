using Autodesk.Maya.OpenMaya;

[assembly: ExtensionPlugin(typeof(MayaCsi.MayaCsiPlugin), "Any")]
[assembly: MPxCommandClass(typeof(MayaCsi.MayaCsi), "csi")]

namespace MayaCsi
{
    public class MayaCsi : MPxCommand, IMPxCommand
    {
        public MayaCsi()
        {
            base.setUndoable(false);
        }

        public override void doIt(MArgList args)
        {
            CSharpInterpreter.MayaRootPath = @"D:\Program Files\Autodesk\Maya2015";

            var script = args.asString(0);
            MGlobal.displayInfo(script);
            var scriptArgs = new string[args.length];
            for (uint i = 1; i < args.length; ++i)
            {
                scriptArgs[i - 1] = args.asString(i);
                MGlobal.displayInfo(scriptArgs[i - 1]);
            }
            new CSharpInterpreter(script).Execute(scriptArgs);
        }
    }

    public class MayaCsiPlugin : IExtensionPlugin
    {
        public string GetMayaDotNetSdkBuildVersion()
        {
            return "201353";
        }

        public bool InitializePlugin()
        {
            return true;
        }

        public bool UninitializePlugin()
        {
            return true;
        }
    }
}
