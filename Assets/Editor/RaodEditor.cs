
using CurveTuto;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoadCreator))]
public class RaodEditor : Editor
{
   private RoadCreator _creator;

   private void OnSceneGUI()
   {
      Debug.Log("OnSceneGUI");
      if (_creator.autoUpdate&& Event.current.type == EventType.Repaint)
      {
         _creator.UpdateRoad();
      }
   }

   private void OnEnable()
   {
      _creator = (RoadCreator) target;
   }
}
