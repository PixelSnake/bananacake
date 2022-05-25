using System.Linq;
using System.Collections;
using System.Collections.Generic;
using BCake.Parser.Syntax.Types;
using BCake.Parser.Exceptions;

namespace BCake.Parser.Syntax.Scopes {
    public class Scope {
        public static int ScopeCount { get; protected set; }
        public int Id { get; protected set; }
        public Type Type { get; protected set; }
        public Scope Parent { get; protected set; }
        public string FullName {
            get {
                if (Id == 0) return null;

                var name = Parent?.FullName ?? "";
                if (Type?.Name == null) return name;
                if (name.Length < 1) name = Type?.Name;
                else name += "." + Type?.Name;
                return name;
            }
        }
        private Dictionary<string, Type> MembersByName = new Dictionary<string, Type>();
        public IEnumerable<KeyValuePair<string, Type>> AllMembers {
            get {
                foreach (var pair in MembersByName) {
                    yield return pair;
                }
            }
        }

        public Scope() {
            Id = ScopeCount++;
        }
        public Scope(Scope parent) {
            Id = ScopeCount++;
            Parent = parent;
        }
        public Scope(Scope parent, Type type) {
            Id = ScopeCount++;
            Parent = parent;
            Type = type;
        }

        public void Declare(params Type[] members) {
            foreach (var m in members) Declare(m);
        }
        public void Declare(Type m) {
            Declare(m, m.Name);
        }
        public void Declare(Type m, string nameOverride) {
            if (MembersByName.ContainsKey(nameOverride)) { // if a symbol with the same name is already defined
                if (m is FunctionType && MembersByName[nameOverride] is FunctionType) { // if both are functions
                    var thisFunc = m as FunctionType;
                    var otherFunc = MembersByName[nameOverride] as FunctionType;

                    if (thisFunc.ParameterListDiffers(otherFunc)) { // the name is equal but the parameters are different, so this is an overload
                        // add the overload and return
                        otherFunc.Overloads = otherFunc.Overloads.Append(thisFunc).ToArray();
                        return;
                    }

                    throw new DuplicateFunctionDeclarationException(m.DefiningToken, otherFunc);
                }

                throw new DuplicateDeclarationException(m.DefiningToken, MembersByName[nameOverride]);
            }

            MembersByName.Add(nameOverride, m);
        }

        public Type GetSymbol(string name, bool localOnly = false) {
            if (!MembersByName.ContainsKey(name)) {
                if (!localOnly) return Parent?.GetSymbol(name);
                else return null;
            }
            return MembersByName[name]; 
        }

        public void Replace(Type m, string name) {
            if (MembersByName.ContainsKey(name)) {
                MembersByName[name] = m;
            } else {
                throw new System.Exception($"Type { name } cannot be replaced because it has not been declared");
            }
        }

        public FunctionType GetClosestFunction() {
            if (Type is FunctionType) return Type as FunctionType;
            else return Parent?.GetClosestFunction();
        }

        public ClassType GetClosestType() {
            switch (Type) {
                case ClassType t: return t;
                default: return Parent?.GetClosestType();
            }
        }

        public bool IsChildOf(Scope other) {
            for (var s = this; s.Parent != null; s = s.Parent) {
                if (s == other) return true;
            }
            return false;
        }
    }
}