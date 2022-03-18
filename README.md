# CronExpressionParser Console App
CronExpressionParser is a console app written in C# and that provides the following facilities:

* Parsing of cron expressions
* Calculation of occurrences as specified
* Formatting of cron expressions

This app does not provide any executable command functionality, so it will just be passed through to the output.
What it provides is parsing, formatting and an algorithm
to produce occurrences of time based on a give schedule expressed in the crontab
format:

    * * * * * /sh
    - - - - - -
    | | | | | |
    | | | | | +--- command (just a string)
    | | | | +----- day of week (0 - 6) (Sunday=0)
    | | | +------- month (1 - 12)
    | | +--------- day of month (1 - 31)
    | +----------- hour (0 - 23)
    +------------- minute (0 - 59)

## Supported cron expressions ##

```
Field name   | Allowed values  | Allowed special characters
------------------------------------------------------------
minutes      | 0-59            | * , - /
hours        | 0-23            | * , - /
day of month | 1-31            | * , - /
month        | 1-12            | * , - /
day of week  | 0-6             | * , - /
```
This app supports the standard step arguments with a range as the first argument as follows:

    0-30/15 * * * * /sh


## Running the application ##
To use the application navigate to the CronExpressionParser folder and run the following command:

```
dotnet run -- "*/15 0 1,15 * 1-5 /usr/bin/find"
```

## Output from application ##
The output is formatted as a table with the field name taking the first 14 columns and
the times as a space-separated list following it, please see example below: 

```
minute        0 15 30 45
hour          0
day of month  1 15
month         1 2 3 4 5 6 7 8 9 10 11 12
day of week   1 2 3 4 5
command       /sh

```