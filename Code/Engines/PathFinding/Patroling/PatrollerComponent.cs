using System;
using System.Collections;
using System.Collections.Generic;
using ModularFramework.Data;
using UnityEngine;

namespace ModularFramework.AI {

    public class PatrollerComponent : MonoBehaviour, IPatroller {

        public PatrolData Data;
        public List<Vector3> PatrolPoints { get; set; }
        public List<Node> PatrolPath { get; set; }

        IPatroller IPatroller.PatrollerComponent { get { return this; } }

        /// <summary>
        /// Elabora i dati attuali e li restituisce nel foramto corretto per il salvataggio.
        /// In questo caso le posizioni diventano relative.
        /// </summary>
        /// <returns></returns>
        public IData GetDataForSave() {
            PatrolData returnData = new PatrolData();
            foreach (PatrolPoint pp in Data.PatrolPoints) {
                PatrolPoint newPatrolPoint = new PatrolPoint() {
                    Order = pp.Order,
                    Position = pp.Position - transform.position
                };
                returnData.PatrolPoints.Add(newPatrolPoint);
            }
            return returnData;
        }

        /// <summary>
        /// Elabora i dati salvati su disco in modo da renderli pronti per essere iniettati negli oggetti di scena.
        /// </summary>
        /// <returns></returns>
        public IData SetDataFromDisk(IData _inputData) {
            PatrolData returnData = new PatrolData();
            foreach (PatrolPoint pp in (_inputData as PatrolData).PatrolPoints) {
                PatrolPoint newPatrolPoint = new PatrolPoint() {
                    Order = pp.Order,
                    Position = pp.Position + transform.position
                };
                returnData.PatrolPoints.Add(newPatrolPoint);
            }
            return returnData;
        }

        public PatrolData GetPatrolData() {

            return Data;
        }


        public void Setup(Grid grid) {
            
        }

    }
}
