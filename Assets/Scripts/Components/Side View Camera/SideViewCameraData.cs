using NaughtyAttributes;
using UDT.Core;
using UnityEngine;

namespace BSH.Cameras
{
    [CreateAssetMenu(fileName = "SideViewCameraData", menuName = "BSH/SideViewCameraData", order = 0)]
    public class SideViewCameraData : ComponentData<SideViewCameraComponent>
    {
        [Tooltip("Should the Camera be generated from this Data?")]
        public bool generateCamera = true;
        [ShowIf("generateCamera")]
        [Tooltip("A Path to the Camera GameObject within the StandardObject Prefab")]
        public string cameraPath = "camera";
        
    }
}