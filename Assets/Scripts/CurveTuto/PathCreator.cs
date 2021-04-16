using UnityEngine;

namespace CurveTuto
{
    public class PathCreator : MonoBehaviour
    {
        [HideInInspector] public Path Path;

        public void CreatePath()
        {
            Path = new Path(transform.position);
        }
    }
}
