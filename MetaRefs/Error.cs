using System;
using UnityEngine;
using Object = UnityEngine.Object;

// Fail loud and ASAP

namespace Assets.Metater.MetaRefs
{
    public sealed class MetaRefsException : Exception
    {
        public MetaRefsException(string message) : base(message) { }
    }

    public static class Error
    {
        public static void NullInstance()
        {
            const string Message = "Attempted to grab a null instance.";
            Debug.LogError(Message);
            throw new MetaRefsException(Message);
        }

        public static void MultipleInstances(Object context)
        {
            const string Message = "Attempted to initialize an instance more than once.";
            Debug.LogError(Message, context);
            throw new MetaRefsException(Message);
        }

        public static void TypeMismatch(Object context)
        {
            const string Message = "This type does not match the given generic type.";
            Debug.LogError(Message, context);
            throw new MetaRefsException(Message);
        }

        public static void IfUsingMethod(string method, string wrappedMethod, Object context)
        {
            IfTypeHasMethod(method, $"It should instead use the wrapped method \"{wrappedMethod}\".", context);
        }

        private static void IfTypeHasMethod(string name, string message, Object context)
        {
            Type type = context.GetType();
            bool typeHasMethod = type.GetMethod(name, (System.Reflection.BindingFlags)(-1)) != null;
            if (typeHasMethod)
            {
                Debug.LogError($"\"{type}\" should not have the method \"{name}\". {message}", context);
            }
        }
    }
}