Feature: Functions

	Global functions

	Scenario: A function can be defined and invoked
		Given the following function is defined:
			"""
			void foo() {}
			"""
		And the main function contains the following code:
			"""
			foo();
			"""
		When the code is compiled
		Then there are no errors

	Scenario: A function can specify a return type
		Given the following function is defined:
			"""
			int foo() {
				return 1337;
			}
			"""
		And the main function contains the following code:
			"""
			TEST("foo", foo());
			"""
		When the code is compiled
		Then there are no errors
		And "foo" evaluates to 1337

	Scenario: A function can specify a parameter
		Given the following function is defined:
			"""
			void foo(int param1) {
				TEST("param1", param1);
			}
			"""
		And the main function contains the following code:
			"""
			foo(1234);
			"""
		When the code is compiled
		Then there are no errors
		And "param1" evaluates to 1234

	@Error
	Scenario: Calling a function with an incorrect argument type causes an error
		Given the following function is defined:
			"""
			void foo(int param1) {}
			"""
		And the main function contains the following code:
			"""
			foo("1234");
			"""
		When the code is compiled
		Then an error is returned
		And the error contains "No matching overload"
	
	Scenario: Functions can be overloaded
		Given the following function is defined:
			"""
			void foo(int param1) {}
			"""
		And the following function is defined:
			"""
			void foo(string param1) {
				TEST("param1", param1);
			}
			"""
		And the main function contains the following code:
			"""
			foo("1234");
			"""
		When the code is compiled
		Then there are no errors
		And "param1" evaluates to "1234"
