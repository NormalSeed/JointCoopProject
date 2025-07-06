using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MinimapRoom : MonoBehaviour
{
    private Vector2Int gridPosition;
    private MapGenerator.RoomData roomData; 
    private Image roomImage;
    private bool isExplored = false;
    private bool isVisible = false;
    private bool isCurrent = false;
    
    private Vector3 originalScale = Vector3.one;
    
    public void Initialize(Vector2Int pos, MapGenerator.RoomData data, Sprite icon)
    {
        gridPosition = pos;
        roomData = data;
        
        roomImage = GetComponent<Image>();
        roomImage.sprite = icon;
        
        if (roomData.roomType == MapGenerator.RoomType.Start)
        {
            SetExplored(true);
            SetVisible(true);
        }
        else
        {
            SetVisible(false);
        }
        
        gameObject.name = "미니맵방_" + roomData.roomType + "_" + pos.x + "_" + pos.y;
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
        
        if (visible) UpdateVisual();
    }
    
    public void SetAsCurrent(bool current)
    {
        isCurrent = current;
        UpdateVisual();
        
        if (current)
        {
            StartCoroutine(PulseEffect());
        }
    }
    
    void UpdateVisual()
    {
        MinimapManager minimap = FindObjectOfType<MinimapManager>();
        
        if (isCurrent)
        {
            roomImage.color = minimap.currentRoomColor;
            transform.localScale = originalScale * 1.2f;
        }
        else if (isExplored)
        {
            roomImage.color = minimap.exploredColor;
            transform.localScale = originalScale;
            ApplySpecialRoomColor();
        }
        else if (isVisible)
        {
            roomImage.color = minimap.unexploredColor;
            transform.localScale = originalScale * 0.9f;
        }
    }
    
    void ApplySpecialRoomColor()
    {
        Color specialColor = Color.white;
        
        switch (roomData.roomType)
        {
            case MapGenerator.RoomType.Boss: specialColor = Color.red; break;
            case MapGenerator.RoomType.Shop: specialColor = Color.blue; break;
            case MapGenerator.RoomType.Item: specialColor = Color.cyan; break;
            case MapGenerator.RoomType.Secret: specialColor = Color.magenta; break;
            default: return;
        }
        
        roomImage.color = Color.Lerp(roomImage.color, specialColor, 0.3f);
    }
    
    IEnumerator PulseEffect()
    {
        float time = 0f;
        Vector3 targetScale = originalScale * 1.3f;
        
        while (isCurrent && time < 1f)
        {
            time += Time.deltaTime;
            float pulse = Mathf.PingPong(time * 2f, 1f);
            transform.localScale = Vector3.Lerp(originalScale * 1.2f, targetScale, pulse);
            yield return null;
        }
        
        if (isCurrent)
        {
            transform.localScale = originalScale * 1.2f;
        }
    }
    
    public Vector2Int GetGridPosition() { return gridPosition; }
    public MapGenerator.RoomData GetRoomData() { return roomData; }
    public bool IsExplored() { return isExplored; }
    public bool IsVisible() { return isVisible; }
    public bool IsCurrent() { return isCurrent; }
}