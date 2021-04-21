using UnityEngine;

namespace CurveTuto
{
    public class PathPlacer : MonoBehaviour
    {
        public float Spacing = 0.1f;
        public float Resolution = 1f;

        private void Start()
        {
            Vector2[] points = FindObjectOfType<PathCreator>().Path.CalculateEvenlySpacePoints(Spacing, Resolution);
            foreach (Vector2 p in points)
            {
                GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                g.transform.position = p;
                g.transform.localScale = Vector3.one * Spacing * 0.5f;
            }
        }
    }
}
