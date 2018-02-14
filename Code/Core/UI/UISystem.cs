using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using ModularFramework.Helpers;
using UniRx;

namespace ModularFramework.Core.UISystem {
    /// <summary>
    /// TODO: far ereditare da un'interfaccia che unifichi tutti i moduli core o i system.
    /// </summary>
    public class UISystem : MonoBehaviour {

        public GameObject UIRootComponent;
        public PopupView PopupViewPrefab;
        /// <summary>
        /// Maskera che oscura lo sfondo.
        /// </summary>
        public Image BlackMask;
        float blackMaskFade = 0.7f;
        float animDuration = 0.5f;

        RectTransform WindowsContainer;

        /// <summary>
        /// Esegue setup degli elementi comuni della UI:
        /// - Popup
        /// - Popup modal
        /// Crea la finestra per popup modal e non.
        /// </summary>
        public void Setup() {
            GameObject newGO = new GameObject("WindowsContainer", typeof(RectTransform), typeof(Canvas));
            WindowsContainer = newGO.GetComponent<RectTransform>();
            WindowsContainer.SetParent(UIRootComponent.transform, false);
            WindowsContainer.anchorMin = Vector2.zero;
            WindowsContainer.anchorMax = Vector2.one;
            WindowsContainer.sizeDelta = Vector2.one;
            WindowsContainer.GetComponent<Canvas>().overrideSorting = true;
            WindowsContainer.GetComponent<Canvas>().sortingOrder = 10;

            newGO = new GameObject("BlackMask", typeof(RectTransform), typeof(Image));
            newGO.transform.SetParent(WindowsContainer.transform, false);
            BlackMask = newGO.GetComponent<Image>();
            BlackMask.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            BlackMask.GetComponent<RectTransform>().anchorMax = Vector2.one;
            BlackMask.GetComponent<RectTransform>().sizeDelta = Vector2.one;
            BlackMask.color = GenericHelper.ColorFromRGB(28,28,28,0);

            genericPopup = Instantiate<PopupView>(PopupViewPrefab, WindowsContainer, false);

        }

        #region popup

        PopupView genericPopup;
        Sequence seq;

        /// <summary>
        /// Mostra il popup.
        /// </summary>
        /// <param name="popupModel"></param>
        public void ShowPopup(PopupModel popupModel, bool withCallback = true) {
            if (seq != null) seq.Kill();
            seq = DOTween.Sequence();
            genericPopup.UpdateView(popupModel);
            seq.Append(genericPopup.GetComponent<RectTransform>().DOAnchorPosY(0, animDuration).SetEase(Ease.OutExpo));
            if (popupModel.Modal)
                seq.Insert(0, BlackMask.DOFade(blackMaskFade, animDuration));
            if (popupModel.AutoHideTime > 0) {
                seq.Append(transform.DOMove(transform.position, popupModel.AutoHideTime));
                if (withCallback)
                    seq.OnComplete(() => { onPopupClosed(); });
            } else {
                if(withCallback)
                    seq.OnComplete(() => { onPopupOpened(); });
            }
        }

        /// <summary>
        /// Nasconde il popup.
        /// </summary>
        /// <param name="closeCallback"></param>
        public void HidePopup(bool withCallback = true) {
            float animDuration = 1f;
            if (seq != null) seq.Kill();
            seq = DOTween.Sequence();
            seq.Append(genericPopup.GetComponent<RectTransform>().DOAnchorPosY(-Screen.height, animDuration).SetEase(Ease.OutExpo));
            seq.Insert(0, BlackMask.DOFade(0, animDuration));
            seq.OnComplete(() => { onPopupClosed(); });
        }

        /// <summary>
        /// #33 | Ripensare OnPopupClosed callback event
        /// </summary>
        void onPopupClosed() {
            Bolt.CustomEvent.Trigger(gameObject, "OnPopupClosed");
        }

        void onPopupOpened() {
            Bolt.CustomEvent.Trigger(gameObject, "OnPopupOpened");
        }

        #endregion
    }

}