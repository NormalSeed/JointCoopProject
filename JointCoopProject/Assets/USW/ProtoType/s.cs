using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor; // Editor 기능을 사용하기 위해 필요

// 이 스크립트가 Tilemap 컴포넌트의 Inspector에 추가적인 기능을 제공하도록 설정
[CustomEditor(typeof(Tilemap))]
public class TilemapBoundsCompressor : Editor
{
    // 기본 Inspector UI를 그립니다.
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // 기존 Tilemap Inspector를 그립니다.

        Tilemap tilemap = (Tilemap)target; // 현재 선택된 Tilemap 컴포넌트를 가져옵니다.

        // "Compress Bounds" 버튼을 그립니다.
        if (GUILayout.Button("Compress Bounds"))
        {
            // 현재 Tilemap의 Bounds를 압축합니다.
            tilemap.CompressBounds();

            // 씬이 변경되었음을 Unity에 알려 저장할 수 있도록 합니다.
            EditorUtility.SetDirty(tilemap);
            Debug.Log($"Tilemap '{tilemap.name}' bounds compressed.");
        }

        // 추가적으로 모든 타일을 지우는 버튼도 유용할 수 있습니다.
        if (GUILayout.Button("Clear All Tiles and Compress Bounds"))
        {
            if (EditorUtility.DisplayDialog("Clear Tilemap?",
                    "Are you sure you want to clear ALL tiles from this Tilemap and compress its bounds? This action cannot be undone.",
                    "Yes", "No"))
            {
                tilemap.ClearAllTiles(); // 모든 타일 지우기
                tilemap.CompressBounds(); // 지운 후 바운스 압축
                EditorUtility.SetDirty(tilemap);
                Debug.Log($"Tilemap '{tilemap.name}' cleared and bounds compressed.");
            }
        }
    }
}