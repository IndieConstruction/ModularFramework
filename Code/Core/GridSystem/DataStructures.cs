using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace ModularFramework.GridSystem {

    #region Grid Strutture e Classi

    #region Grid

    /// <summary>
    /// Rappresenta le coordinate di una posizione sulla grid.
    /// </summary>
    public struct Position2D {
        public int x;
        public int y;

        public Position2D(int _x, int _y) {
            x = _x;
            y = _y;
        }

        #region static constructions
        public static Position2D New(int _x, int _y) {
            return new Position2D(_x, _y);
        }
        /// <summary>
        /// Punto zero.
        /// </summary>
        public static Position2D Zero { get { return new Position2D(0, 0); } }
        /// <summary>
        /// Placeholder per gli elementi che non sono nella griglia di gioco e devono essere piazzati dal player.
        /// </summary>
        public static Position2D ToBePlacedByPlayer { get { return new Position2D(0, -10); } }
        /// <summary>
        /// Placeholder per gli elementi che non sono nella griglia di gioco e sono in attesa di essere piazzati automaticamente.
        /// </summary>
        public static Position2D OutOfTheGrid { get { return new Position2D(-10, -10); } }
        /// <summary>
        /// Placeholder che indica una posizione non valida.
        /// </summary>
        public static Position2D InvalidPositionGrid { get { return new Position2D(-1000, -1000); } }
        #endregion

        public override string ToString() {
            return string.Format("[{0},{1}]", x.ToString(), y.ToString());
        }

        public static bool operator ==(Position2D c1, Position2D c2) {
            return c1.Equals(c2);
        }

        public static bool operator !=(Position2D c1, Position2D c2) {
            return !c1.Equals(c2);
        }

    }

    public struct Dimension2D {
        public int x;
        public int y;
        public Dimension2D(int _x, int _y) {
            x = _x;
            y = _y;
        }

        public static Dimension2D Zero { get { return new Dimension2D(0, 0); } }

        public static bool operator ==(Dimension2D c1, Dimension2D c2) {
            return c1.Equals(c2);
        }

        public static bool operator !=(Dimension2D c1, Dimension2D c2) {
            return !c1.Equals(c2);
        }
    }

    public struct GridElement {

    }

    #endregion

    #region Actions

    public interface iGridAction {
        ActionType GetActionType { get; }
        void DoAction(IGridView _grid);
    }

    public enum ActionType {
        /// <summary>
        /// Action fittizia che serve ad indicare che è terminata un blocco di actions.
        /// Può essere utilizzata dallo scheduler per mettersi in attesa di un input del player, altro o ignorata.
        /// </summary>
        stop = 0,
        action = 1,
    }
    
    /// <summary>
    /// Struttura che contiene una lista di Action eseguibili.
    /// </summary>
    public struct ActionsSet {

        #region passive trigger

        /// <summary>
        /// IsCompleted == true se Actions è vuoto.
        /// Usato come indicatore da trasformare in reactive property.
        /// </summary>
        public bool IsCompleted {
            get {
                if (Actions != null && Actions.Count > 0)
                    return false;
                return true;
            }
        }

        #endregion

        /// <summary>
        /// Actions.
        /// </summary>
        public List<iGridAction> Actions;

        List<iGridAction> ExecutedAction;

        IGridView grid;

        /// <summary>
        /// Crea un ActionsSet.
        /// </summary>
        /// <param name="_grid"></param>
        public ActionsSet(IGridView _grid) {
            Actions = new List<iGridAction>() { };
            ExecutedAction = new List<iGridAction>();
            grid = _grid;
            scheduler = null;
        }

        /// <summary>
        /// Crea un ActionsSet.
        /// </summary>
        /// <param name="_action"></param>
        /// <param name="_grid"></param>
        public ActionsSet(iGridAction _action, IGridView _grid) {
            Actions = new List<iGridAction>() { _action };
            ExecutedAction = new List<iGridAction>();
            grid = _grid;
            scheduler = null;
        }

        /// <summary>
        /// Crea un ActionsSet.
        /// </summary>
        /// <param name="_actions"></param>
        /// <param name="_grid"></param>
        public ActionsSet(List<iGridAction> _actions, IGridView _grid) {
            Actions = _actions;
            ExecutedAction = new List<iGridAction>();
            grid = _grid;
            scheduler = null;
        }

        /// <summary>
        /// Aggiunge una action in coda alla lista delle action da eseguire.
        /// </summary>
        /// <param name="_actionToAdd"></param>
        public void AddAction(iGridAction _actionToAdd) {
            Actions.Add(_actionToAdd);
        }

        /// <summary>
        /// Aggiunge delle action in coda alla lista delle action da eseguire.
        /// </summary>
        /// <param name="_actionsToAdd"></param>
        public void AddActions(List<iGridAction> _actionsToAdd) {
            Actions.AddRange(_actionsToAdd);
        }

        /// <summary>
        /// Esegui la prossima action in lista (se presente, altrimenti)
        /// </summary>
        /// <param name="callBack"></param>
        public void ExecuteNext(Action<iGridAction> callBack = null) {
            iGridAction action = null;
            if (!IsCompleted) {
                action = Actions[0];
                action.DoAction(grid);
                ExecutedAction.Add(action);
                Actions.Remove(action);
            }
            if (callBack != null)
                callBack(action);
        }

        IDisposable scheduler;

        /// <summary>
        /// TODO:
        /// Setta l'auto execution delle action appena la collection Actions cambia.
        /// </summary>
        /// <param name="_autoExecution"></param>
        public void SetAutoExecution(bool _autoExecution) {
            if (_autoExecution) {
                scheduler = Actions.ObserveEveryValueChanged(a => a.Count).Subscribe(_ => {
                    // ExecuteNext();
                });
            } else {
                if (scheduler != null)
                    scheduler.Dispose();
            }
        }
        
    }

    #endregion

    #region Moves
    /// <summary>
    /// Rappresenta una struttura dati per una GridAction di tipo singleMovement.
    /// </summary>
    public struct GridMove {
        public Position2D StartPosition;
        public Position2D DestPosition;
        public GridDirectionMove Direction;

        #region constructors
        public GridMove(Position2D _startPosition, GridDirectionMove _direction, Position2D _endPostion) {
            StartPosition = _startPosition;
            Direction = _direction;
            DestPosition = _endPostion;
        }

        public GridMove(Position2D _startPosition, GridDirectionMove _direction) {
            StartPosition = _startPosition;
            Direction = _direction;
            DestPosition = _startPosition; // devo prima assegnare tutto...
            DestPosition = GetGridPositionDestination(StartPosition, Direction);
        }

        public GridMove(Position2D _startPosition, Position2D _endPostion) {
            StartPosition = _startPosition;
            DestPosition = _endPostion;
            Direction = GridDirectionMove.none;
        }
        #endregion

        /// <summary>
        /// Restituisce la position di destinazione data una posizione di partenza e una direzione di movimento.
        /// </summary>
        /// <param name="_startPosition"></param>
        /// <param name="_direction"></param>
        /// <returns></returns>
        public static Position2D GetGridPositionDestination(Position2D _startPosition, GridDirectionMove _direction) {
            switch (_direction) {
                case GridDirectionMove.up:
                    return new Position2D(_startPosition.x, _startPosition.y + 1);
                case GridDirectionMove.right:
                    return new Position2D(_startPosition.x + 1, _startPosition.y);
                case GridDirectionMove.down:
                    return new Position2D(_startPosition.x, _startPosition.y - 1);
                case GridDirectionMove.left:
                    return new Position2D(_startPosition.x - 1, _startPosition.y);
                case GridDirectionMove.none:
                default:
                    return _startPosition;
            }
        }

        /// <summary>
        /// Restituisce la direction in un movimento da una position ad un'altra.
        /// </summary>
        /// <param name="_from"></param>
        /// <param name="_to"></param>
        /// <returns></returns>
        public static GridDirectionMove GetMovementDirectionFrom2Positions(Position2D _from, Position2D _to) {
            int xMov = _from.x > _to.x ? _from.x - _to.x : _to.x - _from.x;
            int yMov = _from.y > _to.y ? _from.y - _to.y : _to.y - _from.y;
            //if (xMov != 0 && yMov != 0)
            //      return GridDirectionMove.none;
            if (xMov > yMov) {
                // sta andando in orizzontale.
                if (_from.x > _to.x) {
                    return GridDirectionMove.left;
                } else {
                    return GridDirectionMove.right;
                }
            } else if (yMov > xMov) {
                // sta andando in verticale.
                if (_from.y > _to.y) {
                    return GridDirectionMove.down;
                } else {
                    return GridDirectionMove.up;
                }
            }
            return GridDirectionMove.none;
        }

        /// <summary>
        /// Restituisce la direzione contraria a quella passata.
        /// </summary>
        /// <param name="_direction"></param>
        /// <returns></returns>
        public static GridDirectionMove GetReverseDirection(GridDirectionMove _direction) {
            switch (_direction) {
                case GridDirectionMove.up:
                    return GridDirectionMove.down;
                case GridDirectionMove.right:
                    return GridDirectionMove.left;
                case GridDirectionMove.down:
                    return GridDirectionMove.up;
                case GridDirectionMove.left:
                    return GridDirectionMove.right;
                default:
                    return GridDirectionMove.none;
            }
        }

        /// <summary>
        /// Restituisce la direzione attuale inversa. Non modifca l'oggetto.
        /// </summary>
        /// <returns></returns>
        public GridDirectionMove RevertDirection() {
            switch (Direction) {
                case GridDirectionMove.up:
                    return GridDirectionMove.down;
                case GridDirectionMove.right:
                    return GridDirectionMove.left;
                case GridDirectionMove.down:
                    return GridDirectionMove.up;
                case GridDirectionMove.left:
                    return GridDirectionMove.right;
                default:
                    return GridDirectionMove.none;
            }
        }

        #region Static Constructors
        /// <summary>
        /// Restitiusce la lista dei 4 movimenti cardinali partendo dalla posizione passata.
        /// </summary>
        /// <param name="_position"></param>
        /// <returns></returns>
        public static List<GridMove> AllCardinalsMoves(Position2D _position) {
            List<GridMove> returnList = new List<GridMove>();
            returnList.Add(new GridMove(_position, GridDirectionMove.up));
            returnList.Add(new GridMove(_position, GridDirectionMove.right));
            returnList.Add(new GridMove(_position, GridDirectionMove.down));
            returnList.Add(new GridMove(_position, GridDirectionMove.left));
            return returnList;
        }
        /// <summary>
        /// Lista vuota.
        /// </summary>
        /// <returns></returns>
        public static List<GridMove> None { get { return new List<GridMove>(); } }
        #endregion
    }

    public struct GridMoveSet {
        public GridMove Movement;
        public ICellView Element;
        public GridMoveSet(GridMove _movement, ICellView _gridElement) {
            Movement = _movement;
            Element = _gridElement;
        }
    }

    /// <summary>
    /// Direzioni possibili nella griglia.
    /// </summary>
    public enum GridDirectionMove {
        none = 0,
        up = 1,
        right = 2,
        down = 3,
        left = 4,
    }
    #endregion

    #endregion

}