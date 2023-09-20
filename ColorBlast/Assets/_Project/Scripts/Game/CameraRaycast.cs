using UnityEngine;

namespace ColorBlast
{
    public class CameraRaycast : MonoBehaviour
    {
        public LayerMask TileMask;
        
        private Camera mCamera;

        private void Awake()
        {
            mCamera = GetComponent<Camera>();    
        }

        private void Update()
        {
            InputUpdate();
        }

        private void InputUpdate() 
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            if(Input.GetMouseButtonDown(0)) 
            {
                PerformRaycast(Input.mousePosition);
            }
#elif UNITY_ANDROID || UNITY_IOS
            // Tap Event
            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) 
            {
                PerformRaycast(Input.GetTouch(0).position);
            }
#endif
        }

        private void PerformRaycast(Vector2 position)
        {
            Ray ray = mCamera.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out RaycastHit hit, 100, TileMask))
            {
                GameObject hitObject = hit.collider.gameObject;

                // Do something with the hitObject, for example, print its name
                Debug.Log("Ray hit: " + hitObject.name);

                GameObject.Find("Board").GetComponent<Board>().TryToPopTile(hitObject.GetComponent<Tile>());
            }
        }
    }
}
