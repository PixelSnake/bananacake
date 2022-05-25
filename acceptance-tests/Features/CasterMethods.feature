Feature: Caster methods

Can be defined as member methods to convert a class type to another type

Scenario: A class can define a caster method
	Given the following class is defined:
		"""
		class Foo {
			public int foo;

			public Foo(this.foo) {}
		}
		"""
	And the following class is defined:
		"""
		class Bar {
			public int bar;

			public Bar(this.bar) {}

			public cast Foo() {
				return new Foo(bar);
			}
		}
		"""
	And the main function contains the following code:
		"""
		Bar bar = new Bar(1234);
		Foo foo = bar as Foo;
		TEST("foo.foo", foo.foo);
		"""
	When the code is compiled
	Then there are no errors
	And "foo.foo" evaluates to 1234
