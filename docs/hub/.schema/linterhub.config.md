# Linterhub Config
This document describes the structure of linterhub configuration
## Structure
The configuration for linterhub engines

|Key|Type|Required|Description|
|-|:-:|:-:|-|
|engines|[configuration](#configuration)[]|-|List of engines configurations|
|ignore|[ignore](#ignore)[]|-|The list of global ignore rules|
### configuration
The engine configuration

|Key|Type|Required|Description|
|-|:-:|:-:|-|
|name|string|+|The engine name|
|active|boolean|-|Whether engine is enabled. Default is `true`|
|config|object|-|The engine specific configuration|
|ignore|[ignore](#ignore)[]|-|The list of rules for ignoring engine results|
### ignore
The configuration for ignoring engine results

|Key|Type|Required|Description|
|-|:-:|:-:|-|
|mask|string|-|The path mask|
|line|integer|-|The line nubmer|
|ruleId|string|-|The rule id|
## Example
```
{
    "engines": [
        {
            "name": "string",
            "active": false,
            "config": {},
            "ignore": [
                {
                    "mask": "string",
                    "line": 0,
                    "ruleId": "string"
                }
            ]
        }
    ],
    "ignore": [
        {
            "mask": "string",
            "line": 0,
            "ruleId": "string"
        }
    ]
}
```
