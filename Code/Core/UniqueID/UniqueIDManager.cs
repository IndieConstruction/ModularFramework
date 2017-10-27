using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModularFramework.Core {
    /// <summary>
    /// Permette di erogare id univoci.
    /// </summary>
    public class UniqueIDManager {

        int LastId = 0;
        string prefix = string.Empty;

        /// <summary>
        /// Crea un manager di gestione UniqueID.
        /// </summary>
        /// <param name="_prefix"></param>
        public UniqueIDManager(string _prefix = "") {
            prefix = _prefix;
        }

        /// <summary>
        /// Restituisce un nuovo ID univoco.
        /// </summary>
        /// <returns></returns>
        public string GetNextID() {
            LastId++;
            return string.Format("{0}{1}", prefix, LastId.ToString());
        }

        /// <summary>
        /// Restituisce l'ultimo ID erogato.
        /// </summary>
        /// <returns></returns>
        public string GetLastID() {
            return string.Format("{0}{1}", prefix, LastId.ToString());
        }

    }
}
