namespace Engikitty.Types
{
    /// <summary>
    /// Simple class that holds info about a command
    /// </summary>
    public class CommandInfo
    {
        public readonly bool IsEphemeral;
        public readonly bool IsHeavy;
        public readonly int Cooldown;
        public readonly int CooldownOnThisCommand;

        /// <summary>
        /// Initializes the CommandInfo class
        /// </summary>
        /// <param name="IsEphemeral">Whether the command is hidden or not</param>
        /// <param name="IsHeavy">Whether the command is heavy (takes time) or not</param>
        /// <param name="Cooldown">The global cooldown to apply when an user uses this command</param>
        /// <param name="CooldownOnThisCommand">The local (specific to this command) cooldown to apply when an user uses this command</param>
        public CommandInfo(bool IsEphemeral = false, bool IsHeavy = false, int Cooldown = 2, int CooldownOnThisCommand = 2)
        {
            this.IsEphemeral = IsEphemeral;
            this.IsHeavy = IsHeavy;
            this.Cooldown = Cooldown;
            this.CooldownOnThisCommand = CooldownOnThisCommand;
        }
    }
}