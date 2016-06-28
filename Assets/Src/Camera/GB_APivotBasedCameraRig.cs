using UnityEngine;


namespace GBAssets.CameraControl
{
    public abstract class GB_APivotBasedCameraRig : GB_ATargetFollower
    {
        // This script is designed to be placed on the root object of a camera rig,
        // comprising 3 gameobjects, each parented to the next:

        // 	Camera Rig
        // 		Pivot
        // 			Clamp

        protected Transform m_Clamp; // the transform of the camera
        protected Transform m_Pivot; // the point at which the camera pivots around
        protected Vector3 m_LastTargetPosition;


        protected virtual void Awake()
        {
            // find the camera in the object hierarchy
            m_Clamp = GetComponentInChildren<GB_CamClamp>().transform;
            m_Pivot = m_Clamp.parent;
        }
    }
}
