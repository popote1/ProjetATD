using System;
using DG.Tweening;
using UnityEngine;
using PlaneC;
using Unity.Mathematics;

namespace Components
{
    public class PlayerManagerComponent : MonoBehaviour
    {
        public GameManagerComponent GameManagerComponent;

        public static int Gold;
        //public static List<Batiment> Batiments;


        [Header("Cursor Configue")] public GameObject PrefabCursor;
        [Range(0, 100)] public float CursorSmoothFactor;
        public Camera Camera;


        private PlayGrig _playGrig;
        private GameObject _cursor;
        private Vector3 _cursorTaget;
        private Vector2Int _selsectdCell;

        private void Start()
        {
            
            _cursor = Instantiate(PrefabCursor, Vector3.zero, quaternion.identity);
        }

        private void Update()
        {
            if(_playGrig== null)_playGrig = GameManagerComponent.PlayGrig;
            RaycastHit hit;
            if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Debug.Log(_playGrig.GetCellGridPosByWorld(hit.point));
                if (_playGrig.CheckIfInGrid(_playGrig.GetCellGridPosByWorld(hit.point)))
                {
                    _selsectdCell = _playGrig.GetCellGridPosByWorld(hit.point);
                    _cursorTaget = _playGrig.GetCellCenterWorldPosByCell(_selsectdCell)+new Vector3(0,0,-0.5f);
                }
                
                if (_cursor != null)
                    _cursor.transform.position = Vector3.Lerp(_cursor.transform.position, _cursorTaget,
                        Time.deltaTime * CursorSmoothFactor);
            }
        }
    }
}