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
        protected override void addictionalSetup(ISetupSettings _settings) {
            // do some addictional setup instructions...
        }

        public bool TestNameValidator() {
            if (Model.ValidName)
                Debug.Log("TestName is valid!");
            else
                Debug.Log("TestName is not valid!");
            return Model.ValidName;
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
            model.ObserveEveryValueChanged(m => m.ValidName).Subscribe(_ => {
                if (controller.TestNameValidator()) {
                    gameObject.name = model.TestName;
                }
            });
        }
    }

    public class MVC_Test : MonoBehaviour {
        public string ChildViewName;

        private void Start() {
            MVC_Test_Controller newController = new MVC_Test_Controller().Setup(new MVC_Test_Controller.Settings { model = new MVC_Test_Model() { TestName = ChildViewName } }) as MVC_Test_Controller;
            GenericHelper.InstantiateNewAndSetup<MVC_Test_View>(gameObject, new MVC_Test_View.Settings() {
                controller = newController,
                model = newController.Model,
            });
        }
    }

}
