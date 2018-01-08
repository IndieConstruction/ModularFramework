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
using System.Collections.Generic;
using ModularFramework.Core.RewardSystem;

namespace ModularFramework.Core.QuestSystem {

    /// <summary>
    /// Interfaccia per rappresentare una quest.
    /// </summary>
    public interface IQuest {
        // ID
        string ID { get; }
        // Title
        string Title { get; }
        /// <summary>
        /// Se true la quest è completata.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Objectives.
        /// </summary>
        List<IQuestObjective> Objectives { get; }

        /// <summary>
        /// Resources.
        /// </summary>
        List<IQuestResource> Resources { get; }

        /// <summary>
        /// Lista dei rewards riscattabili una volta completata la quest.
        /// </summary>
        List<IRewardBehaviour> Rewards { get; set; }

        /// <summary>
        /// Setups the quest.
        /// </summary>
        /// <param name="_objectives">List of objectives to complete the quest.</param>
        void SetupQuest(List<IQuestObjective> objectives, List<IQuestResource> resources, List<IRewardBehaviour> rewards);

        #region Events (chiamata automatica)

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

        #endregion

    }

    public static class IQuestEvents {
        public delegate void Event(IQuest quest);
    }
}