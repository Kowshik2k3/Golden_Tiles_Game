using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[DisallowMultipleComponent]
public class MusicListPopulator : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public GameObject songEntryPrefab;
    public ScrollRect scrollRect;

    [Header("Layout (pixels)")]
    public float spacing = 8f;
    public float topPadding = 10f;
    public float bottomPadding = 10f;
    public float leftPadding = 12f;
    public float rightPadding = 12f;

    RectTransform contentParent;

    void Awake()
    {
        if (scrollRect == null)
            scrollRect = GetComponentInParent<ScrollRect>();

        if (scrollRect == null)
        {
            Debug.LogError("[MusicListPopulator] No ScrollRect found. Please add one or assign in inspector.");
            return;
        }

        EnsureScrollViewSetup();
        contentParent = scrollRect.content;
    }

    IEnumerator Start()
    {
        yield return null; // let other startup code run
        Populate();
    }

    void EnsureScrollViewSetup()
    {
        // Ensure viewport
        if (scrollRect.viewport == null)
        {
            Transform fp = scrollRect.transform.Find("Viewport");
            if (fp != null) scrollRect.viewport = fp.GetComponent<RectTransform>();
        }

        // Ensure content
        if (scrollRect.content == null)
        {
            Transform fc = (scrollRect.viewport != null) ? scrollRect.viewport.Find("Content") : null;
            if (fc != null) scrollRect.content = fc.GetComponent<RectTransform>();
            else
            {
                // create Content under viewport
                if (scrollRect.viewport != null)
                {
                    GameObject contentGO = new GameObject("Content", typeof(RectTransform));
                    contentGO.transform.SetParent(scrollRect.viewport, false);
                    RectTransform rt = contentGO.GetComponent<RectTransform>();
                    rt.anchorMin = new Vector2(0f, 1f);
                    rt.anchorMax = new Vector2(1f, 1f);
                    rt.pivot = new Vector2(0.5f, 1f);
                    rt.anchoredPosition = Vector2.zero;
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x, 0f);
                    scrollRect.content = rt;
                    Debug.Log("[MusicListPopulator] Created Content under Viewport.");
                }
            }
        }

        // Ensure vertical scrollbar exists (optional)
        if (scrollRect.verticalScrollbar == null)
        {
            Transform vs = scrollRect.transform.Find("Scrollbar Vertical");
            if (vs == null) vs = scrollRect.transform.Find("Scrollbar");
            if (vs != null)
            {
                var sb = vs.GetComponent<Scrollbar>();
                if (sb != null) scrollRect.verticalScrollbar = sb;
            }
        }
    }

    public void Populate()
    {
        if (scrollRect == null || songEntryPrefab == null)
        {
            Debug.LogWarning("[MusicListPopulator] Missing assignment (scrollRect/songEntryPrefab).");
            return;
        }

        contentParent = scrollRect.content;
        if (contentParent == null)
        {
            Debug.LogError("[MusicListPopulator] ScrollRect.content is null. Ensure the ScrollView is set up correctly.");
            return;
        }

        var mgr = MusicManager.Instance;
        if (mgr == null || mgr.musicClips == null || mgr.musicClips.Length == 0)
        {
            Debug.LogWarning("[MusicListPopulator] MusicManager or musicClips missing/empty.");
            return;
        }

        int count = mgr.musicClips.Length;

        // Clear existing children (safe)
        for (int i = contentParent.childCount - 1; i >= 0; i--)
            Destroy(contentParent.GetChild(i).gameObject);

        // Determine prefab height
        float prefabPreferredHeight = -1f;
        var prefabLE = songEntryPrefab.GetComponent<LayoutElement>();
        if (prefabLE != null && prefabLE.preferredHeight > 0.001f)
            prefabPreferredHeight = prefabLE.preferredHeight;
        else
        {
            var prefabRT = songEntryPrefab.GetComponent<RectTransform>();
            if (prefabRT != null && prefabRT.sizeDelta.y > 0.001f)
                prefabPreferredHeight = prefabRT.sizeDelta.y;
        }
        if (prefabPreferredHeight <= 0f) prefabPreferredHeight = 56f;

        // Configure content for top-down layout
        contentParent.pivot = new Vector2(0.5f, 1f);
        contentParent.anchorMin = new Vector2(0f, 1f);
        contentParent.anchorMax = new Vector2(1f, 1f);

        float totalHeight = topPadding + bottomPadding + count * prefabPreferredHeight + Mathf.Max(0, (count - 1)) * spacing;
        Vector2 cd = contentParent.sizeDelta;
        cd.y = totalHeight;
        contentParent.sizeDelta = cd;

        float currentY = -topPadding;

        for (int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(songEntryPrefab, contentParent, false);
            go.transform.localScale = Vector3.one;

            // fix RectTransform so it stretches full width with padding
            RectTransform rt = go.GetComponent<RectTransform>();
            if (rt == null) rt = go.AddComponent<RectTransform>();

            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(1f, 1f);
            rt.pivot = new Vector2(0.5f, 1f);

            // zero out offsets first to avoid carrying odd prefab offsets
            rt.offsetMin = new Vector2(0f, rt.offsetMin.y);
            rt.offsetMax = new Vector2(0f, rt.offsetMax.y);

            // now apply left/right padding correctly
            rt.offsetMin = new Vector2(leftPadding, rt.offsetMin.y);
            rt.offsetMax = new Vector2(-rightPadding, rt.offsetMax.y);

            // ensure width stretches: set sizeDelta.x = 0 (allows anchors & offsets to define width)
            rt.sizeDelta = new Vector2(0f, prefabPreferredHeight);
            rt.anchoredPosition = new Vector2(0f, currentY);

            currentY -= (prefabPreferredHeight + spacing);

            // restore / ensure Image tint is visible (some prefabs had transparent or modified tint)
            var img = go.GetComponent<Image>();
            if (img != null)
            {
                // if alpha is near 0, set to fully visible white; otherwise keep existing color but ensure alpha not zero
                if (img.color.a < 0.01f) img.color = Color.white;
            }

            // setup text and button
            SongEntry se = go.GetComponent<SongEntry>();
            string nameToShow = mgr.musicClips[i] != null ? mgr.musicClips[i].name : $"Track {i + 1}";

            if (se != null)
            {
                se.Setup(i, nameToShow);
            }
            else
            {
                var txt = go.GetComponentInChildren<TextMeshProUGUI>();
                if (txt != null)
                {
                    txt.enableWordWrapping = true;
                    txt.alignment = TextAlignmentOptions.MidlineLeft; // middle-left alignment
                    txt.text = nameToShow;
                }

                var btn = go.GetComponent<Button>();
                if (btn != null)
                {
                    int idx = i;
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => MusicManager.Instance?.SelectMusic(idx));
                }
            }
        }

        // Force update and rebind
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentParent);

        if (scrollRect.content != contentParent) scrollRect.content = contentParent;
        if (scrollRect.viewport == null)
        {
            Transform vp = scrollRect.transform.Find("Viewport");
            if (vp != null) scrollRect.viewport = vp.GetComponent<RectTransform>();
        }

        Debug.Log("[MusicListPopulator] Populate complete. Items: " + count);
    }
}
