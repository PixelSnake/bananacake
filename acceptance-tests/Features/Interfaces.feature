Feature: Interfaces

	Interfaces can be used as a placeholder for types, only describing (parts of) the available member variables and functions of the type.

	Scenario: An interface can be defined
		Given the following interface is defined:
			"""
			interface HasToString { }
			"""
		And the main function is empty
		When the code is compiled
		Then there are no errors

	Scenario: An interface can be implemented
		Given the following interface is defined:
			"""
			interface HasToString { }
			"""
		And the following class is defined:
			"""
			class Foo : HasToString { }
			"""
		And the main function is empty
		When the code is compiled
		Then there are no errors

	@Error
	Scenario: Implementing an interface and not overriding all members causes an error
		Given the following interface is defined:
			"""
			interface HasToString {
				public void foo();
			}
			"""
		And the following class is defined:
			"""
			class Foo : HasToString { }
			"""
		And the main function is empty
		When the code is compiled
		Then an error is returned
		And the error contains "Missing override"

	@Error
	Scenario: Overriding a function with the wrong return type causes an error
		Given the following interface is defined:
			"""
			interface HasToString {
				public void foo();
			}
			"""
		And the following class is defined:
			"""
			class Foo : HasToString {
				public int foo() {}
			}
			"""
		And the main function is empty
		When the code is compiled
		Then an error is returned
		And the error contains "Override return type mismatch"

	@Error
	Scenario: Overriding a member with the wrong member type causes an error
		Given the following interface is defined:
			"""
			interface HasToString {
				public void foo();
			}
			"""
		And the following class is defined:
			"""
			class Foo : HasToString {
				public int foo;
			}
			"""
		And the main function is empty
		When the code is compiled
		Then an error is returned
		And the error contains "Override member type mismatch"

	@Error
	Scenario: Overriding a function with different parameter types causes an error
		Given the following interface is defined:
			"""
			interface HasToString {
				public void foo();
			}
			"""
		And the following class is defined:
			"""
			class Foo : HasToString {
				public void foo(string bar) {}
			}
			"""
		And the main function is empty
		When the code is compiled
		Then an error is returned
		And the error contains "Override function signature mismatch"

	Scenario: Overriding a function correctly works
		Given the following interface is defined:
			"""
			interface HasToString {
				public void foo();
			}
			"""
		And the following class is defined:
			"""
			class Foo : HasToString {
				public void foo() {}
			}
			"""
		And the main function is empty
		When the code is compiled
		Then there are no errors

	@Error
	Scenario: Instantiating an interface causes an error
		Given the following interface is defined:
			"""
			interface HasToString {
				public void foo();
			}
			"""
		And the main function contains the following code:
			"""
			new HasToString();
			"""
		When the code is compiled
		Then an error is returned
		And the error contains "Invalid constructor call"

	Scenario: Overriding more specific parameter and return types works
		Given the following interface is defined:
			"""
			interface IPlusOp {
				public IPlusOp operator_plus(IPlusOp other);
			}
			"""
		And the following class is defined:
			"""
			class Foo {
				public int x;

				public Foo(this.x) {}

				public Foo operator_plus(Foo other) {
					return new Foo(x + other.x);
				}
			}
			"""
		And the following function is defined:
			"""
			IPlusOp add(IPlusOp a, IPlusOp b) {
				return a + b;
			}
			"""
		And the main function contains the following code:
			"""
			Foo f1 = new Foo(10);
			Foo f2 = new Foo(25);
			IPlusOp sum = add(f1, f2);
			"""
		When the code is compiled
		Then there are no errors

	@Error
	Scenario: Fail on using less specific type as function parameter
		Given the following interface is defined:
			"""
			interface IPlusOp {
				public IPlusOp operator_plus(IPlusOp other);
			}
			"""
		And the following class is defined:
			"""
			class Foo : IPlusOp {
				public int x;

				public Foo(this.x) {}

				public Foo operator_plus(Foo other) {
					return new Foo(x + other.x);
				}
			}
			"""
		And the following function is defined:
			"""
			Foo add(Foo a, Foo b) {
				return a + b;
			}
			"""
		And the main function contains the following code:
			"""
			Foo f1 = new Foo(10);
			Foo f2 = new Foo(25);
			IPlusOp op1 = new Foo(234);
			IPlusOp sum = add(f1, op1);
			"""
		When the code is compiled
		Then an error is returned
		And the error contains "No matching overload"
