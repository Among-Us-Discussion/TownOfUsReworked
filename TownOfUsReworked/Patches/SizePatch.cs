﻿using HarmonyLib;
using System.Linq;
using TownOfUsReworked.Extensions;
using UnityEngine;

namespace TownOfUsReworked.Patches
{
    [HarmonyPatch]
    public static class SizePatch
    {
        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        [HarmonyPostfix]
        public static void Postfix(HudManager __instance)
        {
            foreach (var player in PlayerControl.AllPlayerControls.ToArray())
            {
                if (!player.Data.IsDead)
                    player.transform.localScale = player.GetAppearance().SizeFactor;
                else
                    player.transform.localScale = new Vector3(0.7f, 0.7f, 1.0f);

            }

            var playerBindings = PlayerControl.AllPlayerControls.ToArray().ToDictionary(player => player.PlayerId);
            var bodies = UnityEngine.Object.FindObjectsOfType<DeadBody>();

            foreach (var body in bodies)
                body.transform.localScale = playerBindings[body.ParentId].GetAppearance().SizeFactor;
        }
    }
}