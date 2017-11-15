# Reward System [v.1.0](#releases) - [Modular Framework](../../../ReadMe.md)

Il Reward System permette di gestire tutti i reward all'interno del gioco.

Funziona in modo indipendente dagli altri sistemi ma pu� essere agganciato ad ogni elemento di altri sistemi tramite l'utilizzo dell'intefaccia [**IRewardBehaviour**](#irewardbehaviour).

## Come si usa

### Creazione
Per *creare un nuovo tipo di reward* creare una classe che implementa l'intefaccia ***IRewardBehaviour***.

In accoppiamento alla classe, creare una struttura dati che implementa l'intefaccia ***ISetupSettings*** che conterr� tutti i dati necessari all'esecuzione del reward.

### Setup
L'interfaccia prevede una funzione ***void Setup(ISetupSettings _settings)***.

Come parametro verr� fornita una struttura dati che implementa l'intefaccia ***ISetupSettings*** da castare con la struttura creata e , se necessario, salvarla in una variabile per usi futuri.

Durante la fase di setup andr� gestita anche la propriet� ***bool IsUnlocked { get; set; }*** settandola a false in modo da indicare che il reward non � ancora stato riscosso.

### Riscossione

La funzione ***void Redeem()*** permette di "riscuotere" il reward, dovr� quindi contenere tutte le azioni previste dal reward.

Eventuali riferimenti a oggetti, manager, controlleres, etc. interessati dal meccanismo di riscossione potranno essere passati come parametro durante la fase di setup nella struttura custom implementante ***ISetupSettings***.

Durante la fase di riscossione andr� gestita anche la propriet� ***bool IsUnlocked { get; set; }*** settandola a true in modo da indicare che il reward � gi� stato riscosso ed evitare riscossioni multiple (se non consentite).

### Quando � il momento di riscuotere

I comportamenti possono essere differenti a seconda della tipologia di reward che si intende riscuotere, i pi� comuni sono:

- un agente esterno istanzia uno o pi� IRewardBehaviour del tipo desiderato e al momento opportuno chiamer� la funzione ***Redeem()*** dello stesso o degli stessi quando sar� il momento di riscuotere.

- l'IRewardBehaviour si iscriver� ad un evento di un determinato oggetto durante il setup (o diventer� reattivo ad una variabile, stato, etc [*UniRx*]), chiamer� la sua funzione ***Redeem()*** quando allo scatenarsi dell'evento o condizione. In questa occasione bisogner� prevedere la disiscrizione dello stesso evento in modo da non essere pi� richiamato.

## IRewardBehaviour

Chi implementa IRewardBehaviour implementa una delle tipologie di reward riscattabili.

## Releases

### v.1.0
- IRewardBehaviour setuppabili con struttura dati custom (implemtante ISetupSettings).
- IRewardBehaviour riscattabili.

## Next Releases

### v.1.1
- Riscossioni multiple, con numero massimo di riscossioni: 1, 20 o -1 => infinite. 
- Creare componete base di tipo MonoBehaviour implementante IRewardBehaviour.