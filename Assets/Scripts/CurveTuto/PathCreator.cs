using UnityEngine;

namespace CurveTuto
{
    public class PathCreator : MonoBehaviour
    {
        [HideInInspector] public Path Path;
        
        public Color AnchorCol = Color.red;
        public Color ControlCol = Color.white;
        public Color SegmentCol = Color.green;
        public Color SelectedCol = Color.yellow;

        public float AnchorDiamater = 0.1f;
        public float ControlDiameter = 0.075f;
        public bool DisplayControlPoints = true;
        public void CreatePath()
        {
            Path = new Path(transform.position);
        }

        private void Reset()
        {
            CreatePath();
        } 
    }
}
