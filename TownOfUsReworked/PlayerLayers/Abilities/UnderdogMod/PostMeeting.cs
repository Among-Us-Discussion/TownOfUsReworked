﻿using HarmonyLib;
using TownOfUsReworked.Enums;
using TownOfUsReworked.PlayerLayers.Abilities.Abilities;

namespace TownOfUsReworked.PlayerLayers.Abilities.UnderdogMod
{
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    public static class HUDClose
    {
        public static void Postfix()
        {
            var ability = Ability.GetAbility(PlayerControl.LocalPlayer);
            
            if (ability.AbilityType == AbilityEnum.Underdog)
                ((Underdog)ability).SetKillTimer();
        }
    }
}
