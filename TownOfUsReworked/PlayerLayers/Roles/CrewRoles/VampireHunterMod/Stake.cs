﻿using System;
using HarmonyLib;
using Hazel;
using TownOfUsReworked.PlayerLayers.Roles.CrewRoles.MedicMod;
using TownOfUsReworked.Extensions;
using TownOfUsReworked.Enums;
using TownOfUsReworked.Patches;
using TownOfUsReworked.Lobby.CustomOption;
using TownOfUsReworked.PlayerLayers.Roles.Roles;

namespace TownOfUsReworked.PlayerLayers.Roles.CrewRoles.VampireHunterMod
{
    [HarmonyPatch(typeof(KillButton), nameof(KillButton.DoClick))]
    public static class Stake
    {
        [HarmonyPriority(Priority.First)]
        private static bool Prefix(KillButton __instance)
        {
            if (__instance != DestroyableSingleton<HudManager>.Instance.KillButton)
                return true;

            var flag = PlayerControl.LocalPlayer.Is(RoleEnum.Vigilante);

            if (!flag)
                return true;

            var role = Role.GetRole<VampireHunter>(PlayerControl.LocalPlayer);

            if (!PlayerControl.LocalPlayer.CanMove)
                return false;

            if (PlayerControl.LocalPlayer.Data.IsDead)
                return false;

            var flag2 = role.StakeTimer() == 0f;

            if (!flag2)
                return false;

            if (!__instance.enabled | role.ClosestPlayer == null)
                return false;

            var distBetweenPlayers = Utils.GetDistBetweenPlayers(PlayerControl.LocalPlayer, role.ClosestPlayer);
            var flag3 = distBetweenPlayers < GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];

            if (!flag3)
                return false;

            var flag4 = (role.ClosestPlayer.Is(SubFaction.Vampires));

            if (role.ClosestPlayer.Is(RoleEnum.Pestilence))
            {
                Utils.RpcMurderPlayer(role.ClosestPlayer, PlayerControl.LocalPlayer);
                return false;
            }
            
            if (role.ClosestPlayer.IsInfected() | role.Player.IsInfected())
            {
                foreach (var pb in Role.GetRoles(RoleEnum.Plaguebearer))
                    ((Plaguebearer)pb).RpcSpreadInfection(role.ClosestPlayer, role.Player);
            }

            if (role.ClosestPlayer.IsOnAlert())
            {
                if (role.ClosestPlayer.IsShielded())
                {
                    var medic = role.ClosestPlayer.GetMedic().Player.PlayerId;
                    var writer1 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.AttemptSound, SendOption.Reliable, -1);
                    writer1.Write(medic);
                    writer1.Write(role.ClosestPlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer1);

                    if (CustomGameOptions.ShieldBreaks)
                        role.LastStaked = DateTime.UtcNow;

                    StopKill.BreakShield(medic, role.ClosestPlayer.PlayerId, CustomGameOptions.ShieldBreaks);
                    Utils.RpcMurderPlayer(role.ClosestPlayer, PlayerControl.LocalPlayer);
                }
                else if (role.Player.IsShielded())
                {
                    var medic = role.Player.GetMedic().Player.PlayerId;
                    var writer1 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.AttemptSound, SendOption.Reliable, -1);
                    writer1.Write(medic);
                    writer1.Write(role.Player.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer1);

                    if (CustomGameOptions.ShieldBreaks)
                        role.LastStaked = DateTime.UtcNow;

                    StopKill.BreakShield(medic, role.Player.PlayerId, CustomGameOptions.ShieldBreaks);
                    Utils.RpcMurderPlayer(PlayerControl.LocalPlayer, PlayerControl.LocalPlayer);
                }
                else
                    Utils.RpcMurderPlayer(role.ClosestPlayer, PlayerControl.LocalPlayer);

                return false;
            }
            else if (role.ClosestPlayer.IsShielded())
            {
                var medic = role.ClosestPlayer.GetMedic().Player.PlayerId;
                var writer1 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.AttemptSound, SendOption.Reliable, -1);
                writer1.Write(medic);
                writer1.Write(role.ClosestPlayer.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer1);

                if (CustomGameOptions.ShieldBreaks)
                    role.LastStaked = DateTime.UtcNow;

                StopKill.BreakShield(medic, role.ClosestPlayer.PlayerId, CustomGameOptions.ShieldBreaks);

                return false;
            }
            else if (role.ClosestPlayer.IsVesting())
            {
                role.LastStaked.AddSeconds(CustomGameOptions.VestKCReset);

                return false;
            }
            else if (role.ClosestPlayer.IsProtected())
            {
                role.LastStaked.AddSeconds(CustomGameOptions.ProtectKCReset);

                return false;
            }

            if (flag4)
            {
                Utils.RpcMurderPlayer(PlayerControl.LocalPlayer, role.ClosestPlayer);
                role.LastStaked = DateTime.UtcNow;
            }

            return false;
        }
    }
}
