Feature: Operator Access

Scenario: Can access member variables of classes
	Given the following class is defined:
		"""
		class Foo {
			public int bar;

			public Foo(this.bar);
		}
		"""
	And the main function contains the following code:
		"""
		Foo foo = new Foo(1337);
		int xyz = foo.bar;
		TEST("xyz", xyz);
		"""
	When the code is compiled
	Then there are no errors
	And "xyz" evaluates to 1337

Scenario: Can access member functions of classes
	Given the following class is defined:
		"""
		class Foo {
			public int bar() {
				return 1337;
			}
		}
		"""
	And the main function contains the following code:
		"""
		Foo foo = new Foo();
		int xyz = foo.bar();
		TEST("xyz", xyz);
		"""
	When the code is compiled
	Then there are no errors
	And "xyz" evaluates to 1337
	
Scenario: Can access member variables of scopes
	Given the following class is defined:
		"""
		class Foo {
			public int bar;

			public Foo(this.bar);
		}
		"""
	And the main function contains the following code:
		"""
		int xyz = new Foo(1337).bar;
		TEST("xyz", xyz);
		"""
	When the code is compiled
	Then there are no errors
	And "xyz" evaluates to 1337

Scenario: Can access member functions of scopes
	Given the following class is defined:
		"""
		class Foo {
			public int bar() {
				return 1337;
			}
		}
		"""
	And the main function contains the following code:
		"""
		int xyz = new Foo().bar();
		TEST("xyz", xyz);
		"""
	When the code is compiled
	Then there are no errors
	And "xyz" evaluates to 1337