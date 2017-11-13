using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuestItemUse {

    /// <summary>
    /// Azione da compiere all'utilizzo dell'abilità.
    /// </summary>
    IQuestItemUseDelegates.UseDelegate UseAction { get; }

}

public class IQuestItemUseDelegates {
    public delegate void UseDelegate();
}




