using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretSpawnInterface : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] GameObject m_turretPrefab;
    [SerializeField] Material m_previewMaterial;
    GameObject m_objectRef;
    GameObject m_objectPreview;

    public void CreateTurret()
    {
        CreateItemForButton();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CreateItemForButton();
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateTurretPosition();
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        PlaceTurret();
    }

    private void UpdateTurretPosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (m_objectRef != null && Physics.Raycast(ray, out var hit))
        {
            m_objectRef.transform.position = hit.point;
            if (hit.transform.gameObject.TryGetComponent<Platform>(out var platform) &&
                platform.SocketState == SocketState.empty &&
                m_objectPreview == null)
            {
                CreatePreview(platform);
            }
            else if (!hit.transform.gameObject.TryGetComponent<Platform>(out var _) &&
                    m_objectPreview != null)
            {
                Destroy(m_objectPreview);
                m_objectPreview = null;
            }
        }
    }

    private void CreatePreview(Platform platform)
    {
        m_objectPreview = Instantiate(m_turretPrefab, platform.GetSocketPosition(), platform.transform.rotation);
        m_objectPreview.GetComponentInChildren<Collider>().enabled = false;
        var meshRenderers = m_objectPreview.transform.GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in meshRenderers)
        {
            renderer.material = m_previewMaterial;
        }
    }

    private void PlaceTurret()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (m_objectRef != null && Physics.Raycast(ray, out var hit))
        {
            if (hit.transform.gameObject.TryGetComponent<Platform>(out var platform) && platform.SocketState == SocketState.empty)
            {
                platform.PlaceTurret(m_objectRef);
                m_objectRef.GetComponentInChildren<Collider>().enabled = true;
                var colliders = m_objectRef.GetComponents<Collider>();
                foreach (var collider in colliders)
                {
                    collider.enabled = true;
                }
            }
            else
            {
                Destroy(m_objectRef);
            }
            if (m_objectPreview != null)
            {
                Destroy(m_objectPreview);
                m_objectPreview = null;
            }
        }
        m_objectRef = null;
    }

    void CreateItemForButton()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (m_turretPrefab != null && Physics.Raycast(ray, out var hit))
        {
            m_objectRef = Instantiate(m_turretPrefab, hit.point, Quaternion.identity);
            m_objectRef.GetComponentInChildren<Collider>().enabled = false;
            var colliders = m_objectRef.GetComponents<Collider>();
            foreach (var collider in colliders)
            {
                collider.enabled = false;
            }
        }
    }
}
