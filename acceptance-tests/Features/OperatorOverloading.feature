Feature: Operator overloading

Scenario: Classes can overload the plus operator
	Given the following class is defined:
		"""
		class Number {
			int value;

			public Number(this.value);

			public Number operator_plus(Number other) {
				return new Number(value + other.value);
			}

			public cast string() {
				return value as string;
			}
		}
		"""
	And the main function contains the following code:
		"""
		Number sum = new Number(123) + new Number(456);
		TEST("result", sum as string);
		"""
	When the code is compiled
	Then there are no errors
	And "result" evaluates to 579

Scenario: Classes can overload the minus operator:
	Given the following class is defined:
		"""
		class Number {
			int value;

			public Number(this.value);

			public Number operator_minus(Number other) {
				return new Number(value - other.value);
			}

			public cast string() {
				return value as string;
			}
		}
		"""
	And the main function contains the following code:
		"""
		Number sum = new Number(123) - new Number(456);
		TEST("result", sum as string);
		"""
	When the code is compiled
	Then there are no errors
	And "result" evaluates to -333
	