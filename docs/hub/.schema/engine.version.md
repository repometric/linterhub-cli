# Engine Version
This document describes the structure of engine version output
## Structure
Engine version output is an array of objects describing the state of the engines
### result
Represents install/version request result

|Key|Type|Required|Description|
|-|:-:|:-:|-|
|installed|boolean|+|Installation status|
|message|string|-|Error message if cant install engine|
|name|string|+|Engine name|
|version|string|-|Engine version|
|packages|[result](#result)[]|-|List of dependencies|
## Example
```
[
    {
        "installed": false,
        "message": "string",
        "name": "string",
        "version": "string",
        "packages": []
    }
]
```