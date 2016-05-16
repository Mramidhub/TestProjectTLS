using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelScript : MonoBehaviour {

    public static bool EnemyOnMap = true;
    public static bool OnTargetNPC;
    public static int UnitIndex;
    public static bool  StartGame;
    public static int WaveLvl;
    public static float WaveTime;
    public GameObject Player;
    public GameObject Target;
    public Text StartGameTimer;
    public GameObject Sofa;
    public Text HPSofa;
    float StartTimer;
    float TimerFindEnemy;
    GameObject HitGObject;
        Vector3 TargetPosition;
    // Переменные для ситемы таргет-метки.

    GameObject SelctedConstr;


    public static List<GameObject> Unit;
    // Список всех NPC на сцене.
    public static List<GameObject> UnitSelected;
    // Список выделенных нпс на сцене.
    public GUISkin Skin;
    Rect Rect;
    bool Draw;
    bool SingleSelected;
    bool GUIConstrustion;
    bool ClickGUI;
    public static bool EndGameLoose;
    Vector2 StartPosition;
    Vector2 EndPosition;
    // Переменные для рамки выделения.


    void Start() {
        WaveLvl = 1;
        WaveTime = 60f;
        StartTimer = 10f;
        SelctedConstr = new GameObject();
        Unit = new List<GameObject>();
        UnitSelected = new List<GameObject>();
        UnitIndex = 0;
        TimerFindEnemy = 2f;
        OnTargetNPC = true;
        EndGameLoose = false;
        SingleSelected = false;
        StartCoroutine(GameStart());
     
    }

    void Update()
    {
        HPSofa.text = "HP: " + (int)Sofa.GetComponent<Sofa>().Health;
        SingleTarget();
        EnemysOnMap();
        StartTime();
    }

    void NextWave()
    {
        WaveTime -= Time.deltaTime;
        if (WaveTime < 0)
        {
            WaveLvl++;
            WaveTime = 40f;
            WaveTime*= WaveLvl;
            StartCoroutine(GameStart());
        }

    }
    // Повышение сложности волны монстров.

    void EnemysOnMap()
    {
        TimerFindEnemy -= Time.deltaTime;
        if (TimerFindEnemy < 0)
        {
            if (GameObject.FindGameObjectWithTag("Enemy"))
            {
                EnemyOnMap = true;
                TimerFindEnemy = 2f;
            }
            else
            {
                EnemyOnMap = false;
                TimerFindEnemy = 2f;
            }
        }

    }
    // Проверяем есть ли враги на карте.

    void SingleTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.tag == "Minion" || hit.transform.tag == "Player")
                {
                    SingleSelected = true;
                    UnitSelected.Add(hit.transform.gameObject);
                    Select();
                    Draw = false;
                }
                else if (hit.transform.tag == "Terrain" && hit.transform.tag != "GUI")
                {
                    Deselect();
                    ConstructDeselect();
                    // Если попали в Terrain отключаем таргет метку или отключаем GUI здания.

                }
                else if (hit.transform.tag == "Barracks")
                {
                    SelctedConstr = hit.transform.gameObject;
                    SelctedConstr.GetComponent<RespawnMinion>().GUIConstruction.enabled = true;
                    GUIConstrustion = true;
                }
                else if (hit.transform.tag == "Enemy")
                {
                    Player.GetComponent<MovePlayer>().Target = hit.transform.gameObject;
                    Player.GetComponent<MovePlayer>().MinionTargetOn = true;
                    hit.transform.GetComponent<MoveEnemy>().TargetMark.GetComponent<Renderer>().enabled = true;
                }

            }
        }


    }
    // Выбор одиночной цели.

    bool ChekUnit(GameObject unit)
    {
        bool result = false;
        foreach (GameObject u in UnitSelected)
        {
            if (u == unit) result = true;
        }
        return result;
    }
    // Проверка добавлен ли обьект в UnitSelected или нет.

    void Select()
    {
            if (UnitSelected.Count > 0)
            {
                for (int j = 0; j < UnitSelected.Count; j++)
                {
                    UnitSelected[j].transform.Find("TargetMark").GetComponent<Renderer>().enabled = true;
                }

            }
    }
    // Добавляем таргет-метку выделенныи обьектам, перебирая коллецию.

    void Deselect()
    {
            if (UnitSelected.Count > 0)
            {
                for (int j = 0; j < UnitSelected.Count; j++)
                {
                    UnitSelected[j].transform.Find("TargetMark").GetComponent<Renderer>().enabled = false;

                }
            }
    }
    // Снимаем таргет-метку с выделенных обьектов.

    void ConstructDeselect()
    {
        if (GUIConstrustion == true)
        {
            if (SelctedConstr.GetComponent<RespawnMinion>().GUIConstruction.enabled == true)
            {
                SelctedConstr.GetComponent<RespawnMinion>().GUIConstruction.enabled = false;
            }

            GUIConstrustion = false;
        }


    }

    void OnGUI()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            GUI.skin = Skin;
            GUI.depth = 99;

            if (Input.GetMouseButtonDown(0) && SingleSelected == false && Player.GetComponent<MovePlayer>().SkillUsed == false)
            {
                HitGObject = hit.transform.gameObject;
                if(HitGObject.transform.tag != "GUI")
                { 
                    OnTargetNPC = false;
                    Deselect();
                    StartPosition = Input.mousePosition;
                    Draw = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                Draw = false;
                if (SingleSelected == false)
                {
                    Select();
                }
                // Отключаем единичный выбор при нажатии на terrain. 
                OnTargetNPC = true;
                SingleSelected = false;
            }

            if (Draw)
            {
                UnitSelected.Clear();
                EndPosition = Input.mousePosition;
                if (StartPosition == EndPosition)
                {
                    return;
                }

                Rect = new Rect(Mathf.Min(EndPosition.x, StartPosition.x),
                    Screen.height - Mathf.Max(EndPosition.y, StartPosition.y),
                    Mathf.Max(EndPosition.x, StartPosition.x) - Mathf.Min(EndPosition.x, StartPosition.x),
                    Mathf.Max(EndPosition.y, StartPosition.y) - Mathf.Min(EndPosition.y, StartPosition.y)
                    );

                GUI.Box(Rect, "");

                for (int j = 0; j < Unit.Count; j++)
                {
                    Vector2 Tmp = new Vector2(Camera.main.WorldToScreenPoint(Unit[j].transform.position).x, Screen.height - Camera.main.WorldToScreenPoint(Unit[j].transform.position).y);
                    // Трансформируем позицию объекта из мирового пространства, в пространство экрана.
                    if (Rect.Contains(Tmp))
                    {
                        if (UnitSelected.Count == 0)
                        {
                            UnitSelected.Add(Unit[j]);
                        }
                        else if (!ChekUnit(Unit[j]))
                        {
                            UnitSelected.Add(Unit[j]);
                        }

                    }
                }
            }
        }
    }
    // Рисуем рамку, добавляем обьекты в коллецию.

    void StartTime()    
    {
        if (StartTimer > 0)
        {
            StartTimer -= Time.deltaTime;
            StartGameTimer.GetComponent<Text>().text = "До начала следующей волны: " + (int)StartTimer;
        }
        else
        {
            StartGameTimer.GetComponent<Text>().enabled = false;
        }
    }

    IEnumerator GameStart()
    {
        LevelScript.StartGame = false;

        yield return new WaitForSeconds(10f);


        LevelScript.StartGame = true;

    }

}
