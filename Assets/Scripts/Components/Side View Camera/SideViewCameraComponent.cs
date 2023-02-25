using Cinemachine;
using UDT.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace BSH.Cameras
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class SideViewCameraComponent : StandardComponent<SideViewCameraData, CameraSystem>
    {
        [Tooltip("The Camera GameObject")]
        public GameObject cameraGameObject;
        [Tooltip("The Virtual Camera Component")]
        public CinemachineVirtualCamera virtualCamera;

        public override void OnAddComponent()
        {
            if (cameraGameObject == null)
            {
                cameraGameObject = Data.generateCamera ? gameObject.GetComponentInChildren<CinemachineVirtualCamera>().gameObject : new GameObject();
                if (cameraGameObject == null) cameraGameObject = new GameObject();
                
                cameraGameObject.transform.SetParent(transform);
            }
            virtualCamera = cameraGameObject.GetComponent<CinemachineVirtualCamera>();
            if(virtualCamera == null) virtualCamera = cameraGameObject.AddComponent<CinemachineVirtualCamera>();
        }
    }
}