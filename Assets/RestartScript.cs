using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartScript : MonoBehaviour
{
    public void SwitchToMainMenu()
    {
        GameController.instance.ChangeState(GameState.MENU);
    }
}
