using BepInEx;
using Bepinject;

namespace ComputerModExample
{
    [BepInPlugin("tonimacaroni.computermodexample", "Computer Mod Example", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Zenjector.Install<MainInstaller>().OnProject();
        }
    }
}
