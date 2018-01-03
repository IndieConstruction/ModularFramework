using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularFramework.Core {

    public class PopupModel : BaseModel {

        public string Title;
        public string Text;

        public bool Modal;
        public float AutoHideTime;

        /// <summary>
        /// Create new PopupModel.
        /// </summary>
        /// <param name="_title"></param>
        /// <param name="_text"></param>
        /// <param name="_modal"></param>
        /// <param name="_autoHideTime"></param>
        public PopupModel(string _title, string _text, bool _modal, float _autoHideTime) {
            Title = _title;
            Text = _text;
            Modal = _modal;
            AutoHideTime = _autoHideTime;
        }
    }

}