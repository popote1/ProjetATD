using System;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEngine;
using UnityEditor;



namespace CurveTuto
{
    [CustomEditor(typeof(PathCreator))]
    public class PathEditor : Editor
    {
        private PathCreator _creator;
        private Path _path;

        public override void OnInspectorGUI()
        {
            
            
            base.OnInspectorGUI();
            EditorGUI.BeginChangeCheck();
            if (GUILayout.Button("Create new"))
            {
                Undo.RecordObject(_creator , "Create new");
                _creator.CreatePath();
                _path = _creator.Path;
            }

            if (GUILayout.Button("Togglle closed"))
            {
                Undo.RecordObject(_creator , "Toggle closed");
                _path.ToggleClosed();
            }

            bool autoSetControlPoints = GUILayout.Toggle(_path.AutoSetControlPoints, "Auto Set Control Points");
            if (autoSetControlPoints != _path.AutoSetControlPoints)
            {
                Undo.RecordObject(_creator , "Toggle auto se controls");
                _path.AutoSetControlPoints = autoSetControlPoints;
            }
            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }
        }


        private void OnSceneGUI() {
            Input();
            Draw();
        }

        private void Input()
        {
            Event guiEvent = Event.current;
            Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
            {
                Undo.RecordObject(_creator, " Add Segment");
                _path.AddSegment(mousePos);
            }
        }
        private void Draw()
        {
            for (int i = 0; i < _path.NumSegments; i++)
            {
                Vector2[] points = _path.GetPointsInSegment(i);
                Handles.color = Color.black;
                Handles.DrawLine(points[1], points[0]);
                Handles.DrawLine(points[2], points[3]);
                Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.green, null, 2);
            }
            
            
            
            Handles.color = Color.red;
            for (int i = 0; i < _path.NumPoints; i++) {
                Vector2 newPos = Handles.FreeMoveHandle(_path[i], quaternion.identity, 0.1f, Vector2.zero, Handles.CylinderHandleCap);
                if (_path[i] != newPos) {
                    Undo.RecordObject(_creator ,"Move Point");
                    _path.MovePoints(i , newPos);
                }
            }
        } 
        private void OnEnable() {
            _creator = (PathCreator)target;
            if (_creator.Path == null) {
                _creator.CreatePath();
            }
            _path = _creator.Path;
        }
    }
}
