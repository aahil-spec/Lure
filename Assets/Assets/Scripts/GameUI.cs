using UnityEngine;
using UnityEngine.UI;

public class GameUI:MonoBehaviour
{
    public static GameUI Instance;

    public Text promptText;
    public Text notificationText;
    private float notificationTimer;

    void Awake()
    {
        Instance=this;
        HidePrompt();
        notificationText.text="";
    }
    void Update()
    {
        if (notificationTimer>0)
        {
            notificationTimer-=Time.deltaTime;
            if (notificationTimer<=0)
            {
                notificationText.text="";
            }
        }
    }
    public void ShowPrompt(string message)
    {
        promptText.text=message;
    }
    public void HidePrompt()
    {
        promptText.text="";

    }
    public void ShowNotification(string message)
    {
        notificationText.text=message;
        notificationTimer=3f;
    }
    
}
