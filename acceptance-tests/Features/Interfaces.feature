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
