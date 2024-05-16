using ninja.Model;
using ninja.Model.BotFields.States;
using System.Collections.Generic;

namespace ninja.Model.BotFields
{
    public class BotStateMachine
    {
        public BotState CurrentState;

        private BotStatePatrolling botStatePatrolling;
        private BotStateSuspicion botStateSuspicion;
        private BotStateСhase botStateСhase;

        public Dictionary<string, BotState> BotStates = new();

        public BotStateMachine(Bot bot)
        {
            botStatePatrolling = new BotStatePatrolling(bot);
            botStateSuspicion = new BotStateSuspicion(bot);
            botStateСhase = new BotStateСhase(bot);

            BotStates.Add("Patrolling", botStatePatrolling);
            BotStates.Add("Suspicion", botStateSuspicion);
            BotStates.Add("Сhase", botStateСhase);
        }


        public void ChengeState()
        {



        }
    }
}
