using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class Menu : VisualElement
{
  VisualElement mainMenu;
  VisualElement pauseMenu;
  VisualElement settingsMenu;
  VisualElement endMenu;

  GameState menuState = GameState.MainMenu;

  GameState screenBeforeSettings;

  bool hasInitialized = false;

  public new class UxmlFactory : UxmlFactory<Menu, UxmlTraits> { }

  public Menu()
  {
    this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    menuState = GameState.MainMenu;
    EventManager.OnEventEmitted += HandleEvent;
  }

  void HandleEvent(string eventKey, object data)
  {
    Dictionary<string, object> dataDict = data as Dictionary<string, object>;
    if (eventKey == "gameStarted" && menuState != GameState.Playing)
    {
      hideMenu();
    }
    else if (eventKey == "gamePaused" && menuState != GameState.Paused)
    {
      goToPauseMenu();
    }
    else if (eventKey == "gameEnded" && menuState != GameState.End)
    {
      goToEndMenu();
    }
    else if (eventKey == "gameResumed" && menuState != GameState.Playing)
    {
      hideMenu();
    }
  }

  void OnGeometryChange(GeometryChangedEvent evt)
  {
    mainMenu = this.Q("MainMenu");
    pauseMenu = this.Q("PauseMenu");
    settingsMenu = this.Q("SettingsMenu");
    endMenu = this.Q("EndMenu");

    registerCommands();

    if (!hasInitialized) initializeMainMenu();

    //Boilerplate
    this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
  }

  void initializeMainMenu()
  {
    goToMainMenu();
    hasInitialized = true;
  }

  void registerCommands()
  {
    mainMenu?.Q("startButton").RegisterCallback<ClickEvent>(evt =>
    {
      EventManager.EmitEvent("startGame", null);
      hideMenu();
    });

    mainMenu?.Q("settingsButton").RegisterCallback<ClickEvent>(evt =>
    {
      screenBeforeSettings = GameState.MainMenu;
      goToSettingsMenu();
    });

    pauseMenu?.Q("restartButton").RegisterCallback<ClickEvent>(evt =>
    {
      EventManager.EmitEvent("startGame", null);
      hideMenu();
    });

    pauseMenu?.Q("resumeButton").RegisterCallback<ClickEvent>(evt =>
    {
      EventManager.EmitEvent("gameResumed", null);
      hideMenu();
    });

    pauseMenu?.Q("settingsButton").RegisterCallback<ClickEvent>(evt =>
    {
      screenBeforeSettings = GameState.Paused;
      goToSettingsMenu();
    });

    endMenu?.Q("restartButton").RegisterCallback<ClickEvent>(evt =>
    {
      EventManager.EmitEvent("startGame", null);
      hideMenu();
    });

    settingsMenu?.Q("speed").RegisterCallback<ChangeEvent<float>>(evt =>
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      data.Add("speed", evt.newValue);
      EventManager.EmitEvent("speedChanged", data);
    });

    settingsMenu?.Q("close").RegisterCallback<ClickEvent>(evt =>
    {
      if (screenBeforeSettings == GameState.MainMenu) goToMainMenu();
      else if (screenBeforeSettings == GameState.Paused) goToPauseMenu();
      screenBeforeSettings = GameState.MainMenu;
    });
  }

  public void hideMenu()
  {
    mainMenu.style.display = DisplayStyle.None;
    settingsMenu.style.display = DisplayStyle.None;
    pauseMenu.style.display = DisplayStyle.None;
    endMenu.style.display = DisplayStyle.None;

    menuState = GameState.Playing;
  }

  public void goToMainMenu()
  {
    mainMenu.style.display = DisplayStyle.Flex;
    settingsMenu.style.display = DisplayStyle.None;
    pauseMenu.style.display = DisplayStyle.None;
    endMenu.style.display = DisplayStyle.None;

    menuState = GameState.MainMenu;
  }

  public void goToSettingsMenu()
  {
    mainMenu.style.display = DisplayStyle.None;
    settingsMenu.style.display = DisplayStyle.Flex;
    pauseMenu.style.display = DisplayStyle.None;
    endMenu.style.display = DisplayStyle.None;

    menuState = GameState.Settings;
  }

  public void goToPauseMenu()
  {
    mainMenu.style.display = DisplayStyle.None;
    settingsMenu.style.display = DisplayStyle.None;
    pauseMenu.style.display = DisplayStyle.Flex;
    endMenu.style.display = DisplayStyle.None;

    menuState = GameState.Paused;
  }

  public void goToEndMenu()
  {
    mainMenu.style.display = DisplayStyle.None;
    settingsMenu.style.display = DisplayStyle.None;
    pauseMenu.style.display = DisplayStyle.None;
    endMenu.style.display = DisplayStyle.Flex;

    menuState = GameState.End;
  }

}