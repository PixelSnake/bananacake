using System.Linq;
using System.Collections;
using System.Collections.Generic;
using BCake.Parser.Exceptions;
using BCake.Parser.Syntax.Types;
using BCake.Parser.Errors;

namespace BCake.Parser.Syntax.Expressions.Nodes.Operators {
    public abstract class Operator : Node {
        /// <summary>
        /// a list of the names that can be used to overload operators for classes
        /// </summary>
        private bool needsLValue, needsRValue, leftNeedsNone, rightNeedsNone, checkReturnTypes;
        OperatorAttribute.ParameterType typeLeft, typeRight;
        public Expression Left {
            get {
                return _left;
            }
            protected set {
                _left = value;
                if (value == null) return;

                Node lvalue = value.Root;
                if (value.Root is OperatorAccess) {
                    lvalue = new SymbolNode(
                        value.Root.DefiningToken,
                        (value.Root as OperatorAccess).ReturnSymbol
                    );
                }

                if (needsLValue && !(lvalue is ILValue))
                    throw new Exceptions.InvalidArgumentException(value.DefiningToken, typeLeft);
                if (leftNeedsNone && (lvalue is ILValue || lvalue is IRValue))
                    throw new Exceptions.InvalidArgumentException(value.DefiningToken, typeLeft);

                CheckReturnTypes(value);
            }
        }
        private Expression _left;
        public Expression Right {
            get {
                return _right;
            }
            protected set {
                _right = value;
                if (value == null) return;

                if (needsRValue && !(value.Root is IRValue))
                    throw new Exceptions.InvalidArgumentException(value.DefiningToken, typeRight);
                if (rightNeedsNone && (value.Root is ILValue || value.Root is IRValue))
                    throw new Exceptions.InvalidArgumentException(value.DefiningToken, typeRight);

                CheckReturnTypes(value);
            }
        }
        private Expression _right;

        public override Types.Type ReturnType {
            // it does not matter which one we return because they have to be equal
            get => Left?.ReturnType ?? Right?.ReturnType;
        }

        // null is passed to parent class as token parameter because it is set in the parse method
        // this is the case, because the constructor has to be parameterless
        public Operator() : base(null) {
            var operatorAttr = (OperatorAttribute)this.GetType().GetCustomAttributes(typeof(OperatorAttribute), true).FirstOrDefault();
            if (operatorAttr == null) return;

            typeLeft = operatorAttr.Left;
            typeRight = operatorAttr.Right;
            needsLValue = typeLeft == OperatorAttribute.ParameterType.LValue;
            needsRValue = typeRight == OperatorAttribute.ParameterType.RValue;
            leftNeedsNone = typeLeft == OperatorAttribute.ParameterType.None;
            rightNeedsNone = typeRight == OperatorAttribute.ParameterType.None;
            checkReturnTypes = operatorAttr.CheckReturnTypes;
        }

        public virtual IEnumerable<Result> OnCreated(Token token, Scopes.Scope scope)
        {
            yield return Result.True;
        }

        public static OperatorAttribute GetOperatorMetadata<T>() {
            return GetOperatorMetadata(typeof(T));
        }
        public static OperatorAttribute GetOperatorMetadata(System.Type t) {
            var opSymbolAttr = t.GetCustomAttributes(
                typeof(OperatorAttribute),
                true
            ).FirstOrDefault() as OperatorAttribute;
            return opSymbolAttr;
        }

        public static Node Parse(Scopes.Scope scope, Scopes.Scope typeSource, System.Type opType, Token token, Expression left, Token[] right)
        {
            var op = (Operator)System.Activator.CreateInstance(opType);
            op.DefiningToken = token;
            op.Left = left;
            op.Right = op.ParseRight(scope, right, op.Left?.ReturnType?.Scope);

            op.OnCreated(token, scope).Throw();

            return op;
        }

        public static Node Parse(Scopes.Scope scope, Scopes.Scope typeSource, System.Type opType, Token token, Token[] left, Token[] right) {
            var op = (Operator)System.Activator.CreateInstance(opType);
            op.DefiningToken = token;
            op.Left = op.ParseLeft(scope, left, typeSource);
            op.Right = op.ParseRight(scope, right, op.Left?.ReturnType?.Scope);
            
            op.OnCreated(token, scope).Throw();
            
            return op;
        }

        protected virtual Expression ParseLeft(Scopes.Scope scope, Token[] tokens, Scopes.Scope typeSource) {
            return Expression.Parse(scope, tokens);
        }

        protected virtual Expression ParseRight(Scopes.Scope scope, Token[] tokens, Scopes.Scope typeSource) {
            return Expression.Parse(scope, tokens);
        }

        protected void CheckRightReturnType(Type type) {
            if (Right != null && Right.ReturnType != type) throw new TypeException(Right.DefiningToken, Right.ReturnType, type);
        }
        protected void CheckReturnTypes(Expression e) {
            if (!checkReturnTypes) return;

            var other = e == Right ? Left : Right;
            if (Right == null || Left == null) return;
            if (Right.ReturnType != Left.ReturnType) {
                var opMeta = GetOperatorMetadata(GetType());
                if (opMeta.TypeSlope == OperatorAttribute.TypeSlopeDirection.ToLeft)
                {
                    if (Left.ReturnType is InheritableType leftInheritable && Right.ReturnType is InheritableType rightInheritable) {
                        if (rightInheritable.IsDescendantOf(leftInheritable)) return;
                    }
                }

                if (Right.ReturnType != null && Left.ReturnType != null) {
                    if (Right.ReturnType.Scope.GetSymbol($"!as_{ Left.ReturnType.Name }") != null) {
                        throw new TypeException(Right.DefiningToken, Right.ReturnType, Left.ReturnType, Left.ReturnType);
                    } else if (Left.ReturnType.Scope.GetSymbol($"!as_{ Right.ReturnType.Name }") != null) {
                        throw new TypeException(Left.DefiningToken, Left.ReturnType, Right.ReturnType, Right.ReturnType);
                    }
                }

                throw new TypeException(DefiningToken, e.ReturnType, other.ReturnType);
            }
        }
    }
}