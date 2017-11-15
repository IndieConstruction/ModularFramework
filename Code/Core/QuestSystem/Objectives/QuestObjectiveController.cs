using System;

namespace ModularFramework.Core.QuestSystem {
    public abstract class QuestObjectiveController<T> : BaseController<QuestObjectiveModel> {

        #region Controller Singleton

        private static bool instanced = false;

        new public static T Instance {
            get {
                if (instanced == false) {
                    _instance = Activator.CreateInstance<T>();
                    instanced = true;
                }
                return _instance;
            }
            private set { _instance = value; }
        }
        private static T _instance = default(T);

        #endregion

    }

}