using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BCake.Parser.Exceptions;
using BCake.Parser.Syntax.Expressions.Nodes;

using BCake.Runtime;
using BCake.Runtime.Nodes.Value;

namespace BCake.Parser.Syntax.Types {
    public abstract class NativeFunctionType : FunctionType {
        protected NativeFunctionType(Scopes.Scope scope, Type returnType, string name, ParameterType[] parameters)
            : base(scope, returnType, name, parameters)
        {
            DefiningToken = Token.NativeCode();
        }

        protected NativeFunctionType(Scopes.Scope scope, Type returnType, string name, ParameterType[] parameters, NativeFunctionType[] overloads)
            : base(scope, returnType, name, parameters)
        {
            DefiningToken = Token.NativeCode();
            Overloads = overloads;
        }

        protected NativeFunctionType(Scopes.Scope scope, Type parent, Type returnType, string name,
            ParameterType[] parameters)
            : base(scope, parent, returnType, name, parameters)
        {
            DefiningToken = Token.NativeCode();
        }

        public override void ParseInner() {}

        public abstract RuntimeValueNode Evaluate(RuntimeScope scope, RuntimeValueNode[] arguments);
    }
}