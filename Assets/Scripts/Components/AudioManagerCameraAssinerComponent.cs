using UnityEngine;

namespace Components
{
   public class AudioManagerCameraAssinerComponent : MonoBehaviour
   {
      public void Start()
      {
         AudioManager.SetCameraTransform(transform);   
      }
   }
}
