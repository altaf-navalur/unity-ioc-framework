# IOC Framework for Unity

A lightweight and extensible **Inversion of Control (IOC)** framework built for Unity by **Altaf Navalur**.

This framework provides a powerful signal-command architecture for decoupled communication and execution flow management in Unity projects.

---

## 🚀 Installation

### Option 1: Using Git URL
Add this line to your project's `Packages/manifest.json`:
```json
"com.xcelerategames.ioc": "https://github.com/altaf-navalur/unity-ioc-framework.git#1.0.5"
```

### Option 2: Via OpenUPM (coming soon)
```bash
openupm add com.xcelerategames.ioc
```

---

## 📋 Requirements

- Unity 2021.3 or later

---

## 🔧 Project Setup

### 1. Create a Binding Manager

Create a class that inherits from `BindingManager` to configure your signals and commands:

```csharp
using XcelerateGames.IOC;

public class MyGameBindings : BindingManager
{
    protected override void SetBindings()
    {
        base.SetBindings();
        
        // Bind your signals
        BindSignal<StartGameSignal>();
        BindSignal<PlayerDeathSignal>();
        BindSignal<ScoreUpdateSignal>();
    }

    protected override void SetFlow()
    {
        base.SetFlow();
        
        // Define signal-command relationships
        On<StartGameSignal>().Do<InitializeGameCommand>();
        On<PlayerDeathSignal>().Do<ShowGameOverCommand>().OnFinish<RestartGameCommand>();
        On<ScoreUpdateSignal>().Do<UpdateUICommand>().Do<SaveScoreCommand>();
    }
}
```

### 2. Add the Binding Manager to a GameObject

1. Create an empty GameObject in your scene
2. Add your `MyGameBindings` component to it
3. The framework will automatically initialize when the scene starts

---

## 📡 Working with Signals

### Creating Signals

Signals are used for decoupled communication between different parts of your application:

```csharp
using XcelerateGames.IOC;

// Signal with no parameters
public class StartGameSignal : Signal { }

// Signal with one parameter
public class ScoreUpdateSignal : Signal<int> { }

// Signal with multiple parameters
public class PlayerDataSignal : Signal<string, int, bool> { }

// Up to 5 parameters are supported
public class ComplexSignal : Signal<int, float, bool, string, Vector3> { }
```

### Dispatching Signals

```csharp
using XcelerateGames.IOC;

public class GameManager : BaseBehaviour
{
    [InjectSignal] private StartGameSignal startGameSignal = null;
    [InjectSignal] private ScoreUpdateSignal scoreUpdateSignal = null;
    
    public void StartNewGame()
    {
        startGameSignal.Dispatch();
    }
    
    public void UpdateScore(int newScore)
    {
        scoreUpdateSignal.Dispatch(newScore);
    }
}
```

### Listening to Signals

```csharp
using XcelerateGames.IOC;
using UnityEngine;

public class UIManager : BaseBehaviour
{
    [InjectSignal] private ScoreUpdateSignal scoreUpdateSignal = null;
    
    void Start()
    {
        scoreUpdateSignal.AddListener(OnScoreUpdated);
    }
    
    private void OnScoreUpdated(int newScore)
    {
        Debug.Log($"Score updated: {newScore}");
        // Update UI here
    }
    
    void OnDestroy()
    {
        scoreUpdateSignal?.RemoveListener(OnScoreUpdated);
    }
}
```

### Signal Features

#### Muting Signals
```csharp
// Mute a signal (prevents dispatch)
On<MySignal>().Mute<MySignal>();

// Unmute a signal
On<MySignal>().UnMute<MySignal>();
```

#### Pooling Control
```csharp
// Disable pooling for a signal (default is enabled)
On<MySignal>().DoNotPool();
```

---

## ⚡ Working with Commands

### Creating Commands

Commands encapsulate business logic and can be executed in response to signals:

```csharp
using UnityEngine;
using XcelerateGames.IOC;

public class InitializeGameCommand : Command
{
    public override void Execute()
    {
        Debug.Log("Initializing game...");
        // Your initialization logic here
        
        // Always call base.Execute() or Release() when done
        base.Execute();
    }
}
```

### Commands with Parameters

```csharp
using UnityEngine;
using XcelerateGames.IOC;

public class UpdateScoreCommand : Command
{
    [InjectParameter] private int score = 0;
    
    public override void Execute()
    {
        Debug.Log($"Updating score to: {score}");
        // Update score logic here
        
        base.Execute();
    }
}
```

### Asynchronous Commands

For commands that take time to complete:

```csharp
using System.Collections;
using UnityEngine;
using XcelerateGames.IOC;

public class LoadLevelCommand : Command
{
    public override void Execute()
    {
        StartCoroutine(LoadLevelAsync());
        // Don't call base.Execute() immediately
    }
    
    private IEnumerator LoadLevelAsync()
    {
        Debug.Log("Loading level...");
        
        // Simulate loading
        yield return new WaitForSeconds(2f);
        
        Debug.Log("Level loaded!");
        
        // Call Release() when the async operation is complete
        Release();
    }
}
```

---

## 🔗 Command Chaining and Flow Control

### Sequential Execution

Commands bound to the same signal execute in sequence by default:

```csharp
protected override void SetFlow()
{
    base.SetFlow();
    
    // These commands will execute one after another
    On<StartGameSignal>()
        .Do<InitializeCommand>()
        .Do<LoadUICommand>()
        .Do<StartBackgroundMusicCommand>();
}
```

### Completion and Error Handling

```csharp
protected override void SetFlow()
{
    base.SetFlow();
    
    On<ComplexOperationSignal>()
        .Do<Step1Command>()
        .Do<Step2Command>()
        .Do<Step3Command>()
        .OnFinish<OperationCompleteCommand>()    // Executes after all succeed
        .OnAbort<HandleErrorCommand>();          // Executes if any command fails
}
```

### Parallel Execution

```csharp
protected override void SetFlow()
{
    base.SetFlow();
    
    // Execute all commands simultaneously
    On<InitializeSignal>()
        .Do<LoadAssetsCommand>()
        .Do<ConnectToServerCommand>()
        .Do<InitializeUICommand>()
        .ExecuteParallel();
}
```

### Additional Flow Options

```csharp
protected override void SetFlow()
{
    base.SetFlow();
    
    On<MySignal>()
        .Do<Command1>()
        .Do<Command2>()
        .ContinueOnAbort()    // Continue even if a command fails
        .Once();              // Execute only once, then unbind
}
```

### Dispatching Signals from Commands

```csharp
protected override void SetFlow()
{
    base.SetFlow();
    
    // Chain signal dispatches
    On<StartGameSignal>()
        .Do<InitializeCommand>()
        .Dispatch<GameReadySignal>()    // Dispatch another signal
        .Do<ShowMainMenuCommand>();
}
```

### Undoing Commands

```csharp
protected override void SetFlow()
{
    base.SetFlow();
    
    // Remove a command from the sequence
    On<MySignal>()
        .Do<Command1>()
        .Do<Command2>()
        .Undo<Command1>();    // Removes Command1 from the sequence
}
```

---

## 🎯 Best Practices

### 1. Use BaseBehaviour for Injection
Always inherit from `BaseBehaviour` instead of `MonoBehaviour` to enable automatic injection:

```csharp
public class MyClass : BaseBehaviour  // Not MonoBehaviour
{
    [InjectSignal] private MySignal mySignal = null;
}
```

### 2. Proper Signal Lifecycle Management
Always remove listeners in `OnDestroy`:

```csharp
void OnDestroy()
{
    mySignal.RemoveListener(OnMySignalReceived);
}
```

### 3. Keep Commands Focused
Each command should have a single responsibility:

```csharp
// Good
public class SaveGameCommand : Command { }
public class ShowSaveConfirmationCommand : Command { }

// Avoid
public class SaveGameAndShowConfirmationCommand : Command { }
```

### 4. Use Parameters Wisely
Inject parameters from signals into commands:

```csharp
// Signal carries data
public class PlayerLevelUpSignal : Signal<int, string> { }

// Command receives the data
public class ShowLevelUpCommand : Command
{
    [InjectParameter] private int newLevel = 0;
    [InjectParameter] private string playerName = "";
}
```

---

## 📁 Sample Project

Check the `Samples/iOCDemo` folder for a complete working example that demonstrates:
- Basic signal dispatching
- Command execution with parameters
- Sequential command chains
- Signal listening
- UI integration

---

## 📄 License

This project is licensed under the MIT License.

---

## 👨‍💻 Author

**Altaf Navalur**
- GitHub: [@altaf-navalur](https://github.com/altaf-navalur)
