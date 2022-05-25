Feature: StringConcatenation

String concatenation

	Scenario: Two string variables can be concatenated
		Given the main function contains the following code:
			"""
			string banana = "banana";
			string cake = "cake";
			TEST("result", banana + cake);
			"""
		When the code is compiled
		Then there are no errors
		And "result" evaluates to "bananacake"

	Scenario: A string variable can be concatenated to a string literal
		Given the main function contains the following code:
			"""
			string banana = "banana";
			TEST("result", banana + "cake");
			"""
		When the code is compiled
		Then there are no errors
		And "result" evaluates to "bananacake"

	Scenario: A string variable can be concatenated to a string literal 2
		Given the main function contains the following code:
			"""
			string cake = "cake";
			TEST("result", "banana" + cake);
			"""
		When the code is compiled
		Then there are no errors
		And "result" evaluates to "bananacake"

	Scenario: Two string literals can be concatenated
		Given the main function contains the following code:
			"""
			TEST("result", "banana" + "cake");
			"""
		When the code is compiled
		Then there are no errors
		And "result" evaluates to "bananacake"