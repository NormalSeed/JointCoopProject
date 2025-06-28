using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapRoom : MonoBehaviour
{
    // 방 정보
    private Vector2Int pos;                    // 방 위치
    private MapGenerator.RoomData data;        // 방 데이터
    private Image img;                         // 이미지 컴포넌트
    private Sprite defaultSprite;              // 기본 스프라이트
    
    // 상태 변수들
    private bool visited = false;              // 방문했는지 여부
    private bool visible = false;              // 보이는지 여부
    private bool current = false;              // 현재 방인지 여부
    
    // 표시 관련
    private Vector3 defaultScale;              // 기본 크기
    private Color defaultColor;                // 기본 색상
    
    // 애니메이션 관련
    private Coroutine pulseCoroutine;          // 펄스 애니메이션 코루틴
    private bool shouldPulse = false;          // 펄스 해야 하는지 여부
    
    void Awake()
    {
        defaultScale = Vector3.one;
    }
    
    public void Setup(Vector2Int position, MapGenerator.RoomData roomData, Sprite icon)
    {
        pos = position;
        data = roomData;
        defaultSprite = icon;
        
        img = GetComponent<Image>();
        if (img == null)
        {
            img = gameObject.AddComponent<Image>();
        }
        
        img.sprite = icon;
        defaultColor = img.color;
        
        // 시작방은 기본적으로 방문한 상태
        if (data.roomType == MapGenerator.RoomType.Start)
        {
            SetVisited(true);
            SetVisible(true);
        }
        else
        {
            SetVisible(false);
        }
        
        gameObject.name = $"Room_{data.roomType}_{pos.x}_{pos.y}";
    }
    
    public void SetVisited(bool isVisited)
    {
        visited = isVisited;
        UpdateAppearance();
    }
    
    public void SetVisible(bool isVisible)
    {
        visible = isVisible;
        gameObject.SetActive(isVisible);
        
        if (isVisible)
        {
            UpdateAppearance();
        }
    }
    
    public void SetCurrent(bool isCurrent)
    {
        current = isCurrent;
        shouldPulse = isCurrent;
        UpdateAppearance();
        
        if (isCurrent)
        {
            StartPulse();       // 펄스 애니메이션 시작
        }
        else
        {
            StopPulse();        // 펄스 애니메이션 중지
        }
    }
    
    void UpdateAppearance()
    {
        if (!img) return;
        
        MinimapManager minimap = FindObjectOfType<MinimapManager>();
        if (minimap == null) return;
        
        if (current)
        {
            // 현재 방 - 노란색으로 표시하고 크기 키우기
            img.color = minimap.playerColor;
            transform.localScale = defaultScale * 1.2f;
        }
        else if (visited)
        {
            // 방문한 방 - 흰색으로 표시
            img.color = minimap.visitedColor;
            transform.localScale = defaultScale;
        }
        else if (visible)
        {
            // 보이지만 방문하지 않은 방 - 회색으로 표시하고 크기 줄이기
            img.color = minimap.unvisitedColor;
            transform.localScale = defaultScale * 0.9f;
        }
        
        // 특수방들은 방문했을 때 색상 변경
        if (visited)
        {
            if (data.roomType == MapGenerator.RoomType.Boss)
            {
                img.color = Color.Lerp(img.color, Color.red, 0.3f);        // 보스방은 빨간색 섞기
            }
            else if (data.roomType == MapGenerator.RoomType.Shop)
            {
                img.color = Color.Lerp(img.color, Color.blue, 0.3f);       // 상점방은 파란색 섞기
            }
            else if (data.roomType == MapGenerator.RoomType.Item)
            {
                img.color = Color.Lerp(img.color, Color.cyan, 0.3f);       // 아이템방은 청록색 섞기
            }
            else if (data.roomType == MapGenerator.RoomType.Secret)
            {
                img.color = Color.Lerp(img.color, Color.magenta, 0.3f);    // 비밀방은 자주색 섞기
            }
        }
    }
    
    void StartPulse()
    {
        StopPulse(); // 기존 펄스 중지
        if (shouldPulse)
        {
            pulseCoroutine = StartCoroutine(DoPulse());
        }
    }
    
    void StopPulse()
    {
        if (pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
            pulseCoroutine = null;
        }
        
        if (current)
        {
            transform.localScale = defaultScale * 1.2f;
        }
    }
    
    System.Collections.IEnumerator DoPulse()
    {
        float time = 0f;
        float speed = 2f;                               // 펄스 속도
        Vector3 bigScale = defaultScale * 1.3f;         // 큰 크기
        Vector3 normalScale = defaultScale * 1.2f;      // 기본 크기
        
        while (shouldPulse && current)
        {
            time += Time.deltaTime * speed;
            float t = (Mathf.Sin(time) + 1f) * 0.5f;    // 0~1 사이 값
            transform.localScale = Vector3.Lerp(normalScale, bigScale, t);
            yield return null;
        }
        
        // 펄스 끝나면 크기 원복
        if (current)
        {
            transform.localScale = normalScale;
        }
    }
    
    // 게터 함수들
    public Vector2Int GetPosition() { return pos; }
    public MapGenerator.RoomData GetRoomData() { return data; }
    public bool IsVisited() { return visited; }
    public bool IsVisible() { return visible; }
    public bool IsCurrent() { return current; }
}