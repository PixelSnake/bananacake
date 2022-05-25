Feature: MainFunction

The main function

	@Error
	Scenario: Not returning an exit code produces a parsing error
		Given the main function contains the following code:
			"""
			"""
		When the code is compiled
		Then an error is returned
