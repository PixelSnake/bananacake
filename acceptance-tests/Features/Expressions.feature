Feature: Expressions

Expressions

Scenario: Expressions can be grouped with parentheses
	Given the main function contains the following code:
		"""
		TEST("result", 3 * (3 + 3) / 3 * 3 / ((3 * 3) / 3));
		"""
	When the code is compiled
	Then there are no errors
	And "result" evaluates to 6