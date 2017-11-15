using ModularFramework.Core.RewardSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModularFramework.Core.QuestSystem {
    [Serializable]
    public abstract class QuestObjectiveModel : BaseModel {

        #region serializable variables

        /// <summary>
        /// Titolo dell'obbiettivo.
        /// </summary>
        public string Title;

        /// <summary>
        /// Descrizione dell'obbiettivo.
        /// </summary>
        public string Description;

        /// <summary>
        /// Collezione di IQuest items che compongono questo obbiettivo.
        /// </summary>
        public List<IQuestItem> ObjectiveItems;

        /// <summary>
        /// Descrive se questo obbiettivo è obbligatorio per il completamento della quest.
        /// </summary>
        public bool IsMandatory;

        /// <summary>
        /// True se è completato.
        /// </summary>
        public bool IsComplete { get; set; }

        #endregion

        #region non serializable properties

        // QuestObjectiveModel non serializable properties here...

        #endregion

    }

}