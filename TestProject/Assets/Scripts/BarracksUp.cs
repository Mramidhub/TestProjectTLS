using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BarracksUp : MonoBehaviour {


    public int PriceUpLvl;

    public float SpeedTrainigLvl2;
    public float AddArmor2Lvl;
    public float AddPAtck2Lvl;
    public float AddSpeed21Lvl;
    public float AddAtkSpd2Lvl;
    public float AddHP2Lvl;
    public int PriceUpLvl2;

    public float SpeedTrainigLvl3;
    public float AddArmor3Lvl;
    public float AddPAtck3Lvl;
    public float AddSpeed3Lvl;
    public float AddAtkSpd3Lvl;
    public float AddHP3Lvl;
    public int PriceUpLvl3;

    public float SpeedTrainigLvl4;
    public float AddArmor4Lvl;
    public float AddPAtck4Lvl;
    public float AddSpeed4Lvl;
    public float AddAtkSpd4Lvl;
    public float AddHP4Lvl;
    public int PriceUpLvl4;

    public float SpeedTrainigLvl5;
    public float AddArmor5Lvl;
    public float AddPAtck5Lvl;
    public float AddSpeed5Lvl;
    public float AddAtkSpd5Lvl;
    public float AddHP5Lvl;
    public int PriceUpLvl5;

    public GameObject Player;
    public GameObject Barrack;
    public GameObject MinionPrefab;
    public Text SystemMsg;

    void Start () {

        PriceUpLvl = PriceUpLvl2;
	}
	
	void Update () {
	
	}

    public void LvlUP()
    {
        if (Player.GetComponent<MovePlayer>().Gold > PriceUpLvl)
        {

            Barrack.GetComponent<RespawnMinion>().BarrackLvl++;

            if (Barrack.GetComponent<RespawnMinion>().BarrackLvl == 2)
            {
                MinionPrefab.GetComponent<MoveMinion>().Armor += AddArmor2Lvl;
                MinionPrefab.GetComponent<MoveMinion>().PAttack += AddPAtck2Lvl;
                MinionPrefab.GetComponent<MoveMinion>().speed += AddSpeed21Lvl;
                MinionPrefab.GetComponent<MoveMinion>().AttackSpeed += AddAtkSpd2Lvl;
                MinionPrefab.GetComponent<MoveMinion>().Health += AddHP2Lvl;
                Barrack.GetComponent<RespawnMinion>().TimeTraining -= SpeedTrainigLvl2;
                PriceUpLvl = PriceUpLvl3;
            }
            if (Barrack.GetComponent<RespawnMinion>().BarrackLvl == 3)
            {
                MinionPrefab.GetComponent<MoveMinion>().Armor += AddArmor3Lvl;
                MinionPrefab.GetComponent<MoveMinion>().PAttack += AddPAtck3Lvl;
                MinionPrefab.GetComponent<MoveMinion>().speed += AddSpeed3Lvl;
                MinionPrefab.GetComponent<MoveMinion>().AttackSpeed += AddAtkSpd3Lvl;
                MinionPrefab.GetComponent<MoveMinion>().Health += AddHP3Lvl;
                Barrack.GetComponent<RespawnMinion>().TimeTraining -= SpeedTrainigLvl3;
                PriceUpLvl = PriceUpLvl4;
            }
            if (Barrack.GetComponent<RespawnMinion>().BarrackLvl == 4)
            {
                MinionPrefab.GetComponent<MoveMinion>().Armor += AddArmor4Lvl;
                MinionPrefab.GetComponent<MoveMinion>().PAttack += AddPAtck4Lvl;
                MinionPrefab.GetComponent<MoveMinion>().speed += AddSpeed4Lvl;
                MinionPrefab.GetComponent<MoveMinion>().AttackSpeed += AddAtkSpd4Lvl;
                MinionPrefab.GetComponent<MoveMinion>().Health += AddHP4Lvl;
                Barrack.GetComponent<RespawnMinion>().TimeTraining -= SpeedTrainigLvl4;
                PriceUpLvl = PriceUpLvl5;
            }
            if (Barrack.GetComponent<RespawnMinion>().BarrackLvl == 5)
            {
                MinionPrefab.GetComponent<MoveMinion>().Armor += AddArmor5Lvl;
                MinionPrefab.GetComponent<MoveMinion>().PAttack += AddPAtck5Lvl;
                MinionPrefab.GetComponent<MoveMinion>().speed += AddSpeed5Lvl;
                MinionPrefab.GetComponent<MoveMinion>().AttackSpeed += AddAtkSpd5Lvl;
                MinionPrefab.GetComponent<MoveMinion>().Health += AddHP5Lvl;
                Barrack.GetComponent<RespawnMinion>().TimeTraining -= SpeedTrainigLvl5;
            }
        }
        else
        {
            StartCoroutine(SysMsg());
        }

    }
    // Поднятие лвлва казармы. Вместе с етим повышение характеристик юнитов.

    IEnumerator SysMsg()
    {
        SystemMsg.text = "У вас не хватает золота";

        yield return new WaitForSeconds(2);

        SystemMsg.text = "";
    }
    // Сообщение в системный чат о нехватке золота.
}
