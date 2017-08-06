# Linterhub Fetch
This document describes the structure of fetching engines request output
## Structure
Fetch engines output is list of possible used engines
### result
Founded engine

|Key|Type|Required|Description|
|-|:-:|:-:|-|
|name|string|+|The engine name|
|found|string|+|The way how engine founded. Possible values: `sourceExtension`, `engineConfig`, `projectConfig`|
## Example
```
[
    {
        "name": "string",
        "found": "sourceExtension"
    }
]
```