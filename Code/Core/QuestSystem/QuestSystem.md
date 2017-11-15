# Quest System [v.1.1](#releases) - [Modular Framework](../../../ReadMe.md) 

Il quest system è formato dagli elementi di tipo [**IQuest**](#iquest), [**IQuestObjective**](#iquestobjective), [**IQuestItem**](#iquestitem), [**IQuestItemUse**](#iquestitemuse).

## IQuest
Rappresenta una quest. 

La **quest** ha una **lista di IQuestObjective**. Al completamento di tutti gli elementi di questa lista, di tipo ***mandatory*** *(IsMandatory = true)*, la quest diventa completata e verrà scatenato *automaticamente* l'evento *IQuestEvents.Event* ***OnCompleted***.

## IQuestObjective
Definisce il singolo compito da portare a termine legato alla quest. Spiega in oltre come interagire con i Quest Items.

### Come funziona

Ci sono X fasi nel ciclo di vita dell'IQuestObjective:

- Setup
- Completamento dei singoli IQuestItems
- Completamento del IQuestObjective

### Setup

Il setup avviene quando viene chiamata, **in automatico dalla IQuest** di cui fa parte l'IQuestObjective, la funzione setup.

Come parametro della funzione di setup verrà passato il riferimento all'IQuest parent e salvato nella proprietà ***ParentQuest***.

Viene registrata l'iscrizione all'evento ***IQuestObjectiveEvents.OnCompleted*** (vedi Completamento IQuestObjective) da parte della ParentQuest ***OnObjectiveCompleted***.

Per ogni IQuestItem della lista della collezione viene eseguito il setup (vedi IQuestItem - Setup). In questa occasione viene si effettua l'iscrizione al suo evento di OnComplete (vedi IQuestItem Complete).

Al termine di tutte queste operazioni viene chiamato la funzione interna OnSetupDone.

### IQuestItem Complete

Al completamento di un IQuestItem della collezione, in virtù dell'iscrizione all'evento di OnComplete dell'item ***verrà richiamata la funzione interna OnItemCompleted***. Durante questa funzione viene effettuata la ***disiscrizione*** dall'evento di ***OnComplete*** dell'Item e vengono effettuati i controllo sulla progressione del IQuestObjective.

### Completamento IQuestObjective

Ha un evento *QuestObjectiveEvents.Event **OnCompleted*** che notifica il completamento dell'obbiettivo. Durante la fase di setup viene iscritto automaticamente a questo evento l'IQuest.

## IQuestItem

### Come funziona
Funziona in modo indipendente dagli altri sistemi. Si può far diventare *quest item* qualsiasi oggetto facendogli implementare l'intefaccia **IQuestItem**.

Richiamare la funzione ***Complete([**IQuestItemUse**](#iquestitemuse) questItemUse)*** e il lavoro dell'oggetto riguardante la quest è da considerarsi *completato* (ricordarsi, eseguendo l'override, di richiamare al termine Base.Complete(...) in modo da permettere di scatenare l'evento *OnCompleted*).

Automaticamente verrà invocata al termine l'evento ***OnCompleted*** quando il lavoro dell'oggetto riguardante la quest è da considerarsi terminato.

La proporietà ***IsCollected*** è da implementare ma non è necessario impostarla, viene automaticamente settata a true una volta chiamato OnCompleted.

## IQuestItemUse

Identifica una azione applicabile ad un IQuestItem. Chi implementa questa intefaccia deve definire l'azione da impostare nel delegate *IQuestItemUseDelegates.UseDelegate* ***UseAction*** che verrà eseguita una volta richiamata la funzione *Complete* dell'IQuestItem.

### Come funziona

L'unica vincolo dell'interfaccia IQuestItemUse è l'implementazione del delegate *IQuestItemUseDelegates.UseDelegate* ***UseAction*** che dovrà contenere le eventuali azioni da eseguire quando il QuestItem verrà completato ***[verificare se è vero che viene impostata da sola... al momento pare di no]***.

Per dichiarare completato un quest item, chiamare la funzione del QuestItem di riferimento (occorre ovviamente creare un riferimento all'oggetto, di norma è sullo stesso gameobject) e richiamare la funzione **Complete** (internamente poi l'oggetto che implementa l'intefeccia IQuestItem deciderà se e come richiamare l'evento OnComplete e dichiararsi comepletato).

## Releases

### v.1.2
- Aggiunta classe base per Quest Objective (MVC + Inheritance).

### v.1.1
- Aggiunto IsManadatory in IQuestObjective.

### v.1.0
- Interfaccia IQuest 
- Interfaccia IQuestObjective
- Interfaccia IQuestItem
- Interfaccia IQuestItemUse

## Next Releases

### v.1.3
- 
