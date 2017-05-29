# Engine Output
This document describes the structure of engine output
## Structure
Engine output is an array of analysis results
### result
Represents analysis result

|Key|Type|Required|Description|
|-|:-:|:-:|-|
|path|string|+|The path relative to the analysis root|
|messages|[message](#message)[]|+|List of messages in the path|
### message
Represents analysis message

|Key|Type|Required|Description|
|-|:-:|:-:|-|
|message|string|+|The short description of the message|
|description|string|-|The explanatory text of the message|
|severity|string|+|The severity of the message. Possible values: `verbose`, `hint`, `information`, `warning`, `error`|
|line|integer|+|The line where the message is located|
|lineEnd|integer|-|The end line where the message is located (the same as line by default)|
|column|integer|+|The column where the message is located|
|columnEnd|integer|-|The end column where the message is located. Default is `1000`|
|ruleId|string|-|The id of the rule that produced the message|
|ruleName|string|-|The name of the rule that produced the message|
|ruleNs|string|-|The namespace of the rule that produced the message|
## Example
```
[
    {
        "path": "string",
        "messages": [
            {
                "message": "string",
                "description": "string",
                "severity": "verbose",
                "line": 0,
                "lineEnd": 0,
                "column": 0,
                "columnEnd": 0,
                "ruleId": "string",
                "ruleName": "string",
                "ruleNs": "string"
            }
        ]
    }
]
```
