using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMenu : MonoBehaviour {

    public GameObject leftUIMove;
    public GameObject leftUISummon;
    public GameObject leftUIAttack;
    public GameObject leftUIBackground;

    public GameObject rightUIMove;
    public GameObject rightUISummon;
    public GameObject rightUIAttack;
    public GameObject rightUIBackground;

    InputController input;

    bool isLeft;
    bool isSummon;
    bool isActive;

    // Use this for initialization
    void Start () {        
        leftUIMove.SetActive(false);
        leftUISummon.SetActive(false);
        leftUIAttack.SetActive(false);
        leftUIBackground.SetActive(false);

        rightUIMove.SetActive(false);
        rightUISummon.SetActive(false);
        rightUIAttack.SetActive(false);
        rightUIBackground.SetActive(false);
        input = FindObjectOfType<InputController>();

        isLeft = true;
        isSummon = false;
    }

    // Update is called once per frame
    void Update () {
        CheckSide();
	}

    void CheckSide()
    {
        bool currentside = isLeft;
        Vector3 worldActive = input.UIActiveSquare.transform.position;
        Vector2 viewportActive = Camera.main.WorldToViewportPoint(worldActive);
        if (viewportActive != null)
        {
            if (viewportActive.x < 0.3)
            {
                isLeft = false;
            }
            else if (viewportActive.x >= 0.7)
            {
                isLeft = true;
            }
        }

        if (currentside != isLeft && isActive)
        {
            Deactivate();
            Activate(isSummon);
        }
    }

    public void Activate(bool unitSummoner)
    {
        isSummon = unitSummoner;
        isActive = true;
        if (isLeft)
        {
            leftUIBackground.SetActive(true);
            leftUIMove.SetActive(true);

            if (unitSummoner)
            {
                leftUISummon.SetActive(true);
            }
            else
            {
                leftUIAttack.SetActive(true);
            }
        }
        else
        { 
            rightUIBackground.SetActive(true);
            rightUIMove.SetActive(true);

            if (unitSummoner)
            {
                rightUISummon.SetActive(true);
            }
            else
            {
                rightUIAttack.SetActive(true);
            }
        }
    }

    public void Deactivate()
    {
        isActive = false;

        leftUIBackground.SetActive(false);
        leftUIMove.SetActive(false);
        leftUISummon.SetActive(false);
        leftUIAttack.SetActive(false);

        rightUIMove.SetActive(false);
        rightUISummon.SetActive(false);
        rightUIAttack.SetActive(false);
        rightUIBackground.SetActive(false);
    }


    public void AttackPressed()
    {
        Deactivate();
        input.AttackFind();
    }

    public void MovePressed()
    {
        Deactivate();
        input.MoveFind();
    }

    public void SummonPressed()
    {

    }
}
