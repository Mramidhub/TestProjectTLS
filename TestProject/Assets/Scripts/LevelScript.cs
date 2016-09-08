using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelScript : MonoBehaviour {

    public static bool OnTargetNPC;
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
    Vector2 StartPosition;
    Vector2 EndPosition;
    // Переменные для рамки выделения.


    void Start() {
        Unit = new List<GameObject>();
        UnitSelected = new List<GameObject>();

        OnTargetNPC = true;
    }

    void Update()
    {
        //Debug.Log(OnTargetNPC);
        StartCoroutine(OfTargets());
        SingleTarget();

    }

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
                    TargetMark(hit.transform);
                    //Если нажали на NPC или здание создаем таргет - метку.

                    Player.GetComponent<MovePlayer>().MinionTargetOn = true;
                    // Если нажали на NPC или здание управлевние мышкой снимаеться с игрока.
                }
                else if (hit.transform.tag == "Terrain")
                {
                    Player.GetComponent<MovePlayer>().MinionTargetOn = false;
                    OnTargetNPC = false;
                    // Если  попали в Terrain, то передаем NPC глобальную переменную, для уничтожение таргет - меток 
                    // И возвращаем управление героя мышкой.
                }
            }
        }


    }
    // Выбор одиночной цели.

    void TargetMark(Transform Targ)
    {
        TargetPosition = new Vector3(Targ.transform.position.x, 0.1f, Targ.transform.position.z);
        GameObject ChosenTarget = new GameObject();
        ChosenTarget = Instantiate(Target, TargetPosition, Quaternion.identity) as GameObject;
        ChosenTarget.transform.parent = Targ.transform;
        ChosenTarget.transform.Rotate(90f, 0f, 0f);

    }
    // Установка таргет-метки на NPC через добавление дочернего обьекта.

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
                TargetMark(UnitSelected[j].transform);
                Player.GetComponent<MovePlayer>().MinionTargetOn = true;

                // Добавляем таргет-метку.
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
                
               // UnitSelected[j].GetComponent<MoveMinion>().DestroyOnTarget();
                // Уничтожаем таргет метку
            }

        }

    }
    // Снимаем таргет-метку с выделенных обьектов.

    void OnGUI()
    {
        GUI.skin = Skin;
        GUI.depth = 99;

        if (Input.GetMouseButtonDown(0))
        {
            Deselect();
            StartPosition = Input.mousePosition;
            Draw = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Draw = false;
            Select();
            OnTargetNPC = true;
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

    IEnumerator OfTargets()
    {
        if (OnTargetNPC == false)
        {
            yield return new WaitForSeconds(1f);

            OnTargetNPC = true;
        }

    }
    // Возвращаем глоабльной переменной OnTargetNPC значение True.
}
