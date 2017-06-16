using UnityEditor;
using UnityEngine;

namespace ModularFramework.AI {

    [CustomEditor(typeof(PatrollerComponent))]
    public class PatrollerComponentEditor : Editor {

        protected virtual void OnSceneGUI() {
            PatrollerComponent component = target as PatrollerComponent;





            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < component.Data.PatrolPoints.Count; i++) {
                PatrolPoint pp = component.Data.PatrolPoints[i];
                Vector3 newTargetPosition = Handles.DoPositionHandle(pp.Position, Quaternion.identity);
                pp.Position = newTargetPosition;
                pp.Order = i;
                Handles.Label(newTargetPosition, pp.Order.ToString(), 
                    new GUIStyle(){ fontSize = 20, fontStyle = FontStyle.BoldAndItalic });
                Handles.DrawLine(i > 0 ? component.Data.PatrolPoints[i - 1].Position : component.transform.position, pp.Position);
            }
            
            if (EditorGUI.EndChangeCheck()) {
                //Undo.RecordObject(patrolData, "Change Look At Target Position");
                
                //component.Update();
            }

            
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            PatrollerComponent component = target as PatrollerComponent;

            if (GUILayout.Button(string.Format("Add", new GUILayoutOption[] { GUILayout.Width(80), GUILayout.Height(100) }))) {
                PatrolPoint newPP = new PatrolPoint();
                newPP.Position = component.Data.PatrolPoints.Count > 0 ? component.Data.PatrolPoints[component.Data.PatrolPoints.Count - 1].Position : component.transform.position;
                component.Data.PatrolPoints.Add(newPP);
            }
        }

    }
}