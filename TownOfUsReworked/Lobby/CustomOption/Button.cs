using System;

namespace TownOfUsReworked.Lobby.CustomOption
{
    public class CustomButtonOption : CustomOption
    {
        protected internal Action Do;

        protected internal CustomButtonOption(int id, MultiMenu menu, string name, Action toDo = null) : base(id, menu, name, CustomOptionType.Button,
            0)
        {
            Do = toDo ?? BaseToDo;
        }

        protected internal CustomButtonOption(bool indent, int id, MultiMenu menu, string name, Action toDo = null) : this(id, menu, name, toDo)
        {
            Indent = indent;
        }

        public static void BaseToDo() {}

        public override void OptionCreated()
        {
            base.OptionCreated();
            Setting.Cast<ToggleOption>().TitleText.text = Name;
        }
    }
}