using CaputillaMelonLoader;
using CaputillaMelonLoader.Utils;
using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppLocomotion;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(MainMod), ModInfo.NAME, ModInfo.VERSION, ModInfo.AUTHOR)]

namespace CaputillaMelonLoader
{
    internal class MainMod : MelonMod
    {
        private bool _hasInitialized;
        public static GameObject ModdedButton;

        public override void OnInitializeMelon()
        {
            ClassInjector.RegisterTypeInIl2Cpp<CaputillaHub>();
            ClassInjector.RegisterTypeInIl2Cpp<ControllerInputManager>();
        }

        public override void OnUpdate()
        {
            if (!_hasInitialized && Player.Instance != null)
            {
                _hasInitialized = true;
                OnGameInitialized();
                CaputillaHub.Instance.InvokeOnGameInitialized();
            }
        }

        private void OnGameInitialized()
        {
            GameObject componentHolder = new GameObject("Caputilla MelonLoader Component Holder");
            componentHolder.AddComponent<CaputillaHub>();
            componentHolder.AddComponent<ControllerInputManager>();

            // This is copy and pasted from Monky Caputilla but with permission. (I also made better)
            GameObject root = GameObject.Find("Global/Levels/ObjectNotInMaps/Stump/TableOffset/QueueBoard");
            
            var textObj = root.transform.Find("Text (TMP)");
            if (textObj != null && textObj.TryGetComponent(out TextMeshPro tmp))
                tmp.text = "|CASUAL\n\n|KING OF THE HILL\n\n|RISING LAVA\n\n|MODDED";
            
            Transform casualButton = root.transform.Find("Casual");
            Transform cube3 = root.transform.Find("Cube (2)");

            if (casualButton != null && cube3 != null)
            {
                ModdedButton = UnityEngine.Object.Instantiate(casualButton.gameObject, casualButton.parent);
                ModdedButton.name = "Modded";
                ModdedButton.transform.SetPositionAndRotation(cube3.position, casualButton.rotation);
                ModdedButton.transform.localScale = casualButton.localScale;

                cube3.gameObject.Kill();
                
                if (casualButton.TryGetComponent(out QueueSelect originalQueue) && ModdedButton.TryGetComponent(out QueueSelect newQueue))
                {
                    newQueue.button = ModdedButton.GetComponent<MeshRenderer>();
                    newQueue.redMat = originalQueue.redMat;
                    newQueue.defaultMat = originalQueue.defaultMat;
                    newQueue.queue = "MODDED";
                    ModdedButton.GetComponent<MeshRenderer>().material = FusionHub.selectedQueue.ToLower().Contains("modded") ? newQueue.redMat : newQueue.defaultMat;
                }
            }
        }
    }
}