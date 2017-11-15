using System.Collections.Generic;
using ModularFramework.Core.RewardSystem;

namespace ModularFramework.Core.QuestSystem {

    public interface IQuest {
        // ID
        string ID { get; }
        // Title
        string Title { get; }
        // Objectives
        List<IQuestObjective> Objectives { get; }

        /// <summary>
        /// Lista dei rewards riscattabili una volta completata la quest.
        /// </summary>
        List<IRewardBehaviour> Rewards { get; set; }

        /// <summary>
        /// Setups the quest.
        /// </summary>
        /// <param name="_objectives">List of objectives to complete the quest.</param>
        void SetupQuest(List<IQuestObjective> _objectives);

        /// <summary>
        /// Called when a single objective is completed.
        /// </summary>
        /// <param name="objective">The objective.</param>
        void OnObjectiveCompleted(IQuestObjective objective);

        /// <summary>
        /// Called when all quest objectives completed.
        /// </summary>
        /// <param name="quest"></param>
        event IQuestEvents.Event OnCompleted;

    }

    public static class IQuestEvents {
        public delegate void Event(IQuest quest);
    }
}