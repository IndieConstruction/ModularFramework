# Modular Framework [v.0.7](#releases)
Modular Framework for Unity3d

## Features
- Singleton GameManager
- Core modules
  - Scene Module
  - UI Module
  - Data Module
  - Localization Module
  - Player Profile Module
  - Gameplay Module
  - [Quest System](Code/Core/QuestSystem/QuestSystem.md)
  - [Reward System](Code/Core/RewardSystem/RewardSystem.md)
- Module installer
- [UI System](Code/Core/UI/UISystem.md)
- [MVC System](Code/Core/MVC/MVC_System.md) 
- [State Machine](Code/Core/BehaviourMachine/FSM.md)
- MF Helpers
  - [Weighted List](#WeightedList)

## WeightedList
Lista persata che contiene oggetti generici, e un peso espresso come intero.

```c#
    
    // creazione della lista
    WeightedList<string> elementTypesWeightedList = new WeightedList<string>(
            new List<WeightedElement<string>>() {
                new WeightedElement<string>("WallBlock", 1),
                new WeightedElement<string>("ColoredBlock", 5),
            }
        );

    // restituzione di un elemento random tenendo conto dei pesi
    string e = elementTypesWeightedList.GetRandomElement();

```

## How to add ModularFramework as Submodule in your unity project.

## Releases

### v.0.7
- Implementazione MVC pattern.

## Next Releases

### v.0.8