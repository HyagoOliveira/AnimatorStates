# Animator States

* Use scene instance objects on Animators Controllers assets, inside StateMachineBehaviour.
* Unity minimum version: **2019.3**
* Current version: **1.0.0**
* License: **MIT**

## Summary

Unity has a **StateMachineBehaviour** class designed to be attached to an animator state or sub-state machine in an Animator Controller. 
These can be used to add all sorts of state dependent behaviours such as playing sounds, particle effects or enable/disable colliders whenever you enter a state. 
They can even be used to make logic state machines which are independent of animation for reusability.

The limitation of StateMachineBehaviours is they cannot reference instantiated scene components. StateMachineBehaviours are not like MonoBehaviours in the way they are created. Instances of MonoBehaviours are created when they are added to a GameObject and are therefore scene objects. 
The StateMachineBehaviour class derives from ScriptableObject. As such, state machine behaviours are assets, not scene objects. This means that in order to exist within a scene, instances of state machine behaviours are automatically created at runtime during the Animator's internal Awake call. 
Finding a reference to them during a MonoBehaviour's Awake function is not recommended as it will yield unpredictable results.

To solve this situation, we can use **AnimatorStateMachine** and **AbstractAnimatorState** implementations provided on this package.

## How To Use

### Using AnimatorStateMachine

Add the [AnimatorStateMachine](/Runtime/AnimatorStateMachine.cs) component inside the same GameObject where your Animator component is. 
This script will act as a bridge to the referenced scene objects and yours [AbstractAnimatorState](/Runtime/AbstractAnimatorState.cs) implementations.

AbstractAnimatorState derives from StateMachineBehaviour and therefore are identical to them but **can reference instantiated components** on the Scene. 
You can create implementations of this class and use some functionalities already implemented like find components inside the runtime transform.

### Using AnimatorStateMachineGUI

Add the [AnimatorStateMachineGUI](/Runtime/AnimatorStateMachineGUI.cs) component inside the same GameObject where your AnimatorStateMachine component is
and you be able to see the Current and Last States from your Animator.

![AnimatorStateMachineGUI Screenshot](/Docs~/AnimatorStateMachineGUI.png "Using AnimatorStateMachineGUI")

**This component only works on Editor Mode.**

## Installation

### Using the Package Registry Server

Follow the instructions inside [here](https://cutt.ly/ukvj1c8) and the package **ActionCode-[PACKAGE]** 
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