using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Victop.Server.Controls.MVVM
{
    /// <summary>
    /// 提供松散耦合的消息通知机制，为防止内存泄漏，所有对象都使用了弱引用（WeakReference）
    /// </summary>
    public class Messenger
    {
        #region Constructor

        public Messenger()
        {
        }

        #endregion // Constructor

        private static readonly object CreationLock = new object();
        private static Messenger _defaultInstance;
        /// <summary>
        /// 消息管理器默认
        /// </summary>
        public static Messenger Default
        {
            get
            {
                if (_defaultInstance == null)
                {
                    lock (CreationLock)
                    {
                        if (_defaultInstance == null)
                        {
                            _defaultInstance = new Messenger();
                        }
                    }
                }

                return _defaultInstance;
            }
        }
        #region Register

        /// <summary>
        /// 注册消息监听,没有参数
        /// </summary>
        /// <param name="message">注册的消息名称</param>
        /// <param name="callback">消息被广播时回调方法</param>
        public void Register(string message, Action callback)
        {
            this.Register(message, callback, null);
        }

        /// <summary>
        /// 注册消息监听，带参数
        /// </summary>
        /// <param name="message">注册的消息名称</param>
        /// <param name="callback">消息被广播时回调方法</param>
        public void Register<T>(string message, Action<T> callback)
        {
            this.Register(message, callback, typeof(T));
        }

        void Register(string message, Delegate callback, Type parameterType)
        {
            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("'message' cannot be null or empty.");

            if (callback == null)
                throw new ArgumentNullException("callback");

            this.VerifyParameterType(message, parameterType);

            _messageToActionsMap.AddAction(message, callback.Target, callback.Method, parameterType);
        }

        [Conditional("DEBUG")]
        void VerifyParameterType(string message, Type parameterType)
        {
            Type previouslyRegisteredParameterType = null;
            if (_messageToActionsMap.TryGetParameterType(message, out previouslyRegisteredParameterType))
            {
                if (previouslyRegisteredParameterType != null && parameterType != null)
                {
                    if (!previouslyRegisteredParameterType.Equals(parameterType))
                        throw new InvalidOperationException(string.Format(
                            "The registered action's parameter type is inconsistent with the previously registered actions for message '{0}'.\nExpected: {1}\nAdding: {2}",
                            message,
                            previouslyRegisteredParameterType.FullName,
                            parameterType.FullName));
                }
                else
                {
                    // One, or both, of previouslyRegisteredParameterType or callbackParameterType are null.
                    if (previouslyRegisteredParameterType != parameterType)   // not both null?
                    {
                        throw new TargetParameterCountException(string.Format(
                            "The registered action has a number of parameters inconsistent with the previously registered actions for message \"{0}\".\nExpected: {1}\nAdding: {2}",
                            message,
                            previouslyRegisteredParameterType == null ? 0 : 1,
                            parameterType == null ? 0 : 1));
                    }
                }
            }
        }

        #endregion // Register

        #region Notify

        /// <summary>
        /// 发送消息通知，触发监听执行
        /// </summary>
        /// <param name="message">消息名称</param>
        /// <param name="parameter">参数</param>
        public void Notify(string message, object parameter)
        {
            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("'message' cannot be null or empty.");

            Type registeredParameterType;
            if (_messageToActionsMap.TryGetParameterType(message, out registeredParameterType))
            {
                if (registeredParameterType == null)
                    throw new TargetParameterCountException(string.Format("Cannot pass a parameter with message '{0}'. Registered action(s) expect no parameter.", message));
            }

            var actions = _messageToActionsMap.GetActions(message);
            if (actions != null)
                actions.ForEach(action => action.DynamicInvoke(parameter));
        }

        /// <summary>
        /// 发送消息通知，触发监听执行
        /// </summary>
        /// <param name="message">消息名称</param>
        public void Notify(string message)
        {
            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("'message' cannot be null or empty.");

            Type registeredParameterType;
            if (_messageToActionsMap.TryGetParameterType(message, out registeredParameterType))
            {
                if (registeredParameterType != null)
                    throw new TargetParameterCountException(string.Format("Must pass a parameter of type {0} with this message. Registered action(s) expect it.", registeredParameterType.FullName));
            }

            var actions = _messageToActionsMap.GetActions(message);
            if (actions != null)
                actions.ForEach(action => action.DynamicInvoke());
        }

        #endregion // NotifyColleauges

        #region MessageToActionsMap [nested class]

        /// <summary>
        /// This class is an implementation detail of the Messenger class.
        /// </summary>
        private class MessageToActionsMap
        {
            #region Constructor

            internal MessageToActionsMap()
            {
            }

            #endregion // Constructor

            #region AddAction

            /// <summary>
            /// Adds an action to the list.
            /// </summary>
            /// <param name="message">The message to register.</param>
            /// <param name="target">The target object to invoke, or null.</param>
            /// <param name="method">The method to invoke.</param>
            /// <param name="actionType">The type of the Action delegate.</param>
            internal void AddAction(string message, object target, MethodInfo method, Type actionType)
            {
                if (message == null)
                    throw new ArgumentNullException("message");

                if (method == null)
                    throw new ArgumentNullException("method");

                lock (_map)
                {
                    if (!_map.ContainsKey(message))
                        _map[message] = new List<WeakAction>();

                    _map[message].Add(new WeakAction(target, method, actionType));
                }
            }

            #endregion // AddAction

            #region GetActions

            /// <summary>
            /// Gets the list of actions to be invoked for the specified message
            /// </summary>
            /// <param name="message">The message to get the actions for</param>
            /// <returns>Returns a list of actions that are registered to the specified message</returns>
            internal List<Delegate> GetActions(string message)
            {
                if (message == null)
                    throw new ArgumentNullException("message");

                List<Delegate> actions;
                lock (_map)
                {
                    if (!_map.ContainsKey(message))
                        return null;

                    List<WeakAction> weakActions = _map[message];
                    actions = new List<Delegate>(weakActions.Count);
                    for (int i = weakActions.Count - 1; i > -1; --i)
                    {
                        WeakAction weakAction = weakActions[i];
                        if (weakAction == null)
                            continue;

                        Delegate action = weakAction.CreateAction();
                        if (action != null)
                        {
                            actions.Add(action);
                        }
                        else
                        {
                            // The target object is dead, so get rid of the weak action.
                            weakActions.Remove(weakAction);
                        }
                    }

                    // Delete the list from the map if it is now empty.
                    if (weakActions.Count == 0)
                        _map.Remove(message);
                }

                // Reverse the list to ensure the callbacks are invoked in the order they were registered.
                actions.Reverse();

                return actions;
            }

            #endregion // GetActions

            #region TryGetParameterType

            /// <summary>
            /// Get the parameter type of the actions registered for the specified message.
            /// </summary>
            /// <param name="message">The message to check for actions.</param>
            /// <param name="parameterType">
            /// When this method returns, contains the type for parameters 
            /// for the registered actions associated with the specified message, if any; otherwise, null.
            /// This will also be null if the registered actions have no parameters.
            /// This parameter is passed uninitialized.
            /// </param>
            /// <returns>true if any actions were registered for the message</returns>
            internal bool TryGetParameterType(string message, out Type parameterType)
            {
                if (message == null)
                    throw new ArgumentNullException("message");

                parameterType = null;
                List<WeakAction> weakActions;
                lock (_map)
                {
                    if (!_map.TryGetValue(message, out weakActions) || weakActions.Count == 0)
                        return false;
                }
                parameterType = weakActions[0].ParameterType;
                return true;
            }

            #endregion // TryGetParameterType

            #region Fields

            // Stores a hash where the key is the message and the value is the list of callbacks to invoke.
            readonly Dictionary<string, List<WeakAction>> _map = new Dictionary<string, List<WeakAction>>();

            #endregion // Fields
        }

        #endregion // MessageToActionsMap [nested class]

        #region WeakAction [nested class]

        /// <summary>
        /// This class is an implementation detail of the MessageToActionsMap class.
        /// </summary>
        private class WeakAction
        {
            #region Constructor

            /// <summary>
            /// Constructs a WeakAction.
            /// </summary>
            /// <param name="target">The object on which the target method is invoked, or null if the method is static.</param>
            /// <param name="method">The MethodInfo used to create the Action.</param>
            /// <param name="parameterType">The type of parameter to be passed to the action. Pass null if there is no parameter.</param>
            internal WeakAction(object target, MethodInfo method, Type parameterType)
            {
                if (target == null)
                {
                    _targetRef = null;
                }
                else
                {
                    _targetRef = new WeakReference(target);
                }

                _method = method;

                this.ParameterType = parameterType;

                if (parameterType == null)
                {
                    _delegateType = typeof(Action);
                }
                else
                {
                    _delegateType = typeof(Action<>).MakeGenericType(parameterType);
                }
            }

            #endregion // Constructor

            #region CreateAction

            /// <summary>
            /// Creates a "throw away" delegate to invoke the method on the target, or null if the target object is dead.
            /// </summary>
            internal Delegate CreateAction()
            {
                // Rehydrate into a real Action object, so that the method can be invoked.
                if (_targetRef == null)
                {
                    return Delegate.CreateDelegate(_delegateType, _method);
                }
                else
                {
                    try
                    {
                        object target = _targetRef.Target;
                        if (target != null)
                            return Delegate.CreateDelegate(_delegateType, target, _method);
                    }
                    catch
                    {
                    }
                }

                return null;
            }

            #endregion // CreateAction

            #region Fields

            internal readonly Type ParameterType;

            readonly Type _delegateType;
            readonly MethodInfo _method;
            readonly WeakReference _targetRef;

            #endregion // Fields
        }

        #endregion // WeakAction [nested class]

        #region Fields

        readonly MessageToActionsMap _messageToActionsMap = new MessageToActionsMap();

        #endregion // Fields
    }
}
