using HarmonyLib;

namespace TownOfUsReworked.PlayerLayers.Roles.CrewRoles.InvestigatorMod
{
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
    public static class StartGame
    {
        public static void Postfix(ShipStatus __instance)
        {
            AddPrints.GameStarted = true;
        }
    }
}