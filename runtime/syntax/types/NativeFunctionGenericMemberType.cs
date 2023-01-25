using BCake.Parser.Syntax.Scopes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCake.Parser.Syntax.Types
{
    public abstract class NativeFunctionGenericMemberType : NativeFunctionType
    {
        protected NativeFunctionGenericMemberType(Scope scope, Type parent, Type returnType, string name, ParameterType[] parameters)
            : base(scope, parent, returnType, name, parameters)
        { }

        protected NativeFunctionGenericMemberType(Scope scope, Type returnType, string name, ParameterType[] parameters, NativeFunctionType[] overloads)
            : base(scope, returnType, name, parameters, overloads)
        { }

        public abstract override FunctionType MakeConcrete(ConcreteClassType parent, Type concreteReturnType, ParameterType[] concreteParameters);
    }
}
