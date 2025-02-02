using Hazel;
using System;
using System.Linq;
using TownOfUsReworked.Enums;
using TownOfUsReworked.Lobby.CustomOption;
using TownOfUsReworked.Extensions;
using TownOfUsReworked.Patches;
using Il2CppSystem.Collections.Generic;

namespace TownOfUsReworked.PlayerLayers.Roles.Roles
{
    public class Murderer : Role
    {
        public PlayerControl ClosestPlayer;
        public DateTime LastKill { get; set; }
        public bool MurdWins { get; set; }

        public Murderer(PlayerControl player) : base(player)
        {
            Name = "Murderer";
            ImpostorText = () => "Imagine Getting Boring Murderer";
            TaskText = () => "Kill everyone!\nFake Tasks:";
            Color = CustomGameOptions.CustomNeutColors ? Colors.Murderer : Colors.Neutral;
            SubFaction = SubFaction.None;
            LastKill = DateTime.UtcNow;
            RoleType = RoleEnum.Murderer;
            Faction = Faction.Neutral;
            FactionName = "Neutral";
            FactionColor = Colors.Neutral;
            RoleAlignment = RoleAlignment.NeutralKill;
            AlignmentName = () => "Neutral (Killing)";
            IntroText = "Murder those who oppose you";
            Results = InspResults.CrewImpAnMurd;
            AddToRoleHistory(RoleType);
        }

        internal override bool EABBNOODFGL(ShipStatus __instance)
        {
            if (Player.Data.IsDead | Player.Data.Disconnected)
                return true;

            if (PlayerControl.AllPlayerControls.ToArray().Count(x => !x.Data.IsDead && !x.Data.Disconnected &&
                (x.Data.IsImpostor() | (x.Is(RoleAlignment.NeutralKill) | !x.Is(RoleEnum.Murderer)) | x.Is(Faction.Crew))) == 0)
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.MurdererWin,
                    SendOption.Reliable, -1);
                writer.Write(Player.PlayerId);
                Wins();
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                Utils.EndGame();
                return false;
            }

            return false;
        }

        public override void Wins()
        {
            MurdWins = true;
        }

        public override void Loses()
        {
            LostByRPC = true;
        }

        public float KillTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastKill;
            var num = CustomGameOptions.MurdKCD * 1000f;
            var flag2 = num - (float)timeSpan.TotalMilliseconds < 0f;

            if (flag2)
                return 0;

            return (num - (float)timeSpan.TotalMilliseconds) / 1000f;
        }

        protected override void IntroPrefix(IntroCutscene._ShowTeam_d__21 __instance)
        {
            var murdTeam = new List<PlayerControl>();
            murdTeam.Add(PlayerControl.LocalPlayer);
            __instance.teamToShow = murdTeam;
        }
    }
}