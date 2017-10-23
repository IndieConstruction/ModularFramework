using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModularFramework.Core;
using ModularFramework.Helpers;
using UniRx;

namespace ModularFramework.Core { 

    /// <summary>
    /// Model.
    /// </summary>
    public class MVC_Test_Model : BaseModel {
        public string TestName;
        /// <summary>
        /// Return true if name validation passed.
        /// </summary>
        public bool ValidName { get { return TestName == "" ? false : true; } }
    }

    /// <summary>
    /// Controller.
    /// </summary>
    public class MVC_Test_Controller : BaseController<MVC_Test_Model> {

        public bool TestNameValidator(MVC_Test_Model _model) {
            if (_model.ValidName)
                Debug.Log("TestName is valid!");
            else
                Debug.Log("TestName is not valid!");
            return _model.ValidName;
        }
    }

    /// <summary>
    /// View.
    /// </summary>
    public class MVC_Test_View : BaseView<MVC_Test_Model, MVC_Test_Controller> {
        protected override void addictionalSetup(ISetupSettings _settings) {
            // do some addictional setup instructions...
        }

        protected override void dataBindings(ISetupSettings _settings) {
            // do some addictional setup instructions... ex:
            Model.ObserveEveryValueChanged(m => m.ValidName).Subscribe(_ => {
                if (controller.TestNameValidator(Model)) {
                    gameObject.name = Model.TestName;
                }
            });
        }
    }

    public class MVC_Test : MonoBehaviour {
        public string ChildViewName;

        private void Start() {
            GenericHelper.InstantiateNewAndSetup<MVC_Test_View>(gameObject, new MVC_Test_View.Settings() {
                model = new MVC_Test_Model() { TestName = ChildViewName },
            });
        }
    }

}
