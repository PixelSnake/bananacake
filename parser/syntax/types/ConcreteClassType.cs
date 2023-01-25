using System.Linq;

namespace BCake.Parser.Syntax.Types {
    public class ConcreteClassType : GenericClassType {
        Types.Type[] TypeArguments;
        public Types.GenericClassType GenericType { get; protected set; }

        /// <summary>
        /// Precondition: GuardTypeArguments must have been called.
        /// We can't call it from here, because we don't know where this type has
        /// been used, which is where the error must be thrown,
        /// </summary>
        public ConcreteClassType(GenericClassType genericType, Type[] typeArguments)
            : base(genericType.Scope, genericType.Access, genericType.Name, genericType.DefiningToken, genericType.GenericTypeParameters) {
            GenericType = genericType;
            TypeArguments = typeArguments;
            Name = $"{ GenericName }<{ string.Join(", ", typeArguments.Select(t => t.Name)) }>";

            declareMembers();
        }

        public Types.Type GetGenericTypeParameter(string name) {
            return GenericTypeParameters
                .Select((parameter, index) => new { parameter, index })
                .Where(pair => pair.parameter.Name == name)
                .Select(pair => TypeArguments[pair.index])
                .FirstOrDefault();
        }

        /// <summary>
        /// Copies members from the generic base class to the concrete class,
        /// replacing generic types with concrete types.
        /// </summary>
        private void declareMembers() {
            for (int i = 0; i < TypeArguments.Length; i++)
                Scope.Replace(TypeArguments[i], GenericType.GenericTypeParameters[i].Name);

            foreach (var keyValuePair in GenericType.Scope.AllMembers.Where(p => p.Value is MemberVariableType)) {
                var name = keyValuePair.Key;
                var type = keyValuePair.Value;
                var member = type as MemberVariableType;

                if (member.Name == "this") continue;

                Scope.Declare(new MemberVariableType(
                    member.DefiningToken,
                    this,
                    member.Access,
                    member.Type is PlaceholderType ? GetGenericTypeParameter(member.Type.Name) : member.Type,
                    member.Name
                ));
            }

            foreach (var keyValuePair in GenericType.Scope.AllMembers.Where(p => p.Value is FunctionType))
            {
                var name = keyValuePair.Key;
                var type = keyValuePair.Value;
                var function = type as FunctionType;
                var newFunction = function.MakeConcrete(this, makeConcreteType(function.ReturnType), function.Parameters.Select(declareParameterType).ToArray());
                newFunction.ParseInner();
                Scope.Declare(newFunction);
            }
        }

        /// <summary>
        /// Declares parameter types within the scope of the concrete type. Replaces all placeholder types with concrete types.
        /// Takes care of initialized parameters, as these need to be dealt with specifically.
        /// </summary>
        /// <exception cref="Exceptions.InvalidParameterPropertyInitializerException">In the case that the member which an initializer parameter refers to has not been declared</exception>
        private FunctionType.ParameterType declareParameterType(FunctionType.ParameterType param)
        {
            if (param is FunctionType.InitializerParameterType initParam)
            {
                return new FunctionType.InitializerParameterType(
                    initParam.DefiningToken,
                    Scope.GetSymbol(initParam.Name, true) as MemberVariableType ?? throw new Exceptions.InvalidParameterPropertyInitializerException(initParam)
                );
            } else
            {
                return new FunctionType.ParameterType(
                    param.DefiningToken,
                    param.Type is PlaceholderType ? GetGenericTypeParameter(param.Type.Name) : param.Type,
                    param.Name
                );
            }
        }

        private Type makeConcreteType(Type type)
        {
            switch (type)
            {
                case GenericClassType genericClassType:
                    if (genericClassType == GenericType)
                        return this;
                    break;
            }

            return type;
        }

        public static void GuardTypeArguments(Token token, GenericClassType genericType, Types.Type[] typeArguments) {
            var error = new Exceptions.InvalidTypeArgumentListException(token, genericType, typeArguments);

            if (genericType.GenericTypeParameters.Length != typeArguments.Length)
                throw error;
            // for (int i = 0; i < genericType.GenericTypeParameters.Length; i++) {
            //     var param = genericType.GenericTypeParameters[i];
            //     var arg = typeArguments[i];

            //     // Future: Check type compatibilities
            // }
        }

        public override FunctionType GenerateDefaultConstructor()
        {
            var function = new FunctionType(DefiningToken, this, Access.@public, this, "!constructor", new FunctionType.ParameterType[] { }, new Token[] { });
            Scope.Declare(function);
            return function;
        }

        public static bool operator ==(ConcreteClassType a, ConcreteClassType b) => Equals(a, b);
        public static bool operator !=(ConcreteClassType a, ConcreteClassType b) => !Equals(a, b);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as ConcreteClassType;
            if (other == null) return false;

            if (other.GenericType != GenericType) return false;
            if (other.TypeArguments.Length != TypeArguments.Length) return false;

            for (int i = 0; i < TypeArguments.Length; i++)
                if (TypeArguments[i] != other.TypeArguments[i])
                    return false;

            return true;
        }
    }
}