using UnityEngine;


namespace CurveTuto
{
    public static class Bezier 
    {

        public static Vector2 EvaluatQuadratic(Vector2 a, Vector2 b, Vector2 c, float t)
        {
            Vector2 p0 = Vector2.Lerp(a,b,t);
            Vector2 P1 = Vector2.Lerp(b,c,t);
            return Vector2.Lerp(p0, P1, t);
        }

        public static Vector2 EvaluateCubic(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
        {
            Vector2 P0 = EvaluatQuadratic(a, b, c, t);
            Vector2 P1 = EvaluatQuadratic(b, c, d, t);
            return  Vector2.Lerp( P0 , P1 , t);
        }
    }
}
