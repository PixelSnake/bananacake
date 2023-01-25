Feature: Number Literals

Numbers can be expressed in different bases using a prefix

Scenario: Decimal integer literals
	Given the main function contains the following code:
		"""
		TEST("result", 1337);
		"""
	When the code is compiled
	Then there are no errors
	And "result" evaluates to 1337
	
Scenario: Hexadecimal integer literals
	Given the main function contains the following code:
		"""
		TEST("result", 0x539);
		"""
	When the code is compiled
	Then there are no errors
	And "result" evaluates to 1337

Scenario: Octal integer literals
	Given the main function contains the following code:
		"""
		TEST("result", 02471);
		"""
	When the code is compiled
	Then there are no errors
	And "result" evaluates to 1337

Scenario: Binary integer literals
	Given the main function contains the following code:
		"""
		TEST("result", 0b0000010100111001);
		"""
	When the code is compiled
	Then there are no errors
	And "result" evaluates to 1337
