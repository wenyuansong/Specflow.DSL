# Specflow.DSL

An enhancement to Specflow DSL to be able to use dynamic test data in specflow steps by bringing in variables, regular expressions and simple calculations.


Syntax:
  [[ ]]               //double bracket in any text will trigger pattern matching
  [[varName=value]]   //will create a variable named "varName" with value "value"
  [[varName]]         //will get value of "varName", throw an error if "varName" is not defined
  
How it works:
   It actually creates key/value pairs in ScenarioContext.Current.
   So be careful not to conflict with your own context variables. 

**Examples**: 
 
 - Create dynamic test data and refer it in another step
```
	When enter [[var=50]]         //assign 50 to a variable named "var"
	Then [[var]] equals 50    // now get variable "var" value
```	
 - Create dynamic test data using regular expression
```
 	When enter [[var=RegEx([0-9]{3})]]         //assign var with 3 digit random number
	Then [[var]] is a 3 digits number          // now get variable "var" value
```
 - Same applies to SpecFlow tables:
```
 	When create new user with the following details:
     |Field   | Value                                 |
     |UserName| [[name=RegEx([a-z]{5,8})]]            |	        
	 |Password| [[pwd=RegEx([a-z]{3}[0-9]{3})]]       |
	Then verify can login with username="[[name]]" and password="[[pwd]]"
```   

 - Support customerise transformation

```
     for example, you want to map Today to YYYY:MM:dd, add the following code in one of your Specflow steps 
	 or put it in BeforeScenario step.
      ((IParameterTransform)
                (ScenarioContext.Current.GetBindingInstance(typeof(IParameterTransform))))
            .addTransformer(s => s.ToLower() == today ? DateTime.Now.ToString("yyyy/MM/dd") : s); 
	
     Now in Specflow feature files, you can write:	
	 When entered "[[timeVar=Today]]"		  //timeVar will be assigned to yyyy/MM/dd, e.g 2017/12/04
```  
 
 - Calculations are currently NOT supported

Dependencies:
* SpecFlow v2.1 +
* Fare 1.0.3 +

License: MIT (https://github.com/wenyuansong/Specflow.DSL/blob/master/LICENSE)

NuGet: https://www.nuget.org/packages/SpecFlow.DSL

## Installation

- Install plugin from NuGet into your SpecFlow project.

    PM> Install-Package SpecFlow.DSL
 
That's it!!!   

Your project's App.config file will be automatically added the following lines to enable plugin:
```
   <plugins>
      <add name="SpecFlow.DSL" type="Runtime"/>
    </plugins>
 ```
