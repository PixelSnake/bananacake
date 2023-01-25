@StdLib
Feature: Arrays

Arrays can hold a fixed number of items of the same datatype

Scenario: Array can be constructed
	Given the main function contains the following code:
		"""
		Array<int> nums = new Array<int>(8);
		"""
	When the code is compiled
	Then there are no errors

Scenario: Array can be read from
	Given the main function contains the following code:
		"""
		Array<int> nums = new Array<int>(8);
		TEST("result", nums[0]);
		"""
	When the code is compiled
	Then there are no errors

Scenario: Array is initialized with the default value of the given type
	Given the main function contains the following code:
		"""
		Array<int> nums = new Array<int>(8);
		Array<bool> bools = new Array<bool>(8);
		Array<string> strings = new Array<string>(8);
		TEST("int", nums[0]);
		TEST("bool", bools[0]);
		TEST("string", strings[0]);
		"""
	When the code is compiled
	Then there are no errors
	And "int" evaluates to 0
	And "bool" evaluates to false
	And "string" evaluates to null

Scenario: Arrays can be written to
	Given the main function contains the following code:
		"""
		Array<int> nums = new Array<int>(8);
		nums[0] = 15;
		"""
	When the code is compiled
	Then there are no errors