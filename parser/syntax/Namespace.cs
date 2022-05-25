using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using BCake.Parser.Validators;
using BCake.Parser.Exceptions;
using BCake.Parser.Syntax.Types;
using BCake.Parser.Syntax.Expressions.Nodes;
using BCake.Parser.Syntax.Expressions.Nodes.Value;
//using BCake.Parser.Syntax.Types.Native.Std;

namespace BCake.Parser.Syntax {
    public class Namespace : ComplexType {
        public static Namespace Global { get; protected set; }

        public Namespace() : base(null, "public") {
            Scope = new Scopes.Scope(null, this);
            Global = this;

            InitPrimitives();
            InitNativeFunctions();
            InitNativeTypes();
        }
        public Namespace(Scopes.Scope parent, Access access, string name, Token[] tokens)
            : base(parent, name, access) {
            Access = access;
            Name = name;
            this.Tokens = tokens;

            Scope = new Scopes.Scope(parent, this);
        }

        public override void ParseInner() {
            Parser.ParseTypes(Scope, Tokens, new TokenTypeValidator(TokenType.@class, TokenType.@function).WithMemberRules(Parser.RootRules));
        }

        private void InitPrimitives() {
            Scope.Declare(
                StringValueNode.Type,
                IntValueNode.Type,
                BoolValueNode.Type
            );

            //IntValueNode.Type.Scope.Declare(IntToStringCast.Implementation);
        }

        private void InitNativeFunctions() {
            //Scope.Declare(
            //    Print.Implementation,
            //    Println.Implementation
            //);

            //StringValueNode.Type.Scope.Declare(
            //    Types.Native.String.Operators.StringOperatorPlus.Implementation
            //);

            //IntValueNode.Type.Scope.Declare(
            //    Types.Native.Integer.Operators.IntegerOperatorPlus.Implementation
            //);
        }

        private void InitNativeTypes() {
            //Scope.Declare(Types.Native.Std.Array.Array.Implementation);
        }
    }
}