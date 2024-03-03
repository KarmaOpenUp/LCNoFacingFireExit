using BepInEx;
using GameNetcodeStuff;
using HarmonyLib;

namespace NoFacingFireExitBase
{
    [BepInPlugin(modGUID, modName, modVersion)]

    public class NoFacingFireExitBase : BaseUnityPlugin
    {
        private const string modGUID = "Karma.LCNoFacingFireExit";
        private const string modName = "NoFacingFireExit";
        private const string modVersion = "1.0.0";
        private readonly Harmony harmony = new Harmony(modGUID);
        internal static NoFacingFireExitBase Instance;


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            harmony.PatchAll(typeof(NoFacingFireExitBase));
            harmony.PatchAll(typeof(EntrancePatch));
        }
    }

    [HarmonyPatch(typeof(EntranceTeleport))]
    internal class EntrancePatch
    {
        [HarmonyPatch("TeleportPlayer")]
        [HarmonyPostfix]
        static void adjustCamera(
            ref StartOfRound ___playersManager,
            ref bool ___isEntranceToBuilding,
            ref int ___entranceId
            )
        {
            PlayerControllerB playerObj = ___playersManager.localPlayerController;
            if (___isEntranceToBuilding == true && playerObj.isInsideFactory && ___entranceId != 0)
            {
                playerObj.thisPlayerBody.Rotate(0f, 180f, 0f, UnityEngine.Space.Self);
            }
        }
    }
}