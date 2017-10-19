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
            model = _model;
            Title.text = model.Title;
            Body.text = model.Text;
        }

        #endregion

    }

}