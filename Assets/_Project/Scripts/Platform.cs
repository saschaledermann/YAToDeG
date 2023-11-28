using UnityEngine;

public enum SocketState
{
    empty,
    blocked
}

public class Platform : MonoBehaviour
{
    SocketState m_socketState = SocketState.empty;
    public SocketState SocketState { get => m_socketState; private set => m_socketState = value; }
    Vector3 m_socketPosition;
    GameObject m_turret;


    void Awake()
    {
        m_socketPosition = transform.GetChild(1).transform.position;
    }

    public Vector3 GetSocketPosition() => m_socketPosition;

    public void PlaceTurret(GameObject turret)
    {
        if (turret == null || SocketState == SocketState.blocked) return;
        m_turret = turret;
        turret.transform.SetPositionAndRotation(m_socketPosition, transform.rotation);
        turret.transform.SetParent(transform);
        SocketState = SocketState.blocked;
    }
}
