Feature: Dependencies
	In order to ensure that items are executed in the correct order
	As the flattener process
	I want items to be output so that dependencies come before their dependents.

Scenario: Make one package reliant on another
	Given I have a package "PackageA"
	And I have a package "PackageB"
	And "PackageB" is dependent on "PackageA"
	When I output the flat list
	Then "PackageA" should appear before "PackageB"

Scenario: Make one package reliant two others
	Given I have a package "PackageA"
	And I have a package "PackageB"
	And I have a package "PackageC"
	And "PackageA" is dependent on "PackageB"
	And "PackageA" is dependent on "PackageC"
	When I output the flat list
	Then "PackageB" should appear before "PackageA"
	And "PackageC" should appear before "PackageA"

Scenario: Dependency chain
	Given I have a package "PackageA"
	And I have a package "PackageB"
	And I have a package "PackageC"
	And "PackageA" is dependent on "PackageB"
	And "PackageB" is dependent on "PackageC"
	When I output the flat list
	Then "PackageC" should appear before "PackageB"
	And "PackageB" should appear before "PackageA"