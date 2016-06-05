﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Rant.Internal.Engine.Delegates;
using Rant.Internal.Engine.Metadata;
using Rant.Internal.Engine.Syntax;
using Rant.Internal.IO;

namespace Rant.Internal.Engine
{
    /// <summary>
    /// Contains information for associating a delegate with a Rant function.
    /// </summary>
    internal class RantFunctionInfo : IRantFunction
    {
        private readonly Witchcraft _delegate;
        private readonly RantParameter[] _params;
        private readonly ParameterInfo[] _rawParams;
        private readonly MethodInfo _rawMethod;

        public RantParameter[] Parameters => _params;

        public ParameterInfo[] RawParameters => _rawParams;

        public string Name { get; }
        public string Description { get; }
        public bool HasParamArray { get; }

        public int ParamCount => _params.Length;

        public IEnumerable<IRantParameter> GetParameters() => _params;

        public bool TreatAsRichardFunction = false;
        public MethodInfo RawMethod => _rawMethod;

        public RantFunctionInfo(string name, string description, MethodInfo method)
        {
            // Sanity checks
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (!method.IsStatic) throw new ArgumentException($"({method.Name}) Method is not static.");

            _rawMethod = method;

            var parameters = method.GetParameters();
            if (!parameters.Any())
                throw new ArgumentException($"({method.Name}) Cannot use a parameter-less method for a function.");
            if (parameters[0].ParameterType != typeof(Sandbox))
                throw new ArgumentException($"({method.Name}) The first parameter must be of type '{typeof(Sandbox)}'.");

            // Sort out the parameter types for the function
            _params = new RantParameter[parameters.Length - 1];
            _rawParams = parameters;
            Type type;
            RantParameterType rantType;
            for (int i = 1; i < parameters.Length; i++)
            {
                // Resolve Rant parameter type from .NET type
                type = parameters[i].ParameterType;
                if (type.IsArray && i == parameters.Length - 1)
                    type = type.GetElementType();

                if (type == typeof(RantAction) || type.IsSubclassOf(typeof(RantAction)))
                {
                    rantType = RantParameterType.Pattern;
                }
                else if (type == typeof(string))
                {
                    rantType = RantParameterType.String;
                }
                else if (type.IsEnum)
                {
                    rantType = type.GetCustomAttributes(typeof(FlagsAttribute), false).Any()
                        ? RantParameterType.Flags
                        : RantParameterType.Mode;
                }
                else if (IOUtil.IsNumericType(type))
                {
                    rantType = RantParameterType.Number;
                }
                else if (type == typeof(ObjectModel.RantObject))
                {
                    rantType = RantParameterType.RantObject;
                }
                else
                {
                    throw new ArgumentException($"({method.Name}) Unsupported type '{type}' for parameter '{parameters[i].Name}'. Must be a string, number, or RantAction.");
                }

                // If there is a [RantDescription] attribute on the parameter, retrieve its value. Default to empty string if there isn't one.
                string paramDescription = (parameters[i].GetCustomAttributes(typeof(RantDescriptionAttribute), false).FirstOrDefault() as RantDescriptionAttribute)?.Description ?? "";

                // Create Rant parameter
                _params[i - 1] = new RantParameter(parameters[i].Name, type, rantType,
                    HasParamArray = (i == parameters.Length - 1 && parameters[i].GetCustomAttributes(typeof(ParamArrayAttribute), false).FirstOrDefault() != null))
                {
                    Description = paramDescription
                };
            }
            _delegate = Witchcraft.Create(method);
            Name = name;
            Description = description;
        }

        public IEnumerator<RantAction> Invoke(Sandbox sb, object[] arguments)
        {
            return _delegate.Invoke(sb, arguments) as IEnumerator<RantAction> ?? CreateEmptyIterator();
        }

        private static IEnumerator<RantAction> CreateEmptyIterator()
        {
            yield break;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('[').Append(Name);
            for (int i = 0; i < Parameters.Length; i++)
            {
                sb.Append(i == 0 ? ':' : ';').Append(' ');
                sb.Append(Parameters[i].Name);
                if (i == Parameters.Length - 1 && Parameters[i].IsParams)
                    sb.Append("...");
            }
            sb.Append(']');
            return sb.ToString();
        }
    }
}