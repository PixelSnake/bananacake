Feature: Integer arithmetics

Integer arithmetic operations

Scenario: Integers can be added
	Given the main function contains the following code:
		"""
		TEST("result", 17 + 3);
		"""
	When the code is compiled
	Then "result" evaluates to 20
	
Scenario: Integers can be subtracted
	Given the main function contains the following code:
		"""
		TEST("result", 17 - 3);
		"""
	When the code is compiled
	Then "result" evaluates to 14
	
Scenario: Integers can be multiplied
	Given the main function contains the following code:
		"""
		TEST("result", 17 * 3);
		"""
	When the code is compiled
	Then "result" evaluates to 51
	
Scenario: Integers can be divided, dropping decimals
	Given the main function contains the following code:
		"""
		TEST("result", 17 / 3);
		"""
	When the code is compiled
	Then "result" evaluates to 5
	
Scenario: Operator precedence is observed
	Given the main function contains the following code:
		"""
		TEST("result", 3 * 3 + 17 / 3);
		"""
	When the code is compiled
	Then "result" evaluates to 14
		
Scenario: Parentheses are observed
	Given the main function contains the following code:
		"""
		TEST("result", (3 * (3 + 17)) / 3);
		"""
	When the code is compiled
	Then "result" evaluates to 20
