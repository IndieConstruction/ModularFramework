using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ModularFramework.Core {

    public class PopupView : BaseView<PopupModel, PopupController> {

        #region UI Elements

        public TextMeshProUGUI Title;
        public TextMeshProUGUI Body;

        #endregion      

        #region API

        public virtual void UpdateView(PopupModel _model) {
            Model = _model;
            Title.text = Model.Title;
            Body.text = Model.Text;
        }

        #endregion

    }

}