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
            yield return ResultSense.FalseDominates;

            foreach (var (name, prototype) in interf.Scope.AllMembers)
            {
                Type member;
                if ((member = derivedType.Scope.GetSymbol(name)) != null)
                {
                    if (prototype is FunctionType)
                    {
                        if (member is not FunctionType)
                        {
                            yield return Result.False;
                            yield return new OverrideMemberTypeMismatchError(prototype, member, member.DefiningToken);
                        }
                        else
                        {
                            var res = FulfillsFunctionOverride(prototype as FunctionType, member as FunctionType);
                            if (res.HasErrors())
                            {
                                if (!res.BoolValue())
                                    yield return Result.False;

                                foreach (var error in res.Errors())
                                    yield return error;
                            }
                        }
                    }
                }
                else
                {
                    yield return Result.False;
                    yield return new OverrideMissingError(prototype, derivedType, derivedType.DefiningToken);
                }
            }
        }

        private static IEnumerable<Result> FulfillsFunctionOverride(FunctionType prototypeFunc, FunctionType overrideFunc)
        {
            yield return ResultSense.FalseDominates;

            if (prototypeFunc.ReturnType != overrideFunc.ReturnType)
            {
                if (prototypeFunc.ReturnType is InheritableType protoInhType
                    && overrideFunc.ReturnType is InheritableType overrideInhType
                    && overrideInhType.IsDescendantOf(protoInhType))
                {
                    yield return Result.True;
                }
                else
                {
                    yield return Result.False;
                    yield return new OverrideFunctionReturnTypeMismatchError(prototypeFunc, overrideFunc, overrideFunc.DefiningToken);
                }
            }
            if (prototypeFunc.ParameterListDiffers(overrideFunc))
            {
                yield return Result.False;
                yield return new OverrideFunctionParameterListMismatchError(prototypeFunc, overrideFunc, overrideFunc.DefiningToken);
            }
        }
    }
}
