using UnityEngine;

public class TurretSpawner : MonoBehaviour
{
    public GameObject turretPrefab;
    public Material previewMaterial;
    GameObject m_objectRef;
    GameObject m_objectPreview;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (turretPrefab != null && Physics.Raycast(ray, out var hit))
            {
                m_objectRef = Instantiate(turretPrefab, hit.point, Quaternion.identity);
                m_objectRef.GetComponentInChildren<Collider>().enabled = false;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (m_objectRef != null && Physics.Raycast(ray, out var hit))
            {
                m_objectRef.transform.position = hit.point;
                if (hit.transform.gameObject.TryGetComponent<Platform>(out var platform) && 
                    platform.SocketState == SocketState.empty &&
                    m_objectPreview == null)
                {
                    m_objectPreview = Instantiate(turretPrefab, platform.GetSocketPosition(), platform.transform.rotation);
                    m_objectPreview.GetComponentInChildren<Collider>().enabled = false;
                    m_objectPreview.transform.GetChild(0).GetComponent<MeshRenderer>().material = previewMaterial;
                }
                else if (!hit.transform.gameObject.TryGetComponent<Platform>(out var _) &&
                        m_objectPreview != null)
                {
                    Destroy(m_objectPreview);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (m_objectRef != null && Physics.Raycast(ray, out var hit))
            {
                if (hit.transform.gameObject.TryGetComponent<Platform>(out var platform) && platform.SocketState == SocketState.empty)
                {
                    platform.PlaceTurret(m_objectRef);
                }
                else
                {
                    Destroy(m_objectRef);
                }
                if (m_objectPreview != null)
                    Destroy(m_objectPreview);
            }
        }
        else
        {
            m_objectRef = null;
        }
    }
}
