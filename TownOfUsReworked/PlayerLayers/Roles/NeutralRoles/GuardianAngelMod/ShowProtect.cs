using HarmonyLib;
using TownOfUsReworked.Enums;
using TownOfUsReworked.Lobby.CustomOption;
using TownOfUsReworked.Extensions;
using UnityEngine;
using TownOfUsReworked.PlayerLayers.Roles.Roles;
using TownOfUsReworked.PlayerLayers.Roles.CrewRoles.MedicMod;

namespace TownOfUsReworked.PlayerLayers.Roles.NeutralRoles.GuardianAngelMod
{
    public enum ProtectOptions
    {
        Self,
        GA,
        SelfAndGA,
        Everyone
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class ShowProtect
    {
        public static Color ProtectedColor = new Color(1f, 0.85f, 0f, 1f);
        public static Color ShieldedColor = Color.cyan;

        public static void Postfix(HudManager __instance)
        {
            foreach (var role in Role.GetRoles(RoleEnum.GuardianAngel))
            {
                var ga = (GuardianAngel) role;

                var player = ga.TargetPlayer;

                if (player == null)
                    continue;

                if (ga.Protecting)
                {
                    var showProtected = CustomGameOptions.ShowProtect;

                    if (showProtected == ProtectOptions.Everyone)
                    {
                        player.myRend().material.SetColor("_VisorColor", ProtectedColor);
                        player.myRend().material.SetFloat("_Outline", 1f);
                        player.myRend().material.SetColor("_OutlineColor", ProtectedColor);
                    }
                    else if (PlayerControl.LocalPlayer.PlayerId == player.PlayerId && (showProtected == ProtectOptions.Self |
                        showProtected == ProtectOptions.SelfAndGA))
                    {
                        player.myRend().material.SetColor("_VisorColor", ProtectedColor);
                        player.myRend().material.SetFloat("_Outline", 1f);
                        player.myRend().material.SetColor("_OutlineColor", ProtectedColor);
                    }
                    else if (PlayerControl.LocalPlayer.Is(RoleEnum.GuardianAngel) && (showProtected == ProtectOptions.GA |
                        showProtected == ProtectOptions.SelfAndGA))
                    {
                        player.myRend().material.SetColor("_VisorColor", ProtectedColor);
                        player.myRend().material.SetFloat("_Outline", 1f);
                        player.myRend().material.SetColor("_OutlineColor", ProtectedColor);
                    }
                }
                else if (ga.TargetPlayer.IsShielded())
                {

                    var showShielded = CustomGameOptions.ShowShielded;
                    if (showShielded == ShieldOptions.Everyone)
                    {
                        player.myRend().material.SetColor("_VisorColor", ShieldedColor);
                        player.myRend().material.SetFloat("_Outline", 1f);
                        player.myRend().material.SetColor("_OutlineColor", ShieldedColor);
                    }
                    else if (PlayerControl.LocalPlayer.PlayerId == player.PlayerId && (showShielded == ShieldOptions.Self |
                        showShielded == ShieldOptions.SelfAndMedic))
                    {
                        player.myRend().material.SetColor("_VisorColor", ShieldedColor);
                        player.myRend().material.SetFloat("_Outline", 1f);
                        player.myRend().material.SetColor("_OutlineColor", ShieldedColor);
                    }
                    else if (PlayerControl.LocalPlayer.Is(RoleEnum.Medic) && (showShielded == ShieldOptions.Medic |
                        showShielded == ShieldOptions.SelfAndMedic))
                    {
                        player.myRend().material.SetColor("_VisorColor", ShieldedColor);
                        player.myRend().material.SetFloat("_Outline", 1f);
                        player.myRend().material.SetColor("_OutlineColor", ShieldedColor);
                    }
                    else
                    {
                        player.myRend().material.SetColor("_VisorColor", Palette.VisorColor);
                        player.myRend().material.SetFloat("_Outline", 0f);
                    }
                }
                else
                {
                    player.myRend().material.SetColor("_VisorColor", Palette.VisorColor);
                    player.myRend().material.SetFloat("_Outline", 0f);
                }
            }
        }
    }
}