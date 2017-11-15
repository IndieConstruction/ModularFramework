using System.Collections.Generic;
using ModularFramework.Core.RewardSystem;

namespace ModularFramework.Core.QuestSystem {

    public interface IQuestObjective {
        /// <summary>
        /// Parent quest auto injected during setup.
        /// </summary>
        IQuest ParentQuest { get; set; }

        /// <summary>
        /// Titolo dell'obbiettivo.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Descrizione dell'obbiettivo.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Collezione di IQuestItems da completare per completare l'obbiettivo.
        /// </summary>
        List<IQuestItem> ObjectiveItems { get; }

        /// <summary>
        /// Identifica se l'obbiettivo è completato. Viene impostata automaticamente.
        /// </summary>
        bool IsComplete { set; get; }

        /// <summary>
        /// Indica se l'obbiettivo è obbligatorio per il completamento della quest o se è facoltativo.
        /// </summary>
        bool IsMandatory { get; } 

        /// <summary>
        /// Lista dei rewards riscattabili una volta completata il quest objective.
        /// </summary>
        List<IRewardBehaviour> Rewards { get; set; }

        /// <summary>
        /// Richiamata automaticamente al termine del setup.
        /// </summary>
        void OnSetupDone();

        void UpdateProgress(); // TODO: Serve?

        void CheckProgress();

        /// <summary>
        /// Evento che viene invocato quando tutti i IQuest Items sono completati e quindi l'obbiettivo è da considerarsi completato.
        /// </summary>
        event IQuestObjectiveEvents.Event OnCompleted;
    }

    public static class IQuestObjectiveExtensions {
        /// <summary>
        /// Setups the specified quest. Auto called from ParentQuest and subscribe it to OnObjectiveCompleted event.
        /// </summary>
        /// <param name="_this">The this.</param>
        /// <param name="_quest">The quest.</param>
        public static void Setup(this IQuestObjective _this, IQuest _quest) {
            _this.ParentQuest = _quest;
            _this.OnCompleted += _quest.OnObjectiveCompleted;
            foreach (IQuestItem questItem in _this.ObjectiveItems) {
                questItem.OnCompleted += _this.OnItemCompleted;
            }
            _this.OnSetupDone();
        }

        /// <summary>
        /// Viene richiamata automaticamente (iscrizione durante il setup) ogni volta che un IQuestItem viene completato.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="_item"></param>
        static void OnItemCompleted(this IQuestObjective _this, IQuestItem _item) {
            _item.OnCompleted -= _this.OnItemCompleted;
            _item.IsCollected = true;
            _this.UpdateProgress();
            _this.CheckProgress();
        }

        // Cercare di rendere overridable...
        //public static void OnSetupDone(this IQuestObjective _this) {
        //}

    }

    public class IQuestObjectiveEvents {
        public delegate void Event(IQuestObjective _objective);
    }

}
