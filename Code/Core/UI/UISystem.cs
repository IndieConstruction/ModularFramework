using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ModularFramework.Core.UISystem {
    /// <summary>
    /// TODO: far ereditare da un'interfaccia che unifichi tutti i moduli core o i system.
    /// </summary>
    public class UISystem : MonoBehaviour {

        public GameObject RootComponent;
        public PopupView PopupViewPrefab;

        Transform WindowsContainer;

        /// <summary>
        /// TODO: sostituire con setup iSetuppable.
        /// </summary>
        public void Setup() {
            WindowsContainer = new GameObject("WindowsContainer", typeof(RectTransform), typeof(Canvas)).transform;
            WindowsContainer.SetParent(RootComponent.transform, false);
            WindowsContainer.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            WindowsContainer.GetComponent<RectTransform>().anchorMax = Vector2.one;
            WindowsContainer.GetComponent<RectTransform>().sizeDelta = Vector2.one;
            WindowsContainer.GetComponent<Canvas>().overrideSorting = true;
            WindowsContainer.GetComponent<Canvas>().sortingOrder = 10;

            genericPopup = Instantiate<PopupView>(PopupViewPrefab, WindowsContainer, false);
            ShowPopup(new PopupModel());
        }

        #region popup

        PopupView genericPopup;
        Sequence seq;

        public void ShowPopup(PopupModel popupModel) {
            seq = DOTween.Sequence();
            seq.Append(genericPopup.GetComponent<RectTransform>().DOAnchorPosY(-Screen.height, 1).SetEase(Ease.OutExpo));
        }

        #endregion
    }

}