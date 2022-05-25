using System.Collections.Generic;
using BCake.Parser.Syntax;
using BCake.Parser.Syntax.Expressions.Nodes;
using BCake.Parser.Syntax.Scopes;
using BCake.Parser.Syntax.Types;
using BCake.Runtime.Nodes.Value;

namespace BCake.Runtime {
    public class RuntimeScope {
        public RuntimeScope Parent { get; protected set; }
        public Scope Scope { get; protected set; }

        protected Dictionary<string, RuntimeValueNode> Values = new Dictionary<string, RuntimeValueNode>();
        protected static Dictionary<Scope, RuntimeScope> RuntimeScopeForScope = new Dictionary<Scope, RuntimeScope>();

        public RuntimeScope(RuntimeScope parent, Scope scope) : this(parent, scope, false) {

        }
        public RuntimeScope(RuntimeScope parent, Scope scope, bool lateInit) {
            Parent = parent;
            Scope = scope;

            if (scope != null && !RuntimeScopeForScope.ContainsKey(scope))
                RuntimeScopeForScope.Add(scope, this);

            if (lateInit) return;
            Init(scope);
        }

        public void Init(Scope scope) {
            foreach (var member in scope.AllMembers) {
                switch (member.Value) {
                    case FunctionType f: {
                            var val = new RuntimeFunctionValueNode(f, this);
                            Values.Add(member.Key, val);
                            break;
                        }
                    case Namespace n: {
                            var val = new RuntimeNamespaceValueNode(n, Resolve(n.Scope));
                            Values.Add(member.Key, val);
                            break;
                        }
                    default: Values.Add(member.Key, null); break;
                }
            }
        }

        public RuntimeValueNode GetValue(string symbol) {
            if (!Values.ContainsKey(symbol)) return Parent.GetValue(symbol);
            // we don't need to check because an invalid access should fail at parse time
            return Values[symbol];
        }

        public RuntimeValueNode GetValueHere(string symbol) {
            // we don't need to check because an invalid access should fail at parse time
            return Values[symbol];
        }

        public bool SetValue(string symbol, RuntimeValueNode value) {
            if (!Values.ContainsKey(symbol)) return Parent?.SetValue(symbol, value) == true;

            Values[symbol] = value;
            return true;
        }

        public static RuntimeScope Resolve(Scope scope) {
            if (!RuntimeScopeForScope.ContainsKey(scope)) {
                if (scope?.Parent == null) throw new Exceptions.RuntimeException("Could not resolve runtime scope", new Parser.Token());
                return Resolve(scope.Parent);
            }
            return RuntimeScopeForScope[scope];
        }

        public static SymbolNode ResolveSymbolNode(SymbolNode n) {
            if (n.Symbol is CompositeType) return ResolveCompositeType(n.Symbol as CompositeType);
            else return n;
        }

        private static SymbolNode ResolveCompositeType(CompositeType t) {
            var symbol = t.OperatorAccess.SymbolToAccess;
            var scope = Resolve(symbol.Scope);

            var symbolValue = scope.GetValue(symbol.Name);
            if (symbolValue == null) throw new Exceptions.NullReferenceException(symbol, t.OperatorAccess.DefiningToken);

            if (!(symbolValue is RuntimeClassInstanceValueNode)) {
                throw new Exceptions.RuntimeException("Cannot access properties on non-class types", t.OperatorAccess.DefiningToken);
            }

            var instance = symbolValue as RuntimeClassInstanceValueNode;
            // var result = instance.AccessMember(t.OperatorAccess.Right.Root)

            // return new SymbolNode(
            //     actualType.DefiningToken,
            //     actualType
            // );
            return null;
        }
    }
}