using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// MonoBehaviour that handles rebinding of controls.
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        private static ControlActions _inputActions;

        public static event Action RebindComplete;
        public static event Action RebindCanceled;
        public static event Action<InputAction, int> rebindStarted;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _inputActions ??= new ControlActions();
        }

        /// <summary>
        /// Starts the rebinding process for the given action and binding index.
        /// </summary>
        /// <param name="actionName">The name of the action to rebind.</param>
        /// <param name="bindingIndex">The index of the binding to rebind.</param>
        /// <param name="statusText">The text to display while rebinding.</param>
        /// <param name="excludeMouse">Whether or not to exclude mouse input.</param>
        public static void StartRebind(string actionName, int bindingIndex, Text statusText, bool excludeMouse)
        {
            var action = _inputActions.asset.FindAction(actionName);
            if (action == null || action.bindings.Count <= bindingIndex)
            {
                Debug.Log("Couldn't find action or binding");
                return;
            }

            if (action.bindings[bindingIndex].isComposite)
            {
                var firstPartIndex = bindingIndex + 1;
                if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isComposite)
                    DoRebind(action, bindingIndex, statusText, true, excludeMouse);
            }
            else
                DoRebind(action, bindingIndex, statusText, false, excludeMouse);
        }

        /// <summary>
        /// Does the actual rebinding.
        /// </summary>
        /// <param name="actionToRebind">The action to rebind.</param>
        /// <param name="bindingIndex">The index of the binding to rebind.</param>
        /// <param name="statusText">The text to display while rebinding.</param>
        /// <param name="allCompositeParts">If true, rebinds all composite parts.</param>
        /// <param name="excludeMouse">Whether or not to exclude mouse input.</param>
        private static void DoRebind(InputAction actionToRebind, int bindingIndex, Text statusText, bool allCompositeParts, bool excludeMouse)
        {
            if (actionToRebind == null || bindingIndex < 0)
                return;

            statusText.text = $"Press a {actionToRebind.expectedControlType}";

            actionToRebind.Disable();

            var rebind = actionToRebind.PerformInteractiveRebinding(bindingIndex);

            rebind.OnComplete(operation =>
            {
                actionToRebind.Enable();
                operation.Dispose();

                if(allCompositeParts)
                {
                    var nextBindingIndex = bindingIndex + 1;
                    if (nextBindingIndex < actionToRebind.bindings.Count && actionToRebind.bindings[nextBindingIndex].isComposite)
                        DoRebind(actionToRebind, nextBindingIndex, statusText, allCompositeParts, excludeMouse);
                }

                SaveBindingOverride(actionToRebind);
                RebindComplete?.Invoke();
            });

            rebind.OnCancel(operation =>
            {
                actionToRebind.Enable();
                operation.Dispose();

                RebindCanceled?.Invoke();
            });

            rebind.WithCancelingThrough("<Keyboard>/escape");

            if (excludeMouse)
                rebind.WithControlsExcluding("Mouse");

            rebindStarted?.Invoke(actionToRebind, bindingIndex);
            rebind.Start(); //actually starts the rebinding process
        }

        public static string GetBindingName(string actionName, int bindingIndex)
        {
            _inputActions ??= new ControlActions();

            var action = _inputActions.asset.FindAction(actionName);
            return action.GetBindingDisplayString(bindingIndex);
        }

        private static void SaveBindingOverride(InputAction action)
        {
            for (var i = 0; i < action.bindings.Count; i++)
            {
                PlayerPrefs.SetString(action.actionMap + action.name + i, action.bindings[i].overridePath);
            }
        }

        public static void LoadBindingOverride(string actionName)
        {
            _inputActions ??= new ControlActions();

            var action = _inputActions.asset.FindAction(actionName);

            for (var i = 0; i < action.bindings.Count; i++)
            {
                if (!string.IsNullOrEmpty(PlayerPrefs.GetString(action.actionMap + action.name + i)))
                    action.ApplyBindingOverride(i, PlayerPrefs.GetString(action.actionMap + action.name + i));
            }
        }

        public static void ResetBinding(string actionName, int bindingIndex)
        {
            var action = _inputActions.asset.FindAction(actionName);

            if(action == null || action.bindings.Count <= bindingIndex)
            {
                Debug.Log("Could not find action or binding");
                return;
            }

            if (action.bindings[bindingIndex].isComposite)
            {
                for (var i = bindingIndex; i < action.bindings.Count && action.bindings[i].isComposite; i++)
                    action.RemoveBindingOverride(i);
            }
            else
                action.RemoveBindingOverride(bindingIndex);

            SaveBindingOverride(action);
        }

    }
}
