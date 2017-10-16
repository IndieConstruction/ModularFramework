# MVC System [v.1.0](#releases) - [ModularFramework](../../../README.MD)

## Come funziona
*Vedere file* [***ModularFramework.Core.MVC_Test***](https://github.com/IndieConstruction/ModularFramework/blob/master/Code/Core/MVC/Test/MVC_Test.cs) . 

### Esempio

#### Creazione nuova view

```c#

/// ---------------------------
/// CREATE CONTROLLER AND MODEL
MVC_Test_Controller newController = 
    new MVC_Test_Controller().Setup(
        new MVC_Test_Controller.Settings { 
            model = new MVC_Test_Model() { TestName = ChildViewName } 
        }
    ) as MVC_Test_Controller;

/// ---------------------------
/// CREATE A VIEW
GenericHelper.InstantiateNewAndSetup<MVC_Test_View>(
        gameObject, 
        new MVC_Test_View.Settings() {
            controller = newController,
            model = newController.Model,
        }
    );
/// ---------------------------

```

#### Settings personalizzati

Nella view o nel controller (in quanto ISetuppable) aggiungere l'extra settings come nell'esempio:

```c#

/// ---------------------------
/// CREATE ExtraSettings class data in ISetuppable class
public class ExtraSettings : Settings {
    public enum StageLoadingMode { full, none }
    public StageLoadingMode LoadingMode = StageLoadingMode.none;
}
/// ---------------------------

/// ---------------------------
/// Use ExtraSettings in addictionalSetup function or later
protected override void addictionalSetup(ISetupSettings _settings) {
    ExtraSettings extraSettings = _settings as ExtraSettings;
    // do something...            
}

```

## Releases

### v.1.0
- Implementazione MVC pattern.

## Next Releases

### v.1.1
