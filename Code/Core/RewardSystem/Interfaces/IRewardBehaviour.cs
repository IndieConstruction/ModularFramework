using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModularFramework.Core;
using System;

namespace ModularFramework.Core.RewardSystem {

    /// <summary>
    /// Conferisce la capacità di eseguire un reward.
    /// </summary>
    public interface IRewardBehaviour : ISetuppable {

        /// <summary>
        /// Se true il reward è già stato riscosso.
        /// </summary>
        bool IsUnlocked { get; set; }
        
        /// <summary>
        /// Contiene le azioni da compiere quando si "riscatta" il reward.
        /// </summary>
        void Redeem();

    }

    /// <summary>
    /// Contiene tutti i settings necessari alla greazione di un reward.
    /// </summary>
    public interface IRewardSettings : ISetupSettings { }

}