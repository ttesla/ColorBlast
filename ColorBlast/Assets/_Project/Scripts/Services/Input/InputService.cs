using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace ColorBlast
{
    public class InputService : MonoBehaviour, IInputService
    {
        [SerializeField] private LayerMask TileMask;

        public event Action<Tile> Tapped;

        private Camera mCamera;
        private bool mSessionStarted;
        private IGameService mGameService;

        public void Init()
        {
            mCamera = Camera.main;
            mSessionStarted = false;
            mGameService = ServiceManager.Instance.Get<IGameService>();
            mGameService.SessionStarted += OnSessionStarted;
            mGameService.SessionEnded   += OnSessionEnded;
            
            Logman.Log("InputService - Init");
        }

        public void Release()
        {
            mGameService.SessionStarted -= OnSessionStarted;
            mGameService.SessionEnded   -= OnSessionEnded;
            
            Logman.Log("InputService - Release");
        }

        private void Update()
        {
            if(mSessionStarted) 
            {
                InputUpdate();
            }
        }

        private void OnSessionStarted()
        {
            mSessionStarted = true;
        }

        private void OnSessionEnded()
        {
            mSessionStarted = false;
        }

        private void InputUpdate()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButtonDown(0))
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
                var tile = hit.collider.gameObject.GetComponent<Tile>();
                Tapped?.Invoke(tile);
            }
        }
    }
}
