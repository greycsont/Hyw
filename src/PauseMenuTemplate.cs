using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace HYW;

public abstract class PauseMenuTemplate : MonoBehaviour
{
    protected Button resumeBtn;
    protected Button checkpointBtn;
    protected Button optionsBtn;
    protected Button restartBtn;
    protected Button quitBtn;
    protected PauseMenu pauseMenuScript;
    protected RectTransform buttonsContainer;

    protected virtual void Awake()
    {
        OnPreSetup();
        EnsureFullScreen();
        SetUpLayout();
        BuildFixedContent();
        SetupCustomContent();
        OnPostSetup();
    }

    protected virtual void OnEnable() {}

    private void EnsureFullScreen()
    {
        RectTransform rt = GetComponent<RectTransform>();
        if (rt == null) rt = gameObject.AddComponent<RectTransform>();
        transform.SetAsFirstSibling();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        
        rt.localScale = Vector3.one;
        rt.localPosition = Vector3.zero;
    }

    private void BuildFixedContent()
    {
        CreateButton();
        AddPauseMenuScript();
    }

    private void CreateButton()
    {
        resumeBtn = AddButton("RESUME", OnResumeClicked);
        checkpointBtn = AddButton("CHECKPOINT", OnCheckpointClicked);
        restartBtn = AddButton("RESTART", OnRestartClicked);
        optionsBtn = AddButton("OPTIONS", OnOptionsClicked);
        quitBtn = AddButton("QUIT", OnQuitClicked);

        gameObject.AddComponent<GamepadObjectSelector>();
    }
    protected virtual void OnResumeClicked() { OptionsMenuToManager.Instance.UnPause(); }
    protected virtual void OnCheckpointClicked() { OptionsMenuToManager.Instance.RestartCheckpoint(); }
    protected virtual void OnOptionsClicked() { OptionsMenuToManager.Instance.OpenOptions(); }
    protected virtual void OnRestartClicked() { OptionsMenuToManager.Instance.RestartMission(); }
    protected virtual void OnQuitClicked() { OptionsMenuToManager.Instance.QuitMission(); }
    
    protected virtual void OnPreSetup() { } 
    protected virtual void SetupCustomContent() { }
    protected virtual void OnPostSetup() { }
    

    public Button AddButton(string label, UnityAction onClick)
    {
        GameObject btnObj = new GameObject(label + "Button");
        btnObj.transform.SetParent(buttonsContainer, false);

        RectTransform rt = btnObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(100, 50);

        btnObj.AddComponent<CanvasRenderer>();
        Image img = btnObj.AddComponent<Image>(); 
        img.color = new Color(1, 1, 1, 0.5f);

        Button btn = btnObj.AddComponent<Button>();
        btn.onClick.AddListener(onClick);

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btnObj.transform, false);
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = label;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontSize = 12;

        RectTransform textRt = tmp.rectTransform;
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = textRt.offsetMax = Vector2.zero;

        return btn;
    }

    private void SetUpLayout()
    {
        GameObject containerObj = new GameObject("ButtonsContainer");
        containerObj.transform.SetParent(this.transform, false);
        buttonsContainer = containerObj.AddComponent<RectTransform>();
        
        buttonsContainer.anchorMin = new Vector2(0, 0);
        buttonsContainer.anchorMax = new Vector2(0.5f, 1);
        buttonsContainer.offsetMin = new Vector2(0, 0);
        buttonsContainer.offsetMax = new Vector2(0, 0);
        
        var layout = containerObj.AddComponent<VerticalLayoutGroup>();
        
        layout.childAlignment = TextAnchor.UpperLeft;
        
        layout.padding = new RectOffset(25, 0, 25, 0); 

        layout.spacing = 10;
        
        layout.childControlHeight = false; 
        layout.childControlWidth = false; 
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = false;
    }


    private void AddPauseMenuScript()
	{
        // Reference: PauseMenu in Assembly_CSharp
        var instance = MapInfoBase.Instance;
        if (instance == null)
        {
            checkpointBtn.interactable = false;
        }
        else if (instance.replaceCheckpointButtonWithSkip)
        {
            checkpointBtn.GetComponentInChildren<TMP_Text>().text = "SKIP";
            checkpointBtn.interactable = true;
            checkpointBtn.onClick.RemoveAllListeners();
            checkpointBtn.onClick.AddListener(OnCheckpointButton);
        }
        else
        {
            bool hasCheckpoint = MonoSingleton<StatsManager>.Instance.currentCheckPoint != null;
            checkpointBtn.interactable = hasCheckpoint;
        }
    }

    private void OnCheckpointButton()
    {
        StockMapInfo instance = StockMapInfo.Instance;
        if (!(instance == null))
        {
            string nextSceneName = instance.nextSceneName;
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                MonoSingleton<OptionsMenuToManager>.Instance.ChangeLevel(nextSceneName);
            }
        }
    }
}