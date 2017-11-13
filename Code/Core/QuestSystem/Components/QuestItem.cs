using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularFramework.QuestSystem {

    public class QuestItem : MonoBehaviour, IQuestItem {

        public bool IsCollected { get; set; }

        public event IQuestItemEvents.Event OnCompleted;

        public virtual void Complete(IQuestItemUse questItemUse) {
            questItemUse.UseAction();
            IsCollected = true;
            if (OnCompleted != null)
                OnCompleted(this);
        }
    }

}