/* --------------------------------------------------------------
*   Indie Contruction : Modular Framework for Unity
*   Copyright(c) 2018 Indie Construction / Paolo Bragonzi
*   All rights reserved. 
*   For any information refer to http://www.indieconstruction.com
*   
*   This library is free software; you can redistribute it and/or
*   modify it under the terms of the GNU Lesser General Public
*   License as published by the Free Software Foundation; either
*   version 3.0 of the License, or(at your option) any later version.
*   
*   This library is distributed in the hope that it will be useful,
*   but WITHOUT ANY WARRANTY; without even the implied warranty of
*   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
*   Lesser General Public License for more details.
*   
*   You should have received a copy of the GNU Lesser General Public
*   License along with this library.
* -------------------------------------------------------------- */

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModularFramework.Core.RewardSystem;


namespace ModularFramework.Core.QuestSystem {
    /// <summary>
    /// Base class for any type of quest:
    /// - Objectives
    /// </summary>
    /// <seealso cref="ModularFramework.QuestSystem.IQuest" />
    public class BaseQuest : IQuest {

        #region properties

        /// <summary>
        /// ID of quest.
        /// </summary>
        public string ID {
            get { return _id; }
            protected set { _id = value; }
        }
        private string _id;

        /// <summary>
        /// Title of Quest.
        /// </summary>
        public string Title {
            get { return _title; }
            set { _title = value; }
        }
        private string _title;

        /// <summary>
        /// True se la quest è già stata completata.
        /// </summary>
        public bool IsCompleted {
            get { return _isCompleted; }
            set { _isCompleted = value; }
        }
        private bool _isCompleted;

        /// <summary>
        /// List of objectives to complete this quest.
        /// </summary>
        public List<IQuestObjective> Objectives {
            get { return _objectives; }
            protected set { _objectives = value; }
        }
        private List<IQuestObjective> _objectives;


        public List<IQuestResource> Resources {
            get { return _resources; }
            set { _resources = value; }
        }
        private List<IQuestResource> _resources;

        /// <summary>
        /// List of rewards for quest completion.
        /// </summary>
        public List<IRewardBehaviour> Rewards {
            get { return _rewards; }
            set { _rewards = value; }
        }
        private List<IRewardBehaviour> _rewards;

        #endregion  

        public event IQuestEvents.Event OnCompleted;
        public event IQuestEvents.Event OnFail;

        /// <summary>
        /// Imposta i parametri della quest ed esegue il setup per tutti gli elementi che lo richiedono.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="objectives"></param>
        /// <param name="rewards"></param>
        public BaseQuest(string id, string title, List<IQuestObjective> objectives, List<IQuestResource> resources, List<IRewardBehaviour> rewards) {
            ID = id;
            Title = title;
            IsCompleted = false;
            SetupQuest(objectives, resources, rewards);
        }

        /// <summary>
        /// Setups the quest.
        /// </summary>
        /// <param name="objectives">The objectives.</param>
        public void SetupQuest(List<IQuestObjective> objectives, List<IQuestResource> resources, List<IRewardBehaviour> rewards) {
            // objectives
            Objectives = objectives;
            foreach (var objective in Objectives) {
                objective.Setup(this);
            }
            // resources
            Resources = resources;
            foreach (var resource in Resources) {
                
            }
            // rewards
            Rewards = rewards;
            foreach (var reward in Rewards) {
                
            }
            AddictionalSetupQuest();
        }

        /// <summary>
        /// Eseguire override di questo metodo se si vogliono eseguire ulteriori operazioni di setup.
        /// </summary>
        protected virtual void AddictionalSetupQuest() { }

        /// <summary>
        /// Richiamata automaticamente ogni volta che si completa un obbiettivo della quest.
        /// - disiscrizione automatica all'evento di completamento dell'obbiettivo appena completato.
        /// - controlla se sono ancora presenti degli obbiettivi non completati e IsMandatory == true, se non ce ne sono 
        /// Called when a objective is completed. This behaviour only unsubscribe from event subscription (then call this after the override logic).
        /// </summary>
        /// <param name="_objective">The objective.</param>
        public virtual void OnObjectiveCompleted(IQuestObjective objective) {
            objective.OnCompleted -= OnObjectiveCompleted;
            Debug.LogFormat("Objective {0} completed! ", objective.Title);
            if (Objectives.FindAll(o => o.IsComplete == false || o.IsMandatory == true).Count == 0) {
                internalCompleteQuest();
            }
        }

        /// <summary>
        /// Esegue le funzioni che vanno sempre eseguite al completamento della quest.
        /// </summary>
        protected void internalCompleteQuest() {
            CompleteQuest();
            if (OnCompleted != null)
                OnCompleted(this);
            IsCompleted = true;
        }

        /// <summary>
        /// Contiene la logica con cui vengono eseguite le funzioni accessorie al completamento della quest.
        /// Eseguire l'override per implementare le proprie logiche. Se non verrà eseguito alcun override il comportamento base sarà quello di eseguire il redeem di tutti i rewards.
        /// </summary>
        protected virtual void CompleteQuest() {
            // Quest completed
            RedeemAllRewards();

        }

        /// <summary>
        /// Riscatta tutti i rewards assegnati alla quest in modo atomico.
        /// Eseguire l'override per implementare un sistema di redeem alternativo.
        /// </summary>
        protected virtual void RedeemAllRewards() {
            foreach (IRewardBehaviour reward in Rewards) {
                reward.Redeem();
            }
        }

        /// <summary>
        /// Esegue le funzioni che vanno sempre eseguite al fallimento della quest.
        /// </summary>
        protected void internalFailQuest() {
            FailQuest();
            if (OnFail != null)
                OnFail(this);
        }

        /// <summary>
        /// Contiene la logica con cui vengono eseguite le funzioni accessorie al fallimento della quest.
        /// Eseguire l'override per implementare le proprie logiche.
        /// </summary>
        protected virtual void FailQuest() {

        }

    }
}
