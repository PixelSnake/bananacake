using System;

namespace BCake.Parser.Syntax.Expressions.Nodes.Operators {
    public class OperatorAttribute : Attribute {
        public string Symbol, Closer;
        public ParameterType Left, Right;
        public EvaluationDirection Direction;
        public TypeSlopeDirection TypeSlope;
        public bool SuppressErrors = false;
        public bool CheckReturnTypes = true;
        public string OverloadableName = null;


        public enum ParameterType {
            RValue = 0,
            LValue,
            None
        }

        public enum EvaluationDirection {
            LeftToRight = 0,
            RightToLeft
        }

        /// <summary>
        /// The direction in which the specificity of the return types of left and right side of an operator can decrease.
        /// Example: Left side is of type Human, right side of type Woman. If the type slope is falling towards the left,
        /// then the more specific type of Woman can be combined with the less specific type of Human to the left.
        /// </summary>
        public enum TypeSlopeDirection
        {
            None = 0,
            /// <summary>
            /// Type on the left hand side may be less specific
            /// </summary>
            ToLeft,
            /// <summary>
            /// Type on the right hand side may be less specific
            /// </summary>
            /// Not yet implemented
            //ToRight
        }
    }

    public class OperatorParsePreflight : Attribute {}
}