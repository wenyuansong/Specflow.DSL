@UT
Feature: Examples

Scenario: number test
	When entered int [[var=50]]
	Then verify int [[var]] equals 50

Scenario: string test
	When entered string "[[var=50]]"
	Then verify string "[[var]]" equals "50"

Scenario: regex generate
	When entered string "[[var=RegEx([a-z]{3}[0-9]{1,9})]]"
	Then verify string "[[var]]" matches "[a-z]{3}[0-9]{1,9}"

Scenario: table
	When use table with the following details: 
	| Field | Value                |
	| name  | [[var=DesiredValue]] |
	Then verify string "[[var]]" equals "DesiredValue"
		
Scenario: nested
	When entered string "[[var=part1]]"
	And entered string "[[var2=[[var]] part2]]"
	Then verify string "[[var2]]" equals "part1 part2"
		
Scenario: customerise pattern  
    Given I have a cutomerise pattern mapping "MyKeyword" to "MyKeywordValue"
	When entered string "[[var=MyKeyword]]"
	Then verify string "[[var]]" equals "MyKeywordValue"

Scenario: add calculation
    Given I have a cutomerise pattern to support calculation
	When entered int [[var=50]]
	And entered int [[var2=[[var]]+1]]
	Then verify int [[var2]] equals 51
	When entered int [[var2=[[var]]*2]]
	Then verify int [[var2]] equals 100
	When entered int [[var2=[[var]]/2]]
	Then verify int [[var2]] equals 25
	When entered int [[var2=[[var]]-[[var]]]]
	Then verify int [[var2]] equals 0
				