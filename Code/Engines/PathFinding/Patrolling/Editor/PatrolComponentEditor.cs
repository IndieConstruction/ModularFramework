using UnityEditor;
using UnityEngine;

namespace ModularFramework.AI {
    /// <summary>
    /// Custom Editor for components of type PatrollerComponent.
    /// Features:
    /// - Add, (remove) and move patrol point ordered priority.
    /// </summary>
    [CustomEditor(typeof(PatrolComponent))]
    public class PatrolComponentEditor : Editor {

        protected virtual void OnSceneGUI() {
            PatrolComponent component = target as PatrolComponent;
            Handles.color = Color.red;
            Handles.FreeMoveHandle(component.transform.position, Quaternion.identity, 0.4f, Vector3.one, Handles.CircleHandleCap);

            Handles.color = Color.green;
            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < component.Data.PatrolPoints.Count; i++) {
                PatrolPoint pp = component.Data.PatrolPoints[i];
                
                Vector3 newTargetPosition = Handles.FreeMoveHandle(pp.Position, Quaternion.identity, 0.4f, Vector3.one, Handles.CircleHandleCap);
                pp.Position = newTargetPosition;
                pp.Order = i;
                Handles.Label(newTargetPosition, pp.Order.ToString(),
                    new GUIStyle() { fontSize = 20, fontStyle = FontStyle.BoldAndItalic, alignment = TextAnchor.MiddleCenter, });
                if (component.Data.Type == PatrolType.LINEAR) {
                    Handles.color = Color.cyan;
                    Handles.DrawDottedLine(i > 0 ? component.Data.PatrolPoints[i - 1].Position : component.transform.position, pp.Position, 2.5f);
                } else {
                    Handles.color = Color.yellow;
                    Handles.DrawDottedLine(i > 0 ? component.Data.PatrolPoints[i - 1].Position : component.transform.position, pp.Position, 5.0f);
                }
            }

            if (EditorGUI.EndChangeCheck()) {
                //Undo.RecordObject(patrolData, "Change Look At Target Position");
                
                //component.Update();
            }

            // keyboard shortcut
            Event e = Event.current;
            switch (e.type) {
                case EventType.mouseDown:
                    if (Event.current.control) {
                        AddPatrolPoint(component, true);
                    } else if (Event.current.shift) { 
                        RemovePatrolPoint(component);
                    }
                    break;
            }



        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            PatrolComponent component = target as PatrolComponent;

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(string.Format("Add", new GUILayoutOption[] { GUILayout.Width(80), GUILayout.Height(100) }))) {
                AddPatrolPoint(component);
            }
            if (GUILayout.Button(string.Format("Remove", new GUILayoutOption[] { GUILayout.Width(80), GUILayout.Height(100) }))) {
                RemovePatrolPoint(component);
            }
            EditorGUILayout.EndHorizontal();

        }

        #region internal functions

        /// <summary>
        /// Add patrol point.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="onMousePosition"></param>
        void AddPatrolPoint(PatrolComponent component, bool onMousePosition = false) {
            PatrolPoint newPP = new PatrolPoint();
            if (onMousePosition)
                newPP.Position = GetMousePositionInEditor(component.transform);
            else
                newPP.Position = component.Data.PatrolPoints.Count > 0 ? component.Data.PatrolPoints[component.Data.PatrolPoints.Count - 1].Position : component.transform.position;
            component.Data.PatrolPoints.Add(newPP);
        }

        /// <summary>
        /// Return mouse position in scene view.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        Vector3 GetMousePositionInEditor(Transform transform) {
            // mouse position
            Vector3 mousePosition = Event.current.mousePosition;
            mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y;
            mousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);
            //mousePosition.y = -mousePosition.y;
            mousePosition.z = transform.position.z;
            return mousePosition;
        }

        /// <summary>
        /// Remove last patrol point.
        /// </summary>
        /// <param name="component"></param>
        void RemovePatrolPoint(PatrolComponent component) {
            if (component.Data.PatrolPoints.Count > 0)
                component.Data.PatrolPoints.RemoveAt(component.Data.PatrolPoints.Count - 1);
        }
        
        #endregion

    }
}