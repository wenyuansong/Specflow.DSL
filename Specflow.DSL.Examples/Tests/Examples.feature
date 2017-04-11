@UT
Feature: Examples

Scenario: int 
	When entered int [[var=50]]
	Then verify int [[var]] equals 50

Scenario: string 
	When entered string "[[var=50]]"
	Then verify string "[[var]]" equals "50"

Scenario: regex generate
	When entered string "[[var=RegEx([a-z]{3}[0-9]{1,9})]]"
	Then verify string "[[var]]" matches "[a-z]{3}[0-9]{1,9}"
		
		
