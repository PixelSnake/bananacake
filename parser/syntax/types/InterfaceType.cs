using BCake.Parser.Errors;
using BCake.Parser.Exceptions;
using BCake.Parser.Syntax.Scopes;
using BCake.Parser.Validators;
using System.Collections.Generic;

namespace BCake.Parser.Syntax.Types
{
    public class InterfaceType : InheritableType
    {
        public InterfaceType(Token definingToken, Scope scope, string name, Token[] tokens) : base(null, name, Access.@public)
        {
            DefiningToken = definingToken;
            Tokens = tokens;
            Scope = new Scope(scope, this);
        }

        public override void ParseInner()
        {
            var validator = new TokenTypeValidator(TokenType.function, TokenType.cast);
            Parser.ParseTypes(Scope, Tokens, validator);
        }

        /// <summary>
        /// Validates if all methods of an interface have been implemented.
        /// </summary>
        public static IEnumerable<Result> FulfillsInheritanceConstraints(InterfaceType interf, InheritableType derivedType)
        {
            var result = true;

            foreach (var (name, prototype) in interf.Scope.AllMembers)
            {
                Type member;
                if ((member = derivedType.Scope.GetSymbol(name)) != null)
                {
                    if (prototype is FunctionType)
                    {
                        if (member is not FunctionType)
                        {
                            result = false;
                            yield return new OverrideMemberTypeMismatchError(prototype, member, member.DefiningToken);
                        }
                        else
                        {
                            var res = FulfillsFunctionOverride(prototype as FunctionType, member as FunctionType);
                            if (res.HasErrors())
                            {
                                if (!res.Value<bool>())
                                    result = false;

                                foreach (var error in res.Errors())
                                    yield return error;
                            }
                                
                        }
                    }
                }
                else
                {
                    result = false;
                    yield return new OverrideMissingError(prototype, derivedType, derivedType.DefiningToken);
                }
            }

            yield return new ResultValue<bool>(result);
        }

        private static IEnumerable<Result> FulfillsFunctionOverride(FunctionType prototypeFunc, FunctionType overrideFunc)
        {
            var result = true;

            if (prototypeFunc.ReturnType != overrideFunc.ReturnType)
            {
                result = false;
                yield return new OverrideFunctionReturnTypeMismatchError(prototypeFunc, overrideFunc, overrideFunc.DefiningToken);
            }
            if (overrideFunc.ParameterListDiffers(prototypeFunc))
            {
                result = false;
                yield return new OverrideFunctionParameterListMismatchError(prototypeFunc, overrideFunc, overrideFunc.DefiningToken);
            }

            yield return new ResultValue<bool>(result);
        }
    }
}
