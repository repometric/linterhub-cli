# Linterhub Output
This document describes the structure of linterhub output
## Structure
Linterhub output is an array of engines results
### result
The engine result

|Key|Type|Required|Description|
|-|:-:|:-:|-|
|engine|string|+|The engine name that performed analysis|
|result|[Engine Output](engine.output.md)[]|-|The analysis result|
|error|[Engine Error](engine.error.md)|-|The problem definition if analysis is not possible|
## Example
```
[
    {
        "engine": "string",
        "result": [],
        "error": {}
    }
]
```