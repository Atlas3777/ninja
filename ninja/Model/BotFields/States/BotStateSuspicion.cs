namespace ninja.Model.BotFields.States
{
    public class BotStateSuspicion : BotState
    {
        public Bot bot;
        public BotStateSuspicion(Bot bot)
        {
            this.bot = bot;
        }
        public override void Execute()
        {

        }
    }
}
