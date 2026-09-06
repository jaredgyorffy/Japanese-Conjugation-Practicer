using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class DungeonCrawler : MonoBehaviour
{
    private Camera m_Camera;
    [SerializeField] private float walkDuration;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private Vector2 rotationRemap;

    private Transform start;
    private Transform center;
    private Transform end;

    public event Action TileSequenceStart;
    public event Action TileSequenceComplete;

    public void Initialize(Transform start)
    {
        m_Camera = Camera.main;
        m_Camera.transform.position = start.position + positionOffset;
        m_Camera.transform.rotation = start.rotation;
        m_Camera.transform.localRotation = m_Camera.transform.localRotation * Quaternion.Euler(rotationOffset);
    }
    public void CrawlForwards(Transform start, Transform center, Transform end)
    {
        this.start = start;
        this.end = end;
        this.center = center;
        StartCoroutine("DungeonCrawl");
        TileSequenceStart?.Invoke();
    }

    IEnumerator DungeonCrawl()
    {
        float time = 0;
        float duration = walkDuration;
        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = time / walkDuration;
            float part1Alpha = Mathf.Clamp01(math.remap(0, 0.5f, 0, 1, alpha));
            float part2Alpha = Mathf.Clamp01(math.remap(0.5f, 1, 0, 1, alpha));
            m_Camera.transform.position = Vector3.Lerp(
                Vector3.Lerp(start.position, center.position, part1Alpha),
                Vector3.Lerp(center.position, end.position, part2Alpha), alpha) + positionOffset;

            float rotationAlpha = Mathf.Clamp01(math.remap(rotationRemap.x, rotationRemap.y, 0, 1, alpha));
            m_Camera.transform.rotation = Quaternion.Lerp(start.rotation, end.rotation, rotationAlpha);
            m_Camera.transform.localRotation = m_Camera.transform.localRotation * Quaternion.Euler(rotationOffset);
            yield return null;
        }
        TileSequenceComplete?.Invoke();
    }

}

public enum DungeonDirection
{
    None = 0,
    Left = 1,
    Forward = 2,
    Right = 3,
}