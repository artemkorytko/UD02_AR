using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace DefaultNamespace
{
    public class FieldPositionController : MonoBehaviour
    {
        private ARRaycastManager _arRaycastManager;
        private ARPlaneManager _arPlaneManager;
        private ARAnchorManager _arAnchorManager;
        
        public bool IsActive { get; private set; }

        private void Awake()
        {
            _arRaycastManager = FindObjectOfType<ARRaycastManager>();
            _arPlaneManager = FindObjectOfType<ARPlaneManager>();
            _arAnchorManager = FindObjectOfType<ARAnchorManager>();
        }

        private void Update()
        {
            if (_arRaycastManager == null) return;
            if (!IsActive) return;

            Vector2 position = new Vector2(Screen.currentResolution.width * 0.5f,
                                           Screen.currentResolution.height * 0.5f);

            List<ARRaycastHit> arRaycastHits = new List<ARRaycastHit>();

            if (_arRaycastManager.Raycast(position, arRaycastHits, TrackableType.PlaneWithinPolygon))
            {
                Pose pose = arRaycastHits[0].pose;
                TrackableId hitTrackableId = arRaycastHits[0].trackableId;
                ARPlane arPlane = _arPlaneManager.GetPlane(hitTrackableId);
                ARAnchor arAnchorPoint = _arAnchorManager.AttachAnchor(arPlane, pose);
                gameObject.transform.position = arAnchorPoint.transform.position;
                gameObject.transform.rotation = pose.rotation;
            }
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }
    }
}