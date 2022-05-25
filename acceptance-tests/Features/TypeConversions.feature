Feature: TypeConversions

	Scenario: Instances of a more specific type can be assigned to a variable of a less specific type
		Given the following interface is defined:
			"""
			interface HasToString {}
			"""
		And the following class is defined:
			"""
			class Foo : HasToString {}
			"""
		And the main function contains the following code:
			"""
			HasToString h = new Foo();
			"""
		When the code is compiled
		Then there are no errors

	@Error
	Scenario: Instances of a less specific type cannot be assigned to a variable of a more specific type
		Given the following interface is defined:
			"""
			interface HasToString {}
			"""
		And the following class is defined:
			"""
			class Foo : HasToString {}
			"""
		And the main function contains the following code:
			"""
			HasToString h;
			Foo f = h;
			"""
		When the code is compiled
		Then an error is returned
		And the error contains "cannot be implicitly converted"

	Scenario: Less specific types can be used as parameter types
		Given the following interface is defined:
			"""
			interface HasToString {
				public cast string();
			}
			"""
		And the following class is defined:
			"""
			class Foo : HasToString {
				public cast string() {
					return "test";
				}
			}
			"""
		And the following function is defined:
			"""
			void print2(HasToString h) {
				TEST("h as string", h as string);
			}
			"""
		And the main function contains the following code:
			"""
			Foo f = new Foo();
			print2(f);
			"""
		When the code is compiled
		Then there are no errors
		And "h as string" evaluates to "test"

	Scenario: Types implicitly implementing interfaces can be used as such
		Given the following interface is defined:
			"""
			interface HasToString {
				public cast string();
			}
			"""
		And the following class is defined:
			"""
			class Foo {
				public cast string() {
					return "test";
				}
			}
			"""
		And the following function is defined:
			"""
			void print2(HasToString h) {
				TEST("h as string", h as string);
			}
			"""
		And the main function contains the following code:
			"""
			Foo f = new Foo();
			print2(f);
			"""
		When the code is compiled
		Then there are no errors
		And "h as string" evaluates to "test"
