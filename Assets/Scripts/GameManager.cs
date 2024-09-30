using Resul.Helper;
using UnityEngine;

public class GameManager : LocalSingleton<GameManager>
{
    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    

    private void ShowFailPanel()
    {
        // Fail panelini aktif hale getir
        //failPanel.SetActive(true);
    }
    
}
