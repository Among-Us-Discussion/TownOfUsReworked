using TownOfUsReworked.Patches;
using TownOfUsReworked.Lobby.CustomOption;
using TownOfUsReworked.Enums;

namespace TownOfUsReworked.PlayerLayers.Modifiers.Modifiers
{
    public class Diseased : Modifier
    {
        public Diseased(PlayerControl player) : base(player)
        {
            Name = "Diseased";
            TaskText = () => "Your killers get a higher cooldown";
            Color = CustomGameOptions.CustomModifierColors ? Colors.Diseased : Colors.Modifier;
            ModifierType = ModifierEnum.Diseased;
            AddToModifierHistory(ModifierType);
        }
    }
}