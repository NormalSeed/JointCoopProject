
using UnityEngine;
using UnityEngine.UI;

public class MinimapRoom : MonoBehaviour
{
    private Vector2Int gridPosition;
    private MapGenerator.RoomData roomData; 
    private Image roomImage;
    private Sprite originalSprite;
    private bool isExplored = false;
    private bool isVisible = false;
    private bool isCurrent = false;
    
    // 애니메이션용
    private Vector3 originalScale;
    private Color originalColor;
    
    void Awake()
    {
        originalScale = Vector3.one;
    }
    
    public void Initialize(Vector2Int pos, MapGenerator.RoomData data, Sprite icon)
    {
        gridPosition = pos;
        roomData = data;
        originalSprite = icon;
        
        roomImage = GetComponent<Image>();
        if (roomImage == null)
        {
            roomImage = gameObject.AddComponent<Image>();
        }
        
        roomImage.sprite = icon;
        originalColor = roomImage.color;
        
        // 시작방은 처음부터 탐험된 상태
        if (roomData.roomType == MapGenerator.RoomType.Start)
        {
            SetExplored(true);
            SetVisible(true);
        }
        else
        {
            SetVisible(false);
        }
        
        // 룸 아이콘 이름 설정
        gameObject.name = $"MinimapRoom_{roomData.roomType}_{pos.x}_{pos.y}";
    }
    
    public void SetExplored(bool explored)
    {
        isExplored = explored;
        UpdateVisual();
    }
    
    public void SetVisible(bool visible)
    {
        isVisible = visible;
        gameObject.SetActive(visible);
        
        if (visible)
        {
            UpdateVisual();
        }
    }
    
    public void SetAsCurrent(bool current)
    {
        isCurrent = current;
        UpdateVisual();
        
        if (current)
        {
            // 현재 방 애니메이션 효과
            StartCoroutine(PulseAnimation());
        }
    }
    
    void UpdateVisual()
    {
        if (!roomImage) return;
        
        MinimapManager minimap = FindObjectOfType<MinimapManager>();
        if (minimap == null) return;
        
        if (isCurrent)
        {
            // 현재 방 표시 ( 조금더 스케일 키움 ) 
            roomImage.color = minimap.currentRoomColor;
            transform.localScale = originalScale * 1.2f;
        }
        else if (isExplored)
        {
            // 탐험된 방
            roomImage.color = minimap.exploredColor;
            transform.localScale = originalScale;
        }
        else if (isVisible)
        {
            // 보이지만 탐험되지 않은 방 
            roomImage.color = minimap.unexploredColor;
            transform.localScale = originalScale * 0.9f;
        }
        
        // 특수방 표시 강화
        if (isExplored)
        {
            switch (roomData.roomType)
            {
                case MapGenerator.RoomType.Boss:
                    roomImage.color = Color.Lerp(roomImage.color, Color.red, 0.3f);
                    break;
                case MapGenerator.RoomType.Shop:
                    roomImage.color = Color.Lerp(roomImage.color, Color.blue, 0.3f);
                    break;
                case MapGenerator.RoomType.Item:
                    roomImage.color = Color.Lerp(roomImage.color, Color.cyan, 0.3f);
                    break;
                case MapGenerator.RoomType.Secret:
                    roomImage.color = Color.Lerp(roomImage.color, Color.magenta, 0.3f);
                    break;
            }
        }
    }
    
    System.Collections.IEnumerator PulseAnimation()
    {
        float time = 0f;
        float duration = 1f;
        Vector3 targetScale = originalScale * 1.3f;
        
        while (isCurrent && time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.PingPong(time * 2f, 1f);
            transform.localScale = Vector3.Lerp(originalScale * 1.2f, targetScale, t);
            yield return null;
        }
        
        if (isCurrent)
        {
            transform.localScale = originalScale * 1.2f;
        }
    }
    public Vector2Int GetGridPosition() => gridPosition;
    public MapGenerator.RoomData GetRoomData() => roomData;
    public bool IsExplored() => isExplored;
    public bool IsVisible() => isVisible;
    public bool IsCurrent() => isCurrent;
}