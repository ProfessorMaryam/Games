using UnityEngine;
using AC;

public class ReturnPickUp : MonoBehaviour
{

    [SerializeField] private Marker markerWhenHeld = null;
    public float moveSpeed = 10f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool doReturn;
    private bool isHeld;
    private DragBase dragBase;
    private Moveable_PickUp pickUp;

    private LerpUtils.Vector3Lerp positionLerp = new LerpUtils.Vector3Lerp();
    private LerpUtils.QuaternionLerp rotationLerp = new LerpUtils.QuaternionLerp();



    private void OnEnable()
    {
        dragBase = GetComponent<DragBase>();
        pickUp = GetComponent<Moveable_PickUp>();

        EventManager.OnGrabMoveable += GrabMoveable;
        EventManager.OnDropMoveable += DropMoveable;
    }


    private void OnDisable()
    {
        EventManager.OnGrabMoveable -= GrabMoveable;
        EventManager.OnDropMoveable -= DropMoveable;
    }


    private void Update()
    {
        if (doReturn)
        {
            transform.position = positionLerp.Update(transform.position, originalPosition, moveSpeed);
            transform.rotation = rotationLerp.Update(transform.rotation, originalRotation, moveSpeed);
        }
        else if (isHeld && markerWhenHeld != null)
        {
            if (pickUp)
            {
                pickUp.OverrideMoveToPosition(markerWhenHeld.transform.position);
            }
            else
            {
                transform.position = positionLerp.Update(transform.position, markerWhenHeld.transform.position, moveSpeed);
            }
        }
    }


    private void GrabMoveable(DragBase _dragBase)
    {
        if (_dragBase == dragBase)
        {
            originalPosition = transform.position;
            originalRotation = transform.rotation;

            doReturn = false;
            isHeld = true;

            if (KickStarter.player)
            {
                KickStarter.player.upMovementLocked = KickStarter.player.downMovementLocked = KickStarter.player.leftMovementLocked = KickStarter.player.rightMovementLocked = true;
                
                //FirstPersonCamera fpCam = KickStarter.mainCamera.attachedCamera as FirstPersonCamera;

                //if (fpCam != null)
                //{
                //    fpCam.enabled = false;
                //}
            }
        }
    }


    private void DropMoveable(DragBase _dragBase)
    {
        if (_dragBase == dragBase)
        {
            doReturn = true;
            isHeld = false;

            if (moveSpeed <= 0f)
            {
                transform.position = originalPosition;
                transform.rotation = originalRotation;
            }

            if (KickStarter.player)
            {
                KickStarter.player.upMovementLocked = KickStarter.player.downMovementLocked = KickStarter.player.leftMovementLocked = KickStarter.player.rightMovementLocked = false;
                //FirstPersonCamera fpCam = KickStarter.mainCamera.attachedCamera as FirstPersonCamera;

                //if (fpCam != null)
                //{
                //    fpCam.enabled = false;
                //}
            }
        }
    }

}