# Animator States

* Bind MonoBehaviour State components inside Animator Controllers States.
* Unity minimum version: **2021.1**
* Current version: **2.0.0**
* License: **MIT**

## Summary

Unity has a **StateMachineBehaviour** class designed to be attached to a State or Sub-State Machine inside an Animator Controller. 
Using them, you can implement behaviors whenever you enter, update or exit a State. 
They are perfect to make logic state machines which are independent from animations.

However, the limitation of **StateMachineBehaviours** is they cannot reference instantiated scene components because they are not **MonoBehaviours**. They are **ScriptableObjects** and, as such, they are assets, not scene objects. This means that in order to exist within a scene, instances of **StateMachineBehaviours** are automatically created at runtime during the Animator's internal Awake call. 

Finding a reference to them during a MonoBehaviour's Awake function is not recommended as it will yield unpredictable results.

To solve this situation, we can use **AnimatorStateMachine** and implementations of **AbstractState** provided on this package.

## How To Use

### Using AnimatorStateMachine

Add the [AnimatorStateMachine](/Runtime/StateMachine/AnimatorStateMachine.cs) component inside the same GameObject where your **Animator** component is. 
This script will act as a bridge between the Animator Controller asset and yours [AbstractState](/Runtime/States/AbstractState.cs) implementations.

### Using AbstractState

**AbstractState** derives from **MonoBehaviour** and therefore **can reference local components** on the GameObject.

#### Implementing AbstractState class

Implement a class from [AbstractState](/Runtime/States/AbstractState.cs). 
You can override the virtual `EnterState()`, `UpdateState()` or `ExitState()` functions to execute code when the Animator enters, updates or exits this State, respectively. 

The bellow `RunState` class will play a start sound when entering it and a stop sound when exiting it:

```csharp
using UnityEngine;
using ActionCode.AnimatorStates;

namespace YourGameNamespace
{
    [DisallowMultipleComponent]
    public sealed class RunState : AbstractState
    {
        public AudioSource source;
        public AudioClip startSound;
        public AudioClip stopSound;

        protected override void EnterState()
        {
            base.EnterState();
            source.PlayOneShot(startSound);
        }

        protected override void ExitState()
        {
            base.ExitState();
            source.PlayOneShot(stopSound);
        }
    }
}
````

> **Note**: when attaching an **AbstractState** component, an **AnimatorStateMachine** component will be automatically added to this GameObject root if no one is.

You can get a reference of this state by using `AnimatorStateMachine.GetState<RunState>()` or using a serialized reference.

#### Using AbstractState events

In order to uncouple the system, `OnEntered`, `OnUpdated` and `OnExited` events are available so other classes can easily use them.

The bellow `Tutorial` class prints a log message when entering in the Run State:

```csharp
using UnityEngine;

namespace YourGameNamespace
{
   [DisallowMultipleComponent]
    public sealed class Tutorial : MonoBehaviour
    {
        public RunState runState;

        private void OnEnable() => runState.OnEntered += HandleRunEntered;
        private void OnDisable() => runState.OnEntered -= HandleRunEntered;

        private void HandleRunEntered() => print("Player started to run!");
    }
}
```

#### Binding a State into Animator Controller

The final step is open your Animation Controller asset, select a State and click on the **Add Behaviour** button.

Select **StateBinder** and type your class name on the **State Name** field, like "RunState".

![AnimatorController Screenshot](/Docs~/AnimatorController.png "Using Abstract State")

### Using AnimatorStateMachineGUI

Add the [AnimatorStateMachineGUI](/Runtime/StateMachine/AnimatorStateMachineGUI.cs) component inside the same GameObject where your **AnimatorStateMachine** component is
and you be able to see the **Current** and **Last** States on your Game window.

> **Note**: since an **Animator** has multiple Layers, there are multiple Current and Last State as well, one per Layer.

## Installation

### Using the Package Registry Server

Follow the instructions inside [here](https://cutt.ly/ukvj1c8) and the package **ActionCode-Animator States** 
will be available for you to install using the **Package Manager** windows.

### Using the Git URL

You will need a **Git client** installed on your computer with the Path variable already set. 

- Use the **Package Manager** "Add package from git URL..." feature and paste this URL: `https://github.com/HyagoOliveira/AnimatorStates.git`

- You can also manually modify you `Packages/manifest.json` file and add this line inside `dependencies` attribute: 

```json
"com.actioncode.animator-states":"https://github.com/HyagoOliveira/AnimatorStates.git"
```

---

**Hyago Oliveira**

[GitHub](https://github.com/HyagoOliveira) -
[BitBucket](https://bitbucket.org/HyagoGow/) -
[LinkedIn](https://www.linkedin.com/in/hyago-oliveira/) -
<hyagogow@gmail.com>