using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularFramework.Data {

    public interface IDataContainer {
        /// <summary>
        /// Prepare and return data for saving.
        /// </summary>
        /// <returns></returns>
        IData GetDataForSave();
    }

    public interface IData {
        
    }

}
