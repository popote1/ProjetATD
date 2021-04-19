using System.Collections.Generic;
using UnityEngine;

namespace CurveTuto
{
    [System.Serializable]
    public class Path
    {
        [SerializeField , HideInInspector]
        private List<Vector2> _points;
        [SerializeField, HideInInspector] 
        private bool _isClosed;
        [SerializeField, HideInInspector] 
        private bool autoSetControlPoints;

        public Path(Vector2 Center) {
            _points = new List<Vector2> {
                Center+Vector2.left,
                Center+ (Vector2.left+Vector2.up)*0.5f,
                Center+(Vector2.right+Vector2.down)*0.5f,
                Center+Vector2.right
            };
        }

        public Vector2 this[int i] {
            get {
                return _points[i];
            }
        }

        public bool IsClosed {
            get => _isClosed;
            set {
                _isClosed = value;
               ToggleClosed();
            }
        }

        public bool AutoSetControlPoints
        {
            get => autoSetControlPoints;
            set {
                if (autoSetControlPoints != value){
                    autoSetControlPoints = value;
                    if(autoSetControlPoints){
                        AutoSetAllControlPoints();
                    }
                }
            }
        }

        public int NumPoints {
            get {
                return _points.Count;
            }
        }
        
        public int NumSegments {
            get {
                return _points.Count/3;
            }
        }

        public void AddSegment(Vector2 anchorPos) {
            _points.Add(_points[_points.Count-1]*2-_points[_points.Count-2]);
            _points.Add(_points[_points.Count-1]+anchorPos*0.5f);
            _points.Add(anchorPos);
            if (autoSetControlPoints)AutoSetAllAffectedContolPonits(_points.Count-1);
        }

        public void SplitSegment(Vector2 anchorPos, int segmentIndex)
        {
            _points.InsertRange(segmentIndex*3+2,new Vector2[]{Vector2.zero, anchorPos, Vector2.zero});
            if (autoSetControlPoints)
            {
                AutoSetAllAffectedContolPonits(segmentIndex*3+3);
            }
            else
            {
                AutoSetAnchorControlPoints(segmentIndex*3+3);
            }
        }

        public void DeleteSemente(int anchorIndex)
        {
            if (NumSegments > 2 || !_isClosed && NumSegments > 1)
            {
                if (anchorIndex == 0)
                {
                    if (_isClosed)
                    {
                        _points[_points.Count - 1] = _points[2];
                    }
                    _points.RemoveRange(0,3);
                }
                else if (anchorIndex == _points.Count - 1 && !_isClosed)
                {
                    _points.RemoveRange(anchorIndex -2,3);
                }
                else
                {
                    _points.RemoveRange(anchorIndex-1,3);
                }
            }
        }

        public Vector2[] GetPointsInSegment(int i) {
            return new Vector2[]{_points[i*3],_points[i*3+1],_points[i*3+2],_points[LoopIndex(i*3+3)]};
        }

        public void MovePoints(int i, Vector2 pos) {
            Vector2 deltaMove = pos - _points[i];
            if (i % 3 == 0 || !autoSetControlPoints) {
                _points[i] = pos;
                if (autoSetControlPoints) {
                    AutoSetAnchorControlPoints(i);
                }
                else {
                    if (i % 3 == 0) {
                        if (i + 1 < _points.Count || _isClosed) _points[LoopIndex(i + 1)] += deltaMove;
                        if (i - 1 >= 0 || _isClosed) _points[LoopIndex(i - 1)] += deltaMove;
                    }
                    else {
                        bool nextPointIsAnchor = (i + 1) % 3 == 0;
                        int correspondingControlIndex = (nextPointIsAnchor) ? i + 2 : i - 2;
                        int anchorIndex = (nextPointIsAnchor) ? i + 1 : i - 1;
                        if (correspondingControlIndex >= 0 && correspondingControlIndex < _points.Count || _isClosed) {
                            float dst =
                                (_points[LoopIndex(anchorIndex)] - _points[LoopIndex(correspondingControlIndex)])
                                .magnitude;
                            Vector2 dir = (_points[LoopIndex(anchorIndex)] - pos).normalized;
                            _points[LoopIndex(correspondingControlIndex)] = _points[LoopIndex(anchorIndex)] + dir * dst;
                        }
                    }
                }
            }
        }

        public void ToggleClosed()
        {
            if (_isClosed)
            {
                _points.Add(_points[_points.Count-1]*2-_points[_points.Count-2]);
                _points.Add(_points[0]*2-_points[1]);
                if (autoSetControlPoints)
                {
                    AutoSetAnchorControlPoints(0);
                    AutoSetAnchorControlPoints(_points.Count-3);
                }
            }
            else
            {
                _points.RemoveRange(_points.Count - 2, 2);
                if (autoSetControlPoints)AutoSetStartAndEndContols();
            }
        }

        public Vector2[] CalculateEvenlySpacePoints(float spacing, float resolition = 1) {
            List<Vector2> evenlySpacePoints = new List<Vector2>();
            evenlySpacePoints.Add(_points[0]);
            Vector2 previousPoint = _points[0];
            float dstSinceLastEvenPointe = 0;
            for (int segmentIndex = 0; segmentIndex < NumSegments; segmentIndex++) {
                Vector2[] p = GetPointsInSegment(segmentIndex);
                float controlNetLenght = Vector2.Distance(p[0], p[1]) + Vector2.Distance(p[1], p[2]) + Vector2.Distance(p[2], p[3]);
                float estimatedCurveLenght = Vector2.Distance(p[0], p[3]) + controlNetLenght / 2;
                int divisions = Mathf.CeilToInt(estimatedCurveLenght * resolition * 10);
                float t = 0;
                while (t <= 1) {
                    t += 1f / divisions;
                    Vector2 pointOnCurve = Bezier.EvaluateCubic(p[0], p[1], p[2], p[3], t);
                    dstSinceLastEvenPointe += Vector2.Distance(previousPoint, pointOnCurve);
                    while (dstSinceLastEvenPointe >= spacing) {
                        float overShootDst = dstSinceLastEvenPointe - spacing;
                        Vector2 newEvenlySpacedPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized * overShootDst;
                        evenlySpacePoints.Add(newEvenlySpacedPoint );
                        dstSinceLastEvenPointe = overShootDst;
                        previousPoint = newEvenlySpacedPoint;
                    }
                    previousPoint = pointOnCurve;
                }
            }
            return evenlySpacePoints.ToArray();
        }
        public Vector2[] CalculateEvenlySpacePointsOnSegment(float spacing, int segment, float resolition = 1f) {
            List<Vector2> evenlySpacePoints = new List<Vector2>();
            evenlySpacePoints.Add(_points[segment*3]);
            Vector2 previousPoint = _points[segment*3];
            float dstSinceLastEvenPointe = 0;
            Vector2[] p = GetPointsInSegment(segment);
            float controlNetLenght = Vector2.Distance(p[0], p[1]) + Vector2.Distance(p[1], p[2]) + Vector2.Distance(p[2], p[3]);
            float estimatedCurveLenght = Vector2.Distance(p[0], p[3]) + controlNetLenght / 2;
            int divisions = Mathf.CeilToInt(estimatedCurveLenght * resolition * 10);
            float t = 0; 
            while (t <= 1) { 
                t += 1f / divisions;
                Vector2 pointOnCurve = Bezier.EvaluateCubic(p[0], p[1], p[2], p[3], t);
                dstSinceLastEvenPointe += Vector2.Distance(previousPoint, pointOnCurve);
                while (dstSinceLastEvenPointe >= spacing) {
                    float overShootDst = dstSinceLastEvenPointe - spacing;
                    Vector2 newEvenlySpacedPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized *
                        overShootDst; evenlySpacePoints.Add(newEvenlySpacedPoint );
                    dstSinceLastEvenPointe = overShootDst;
                    previousPoint = newEvenlySpacedPoint;
                }
                previousPoint = pointOnCurve;
            }
            return evenlySpacePoints.ToArray();
        }

        private void AutoSetAllAffectedContolPonits(int updatedAnchorIndex)
        {
            for (int i = updatedAnchorIndex - 3; i <= updatedAnchorIndex + 3; i += 3)
                if (i >= 0 && i < _points.Count || _isClosed)
                    AutoSetAnchorControlPoints(LoopIndex(i));
            AutoSetStartAndEndContols();
        }

        private void AutoSetAllControlPoints()
        {
            for (int i = 0; i < _points.Count; i += 3) AutoSetAnchorControlPoints(LoopIndex(i));
            AutoSetStartAndEndContols();
        }

        private void AutoSetAnchorControlPoints(int anchorIndex)
        {
            Vector2 anchorPos = _points[anchorIndex];
            Vector2 dir = Vector2.zero;
            float[] neighbourDistances = new float[2];

            if (anchorIndex - 3 >= 0 || _isClosed)
            {
                Vector2 offset = _points[LoopIndex(anchorIndex - 3)] - anchorPos;
                dir += offset.normalized;
                neighbourDistances[0] = offset.magnitude;
            }
            if (anchorIndex +3 >= 0 || _isClosed)
            {
                Vector2 offset = _points[LoopIndex(anchorIndex +3)] - anchorPos;
                dir -= offset.normalized;
                neighbourDistances[1] = -offset.magnitude;
            }

            dir.Normalize();

            for (int i = 0; i < 2; i++) {
                int controlIndex = anchorIndex + i * 2 - 1;
                if (controlIndex >= 0 && controlIndex < _points.Count || _isClosed) {
                    _points[LoopIndex(controlIndex)] = anchorPos + dir * neighbourDistances[i] * 0.5f;
                }
            }
        }

        private void AutoSetStartAndEndContols() {
            if (!_isClosed) {
                _points[1] = (_points[0] + _points[2]) * 0.5f;
                _points[_points.Count - 2] = (_points[_points.Count - 1] + _points[_points.Count - 3]) * 0.5f;
            }
        }

        private int LoopIndex(int i)
        {
            return (i + _points.Count) % _points.Count;
        }
    }
}
