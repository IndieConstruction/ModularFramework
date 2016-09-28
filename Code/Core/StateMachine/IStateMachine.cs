using System;
using System.Collections.Generic;
using UnityEngine;

namespace ModularFramework.Core.SM {

    public interface IStateMachine {
        List<IState> States { get; set; }
        void Change(Type _type);
        void Change<T>() where T : Type;
        void Init(MonoBehaviour _view);
    }
}