# Linterhub Output
This document describes the structure of linterhub output
## Structure
Linterhub output is an array of engines results
### result
The engine result

|Key|Type|Required|Description|
|-|:-:|:-:|-|
|engine|string|+|The engine name that performed analysis|
|results|[Engine Output](engine.output.md)[]|+|The list of analysis results|
|error|[error](#error)|-|The problem definition if analysis is not possible|
### error
The problem definition if analysis is not possible

|Key|Type|Required|Description|
|-|:-:|:-:|-|
|code|integer|+|The error code|
|title|string|+|The error title|
|description|string|-|The error decription|
## Example
```
[
    {
        "engine": "string",
        "results": [],
        "error": {
            "code": 0,
            "title": "string",
            "description": "string"
        }
    }
]
```
