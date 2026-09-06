using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    public Transform Target { get { return target; } set { target = value; } }
    [Space]
    [Header("IgnorePosition")]
    [SerializeField] private bool ignorePositionX;
    [SerializeField] private bool ignorePositionY;
    [SerializeField] private bool ignorePositionZ;
    [Space]
    [Header("IgnoreRotation")]
    [SerializeField] private bool ignoreRotationX;
    [SerializeField] private bool ignoreRotationY;
    [SerializeField] private bool ignoreRotationZ;
    [Space]
    [SerializeField] private bool useLocalRotation;
    [SerializeField] private bool inheritLocalScale;

    public void SetPositionIgnoreFlags(bool ignoreX, bool ignoreY, bool ignoreZ)
    {
        this.ignorePositionX = ignoreX;
        this.ignorePositionY = ignoreY;
        this.ignorePositionZ = ignoreZ;
    }
    public void SetRotationIgnoreFlags(bool ignoreX, bool ignoreY, bool ignoreZ)
    {
        this.ignoreRotationX = ignoreX;
        this.ignoreRotationY = ignoreY;
        this.ignoreRotationZ = ignoreZ;
    }

    private void Update()
    {
        FollowPosition();
        FollowRotation();
        UpdateScale();
    }

    private void FollowPosition()
    {
        if (target == null)
        {
            return;
        }

        Vector3 initialPosition = transform.position;
        Vector3 targetPosition;

        targetPosition = target.position;

        float x = initialPosition.x;
        if (ignorePositionX == false)
        {
            x = targetPosition.x;
        }

        float y = initialPosition.y;
        if (ignorePositionY == false)
        {
            y = targetPosition.y;
        }

        float z = initialPosition.z;
        if (ignorePositionZ == false)
        {
            z = targetPosition.z;
        }

        transform.position = new Vector3(x, y, z);
    }

    private void FollowRotation()
    {
        if (target == null)
        {
            return;
        }

        Vector3 initialRotationEuler = transform.rotation.eulerAngles;
        Vector3 targetRotationEuler = target.rotation.eulerAngles;

        if (useLocalRotation)
        {
            initialRotationEuler = transform.localEulerAngles;
            targetRotationEuler = target.localEulerAngles;
        }

        float x = initialRotationEuler.x;
        if (ignoreRotationX == false)
        {
            x = targetRotationEuler.x;
        }

        float y = initialRotationEuler.y;
        if (ignoreRotationY == false)
        {
            y = targetRotationEuler.y;
        }

        float z = initialRotationEuler.z;
        if (ignoreRotationZ == false)
        {
            z = targetRotationEuler.z;
        }
        if (useLocalRotation)
        {
            transform.localEulerAngles = new Vector3(x, y, z);
        }
        else
        {
            transform.rotation = Quaternion.Euler(x, y, z);
        }
    }
    private void UpdateScale()
    {
        if (inheritLocalScale)
        {
            transform.localScale = target.localScale;
        }
    }
}
