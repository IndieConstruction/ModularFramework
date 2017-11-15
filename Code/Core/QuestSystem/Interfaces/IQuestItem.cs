using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularFramework.Core.QuestSystem {

    /// <summary>
    /// Interface for any quest item.
    /// </summary>
    public interface IQuestItem {

        /// <summary>
        /// Non è necessario settare questa proprietà, *verrà settata in automatico una volta completato*.
        /// </summary>
        bool IsCollected { get; set; }
        
        /// <summary>
        /// Funzione che definisce le operazioni necessarie per "completare" l'elemento.
        /// </summary>
        /// <param name="questItemUse"></param>
        void Complete(IQuestItemUse questItemUse);

        /// <summary>
        /// Evento da richiamare per notificare che è terminata la funzione dell'item.
        /// L'interfaccia obbliga la dichiarazione dell'evento, nella classe implementante devono essere specificate le modalità con cui avviene questo evento.
        /// </summary>
        event IQuestItemEvents.Event OnCompleted;

    }

    public static class IQuestItemExtensions {
        
    }

    public static class IQuestItemEvents {
        /// <summary>
        /// Dichiarazione della tipologia di delegato.
        /// </summary>
        /// <param name="_item">The item.</param>
        public delegate void Event(IQuestItem _item);
    }
}