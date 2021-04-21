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

        private Path _path
        {
            get
            {
                return _creator.Path;
            }
        }

        private const float segmentSelectDistanceThreshold = 0.1f;
        private int selectedSegment = -1;

        public override void OnInspectorGUI()
        {
            
            
            base.OnInspectorGUI();
            EditorGUI.BeginChangeCheck();
            if (GUILayout.Button("Create new"))
            {
                Undo.RecordObject(_creator , "Create new");
                _creator.CreatePath();
            }

            bool IsClosed = GUILayout.Toggle(_path.IsClosed, "Closed");
            if (IsClosed != _path.IsClosed)
            {
                Undo.RecordObject(_creator , "Toggle closed");
                _path.IsClosed = IsClosed;
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
                if (selectedSegment != -1)
                {
                    Undo.RecordObject(_creator, "Split segment");
                    _path.SplitSegment(mousePos, selectedSegment);
                }
                else if (!_path.IsClosed)
                {
                    Undo.RecordObject(_creator, " Add Segment");
                    _path.AddSegment(mousePos);
                }
            }

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
            {
                float minDstToAnchor =_creator.AnchorDiamater*0.5f;
                int closestAnchorIndex = 1;
                for (int i = 0; i < _path.NumPoints; i+=3) {
                    float dst = Vector2.Distance(mousePos, _path[i]);
                    if (dst < minDstToAnchor) {
                        minDstToAnchor = dst;
                        closestAnchorIndex = i;
                    }
                }
                if (closestAnchorIndex != -1) {
                    Undo.RecordObject(_creator, " Delete segment");
                    _path.DeleteSemente(closestAnchorIndex);
                }
            }

            if (guiEvent.type == EventType.MouseMove)
            {
                float minDstToSegment = segmentSelectDistanceThreshold;
                int newSelectedSegmentIndex = -1;
                for (int i = 0; i < _path.NumSegments; i++)
                {
                    Vector2[] points = _path.GetPointsInSegment(i);
                    float dst = HandleUtility.DistancePointBezier(mousePos, points[0], points[3], points[1], points[2]);
                    if (dst < minDstToSegment)
                    {
                        minDstToSegment = dst;
                        newSelectedSegmentIndex = i;
                    }

                    if (newSelectedSegmentIndex != selectedSegment)
                    {
                        selectedSegment = newSelectedSegmentIndex;
                        HandleUtility.Repaint();
                    }
                }
            }
            HandleUtility.AddDefaultControl(0);
        }
        private void Draw()
        {
            for (int i = 0; i < _path.NumSegments; i++)
            {
                Vector2[] points = _path.GetPointsInSegment(i);
                if (_creator.DisplayControlPoints)
                {
                    Handles.color = Color.black;
                    Handles.DrawLine(points[1], points[0]);
                    Handles.DrawLine(points[2], points[3]);
                }

                Color segmentCol = (i == selectedSegment && Event.current.shift) ? _creator.SelectedCol: _creator.SegmentCol;
                Handles.DrawBezier(points[0], points[3], points[1], points[2], segmentCol, null, 2);
            }
            
            for (int i = 0; i < _path.NumPoints; i++) {
                if (i % 3 == 0 || _creator.DisplayControlPoints)
                {
                    Handles.color = (i % 3 == 0) ? _creator.AnchorCol : _creator.ControlCol;
                    float handleSize = (i % 3 == 0) ? _creator.AnchorDiamater : _creator.ControlDiameter;
                    Vector2 newPos = Handles.FreeMoveHandle(_path[i], quaternion.identity, handleSize, Vector2.zero,
                        Handles.CylinderHandleCap);
                    if (_path[i] != newPos)
                    {
                        Undo.RecordObject(_creator, "Move Point");
                        _path.MovePoints(i, newPos);
                    }
                }
            }
        } 
        private void OnEnable() {
            _creator = (PathCreator)target;
            if (_creator.Path == null) {
                _creator.CreatePath();
            }
        }
    }
}
