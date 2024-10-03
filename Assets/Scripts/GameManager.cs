using Resul.Helper;
using UnityEngine;

public class GameManager : LocalSingleton<GameManager>
{
    private void Start()
    {
        Application.targetFrameRate = 60;
        
        WheelController.Instance.Init();
        CurrencyManager.Instance.Init();
        
    }
    
}
