# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Fixed
- Null reference exception on AnimatorStateMachine.IsExecuting function

## [2.1.0] - 2023-03-30
### Added
- AnimatorStateMachine.Animator property

## [2.0.0] - 2023-01-15
### Changed
- Increase Unity minimum version to 2021.1
- AbstractState OnEnter, OnUpdate and OnExit are renamed to OnEntered, OnUpdated and OnExited

### Added
- IState interface
- AbstractState component
- StateBinder
- AnimatorStateMachineLayer

### Removed
- AbstractAnimatorState
- AbstractMonoBehaviourState
- GenericMonoBehaviourState
- MonoBehaviourState

## [1.2.0] - 2022-12-19
### Added
- BehaviourStates property and Get functions into AnimatorStateMachine component.

## [1.1.0] - 2022-12-11
### Added
- GenericMonoBehaviourState
- MonoBehaviourState
- AbstractMonoBehaviourState

## [1.0.0] - 2022-06-06
### Added
- AnimatorStateMachineGUI
- AnimatorStateMachine
- AbstractAnimatorState
- CHANGELOG
- Package file
- README
- LICENSE
- gitignore
- Initial commit

[Unreleased]: https://github.com/HyagoOliveira/AnimatorStates/compare/2.1.0...main
[2.1.0]: https://github.com/HyagoOliveira/AnimatorStates/tree/2.1.0/
[2.0.0]: https://github.com/HyagoOliveira/AnimatorStates/tree/2.0.0/
[1.2.0]: https://github.com/HyagoOliveira/AnimatorStates/tree/1.2.0/
[1.1.0]: https://github.com/HyagoOliveira/AnimatorStates/tree/1.1.0/
[1.0.0]: https://github.com/HyagoOliveira/AnimatorStates/tree/1.0.0/