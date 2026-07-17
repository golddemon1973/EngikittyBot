namespace Engikitty.Types
{
    /// <summary>
    /// Simple class that holds info about a command
    /// </summary>
    public class CommandInfo
    {
        public readonly bool IsEphemeral;
        public readonly bool IsHeavy;

        /// <summary>
        /// Initializes the CommandInfo class
        /// </summary>
        /// <param name="IsEphemeral">Whether the command is hidden or not</param>
        /// <param name="IsHeavy">Whether the command is heavy (takes time) or not</param>
        public CommandInfo(bool IsEphemeral = false, bool IsHeavy = false)
        {
            this.IsEphemeral = IsEphemeral;
            this.IsHeavy = IsHeavy;
        }
    }
}