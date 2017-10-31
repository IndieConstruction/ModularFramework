# UI System [v.1.0](#releases) - [ModularFramework](../../../README.MD)

## Come funziona

### Esempi

```c#

    // ---------------------
    // Popup non modale con auto hide dopo 3 secondi
    BactericaManager.UI.ShowPopup(new PopupModel() {
        Title = "Title Popup",
        Text = "This is 3 seconds popup!!!",
        AutoHideTime = 3,
        Modal = false
    });

    // ---------------------
    // Popup modale
    BactericaManager.UI.ShowPopup(new PopupModel() {
        Title = "Wellcome!",
        Text = "Press space to continue...",
        Modal = true,
    });

    // Chiusura popup modale
    BactericaManager.UI.HidePopup();
    // ---------------------

```

## Releases

### v.1.0
- UI System manager.
- Popup modale.

## Next Releases

### v.1.1
- Callback all'auto chiusura.
- Callback buttons.
