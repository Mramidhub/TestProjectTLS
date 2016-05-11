using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelScript : MonoBehaviour {

    public static bool OnTargetNPC;
    public static int UnitIndex;
    public GameObject Player;
    public GameObject Target;
    Vector3 TargetPosition;
    // Переменные для ситемы таргет-метки.




    public static List<GameObject> Unit;
    // Список всех NPC на сцене.
    public static List<GameObject> UnitSelected;
    // Список выделенных нпс на сцене.
    public GUISkin Skin;
    Rect Rect;
    bool Draw;
    bool SingleSelected;
    public static bool EndGameLoose;
    Vector2 StartPosition;
    Vector2 EndPosition;
    // Переменные для рамки выделения.


    void Start() {
        Unit = new List<GameObject>();
        UnitSelected = new List<GameObject>();
        UnitIndex = 0;
        OnTargetNPC = true;
        EndGameLoose = false;
        SingleSelected = false;
    }

    void Update()
    {

        SingleTarget();
    }

    void SingleTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.Log("1");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log("2");
                if (hit.transform.tag == "Minion" || hit.transform.tag == "Player")
                {
                    SingleSelected = true;
                    Debug.Log("3");
                    UnitSelected.Add(hit.transform.gameObject);
                    Select();
                    Draw = false;
                }
                else if (hit.transform.tag == "Terrain")
                {
                    Debug.Log("7");
                    Deselect();
                    // Если попали в Terrain отключаем таргет метку.

                }
                else if (hit.transform.tag == "Barracks")
                {
                    hit.transform.GetComponent<RespawnWarrior>().GUIBarracks.enabled = true;

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
        Debug.Log("4");
        if (UnitSelected.Count > 0)
        {
            Debug.Log("5");
            for (int j = 0; j < UnitSelected.Count; j++)
            {
                Debug.Log("6");
                UnitSelected[j].transform.Find("TargetMark").GetComponent<Renderer>().enabled = true;
            }

        } 

    }
    // Добавляем таргет-метку выделенныи обьектам, перебирая коллецию.

    void Deselect()
    {
        Debug.Log("9");
        if (UnitSelected.Count > 0)
        {
            Debug.Log("10");
            for (int j = 0; j < UnitSelected.Count; j++)
            {
                UnitSelected[j].transform.Find("TargetMark").GetComponent<Renderer>().enabled = false;
                Debug.Log("11");
                
            }
        }

    }
    // Снимаем таргет-метку с выделенных обьектов.

    void OnGUI()
    {
        GUI.skin = Skin;
        GUI.depth = 99;

        if (Input.GetMouseButtonDown(0) && SingleSelected == false)
        {
            OnTargetNPC = false;
            Deselect();
            StartPosition = Input.mousePosition;
            Draw = true;
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
    // Рисуем рамку, добавляем обьекты в коллецию.

}
