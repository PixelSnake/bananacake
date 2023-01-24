Feature: Classes

Classes

Scenario: Class can be defined
	Given the following class is defined:
		"""
		class Foo {}
		"""
	And the main function is empty
	When the code is compiled
	Then there are no errors
	
Scenario: Class can be used
	Given the following class is defined:
		"""
		class Foo {}
		"""
	And the main function contains the following code:
		"""
		Foo foo = new Foo();
		"""
	When the code is compiled
	Then there are no errors
	
Scenario: Class can have member variables and functions
	Given the following class is defined:
		"""
		class Foo {
			public int bar;

			public void nop() {}
		}
		"""
	And the main function contains the following code:
		"""
		Foo foo = new Foo();
		"""
	When the code is compiled
	Then there are no errors

Scenario: Class without explicit constructor definition is automatically assigned empty constructor
	Given the following class is defined:
		"""
		class Foo {}
		"""
	And the main function contains the following code:
		"""
		Foo foo = new Foo();
		"""
	When the code is compiled
	Then there are no errors

Scenario: Constructors can have parameters
	Given the following class is defined:
		"""
		class Foo {
			public int bar;

			public Foo(int _bar) {
				bar = _bar;
			}
		}
		"""
	And the main function contains the following code:
		"""
		Foo foo = new Foo(1337);
		TEST("foo.bar", foo.bar);
		"""
	When the code is compiled
	Then there are no errors
	And "foo.bar" evaluates to 1337

Scenario: Constructor parameters can be initializers
	Given the following class is defined:
		"""
		class Foo {
			public int bar;

			public Foo(this.bar) {}
		}
		"""
	And the main function contains the following code:
		"""
		Foo foo = new Foo(1337);
		TEST("foo.bar", foo.bar);
		"""
	When the code is compiled
	Then there are no errors
	And "foo.bar" evaluates to 1337

Scenario: Constructor method bodys can be omitted
	Given the following class is defined:
		"""
		class Foo {
			public int value;

			public Foo(this.value);
		}
		"""
	And the main function contains the following code:
		"""
		Foo foo = new Foo(1234);
		TEST("foo.value", foo.value);
		"""
	When the code is compiled
	Then there are no errors
	And "foo.value" evaluates to 1234