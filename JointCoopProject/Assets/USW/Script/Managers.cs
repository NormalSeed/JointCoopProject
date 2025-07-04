using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
public class Managers : MonoBehaviour
{

    private static Managers s_instance; 
    private static Managers Instance { get { Init(); return s_instance; } }  // 유일한 매니저를 갖고옴



    private GameManager _game = new GameManager();

    public static GameManager Game { get { return Instance._game; } }





    private DataManager _data = new DataManager();
    private InputManager _input = new InputManager();
    private ResourceManager _resource = new ResourceManager();
    private SceneManagerEx _scene = new SceneManagerEx();
    private SoundManager _sound = new SoundManager();
    private UIManager _ui = new UIManager();

    public static DataManager Data { get { return Instance._data; } }
    public static InputManager Input { get { return Instance._input; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI { get { return Instance._ui; } }



    void Start()
    {
        Init();     // 싱글톤
    }

    void Update()
    {
        Input.OnUpdate();  
    }

    // 싱글톤 메소드
    static void Init()
    {
        if (s_instance == null){
            GameObject go = GameObject.Find("@Manager");// 오브젝트 찾기

            if (go == null){
                go = new GameObject{name = "@Manager"}; // 오브젝트 이름 설정
                go.AddComponent<Managers>();            // 컴포넌트 추가
                Debug.Log("@Manager 생성.");
            }

            DontDestroyOnLoad(go);                      
            s_instance = go.GetComponent<Managers>();
            
            // [ 프로퍼티 변수를 사용안한 이유 ] 
            // "Sound = Instance._sound"인데 Instance안에 Init() 호출 메소드가 있으므로 무한루프에 빠질 수 있다.
            // 그러므로 s_instance를 사용해서 접근한다.
 
            s_instance._sound.Init();
            s_instance._pool.Init();
            s_instance._data.Init();
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        Input.Clear();
        UI.Clear();
        Scene.Clear();
        // 보통 다른 매니저에서 사용할수있는것들은 마지막에 clear해줌 

    //  Data.Clear();  <- Data는 게임 중 항상 들고 있는 것이 맞기 때문에 Clear 하지 않는다.
    }
}

*/