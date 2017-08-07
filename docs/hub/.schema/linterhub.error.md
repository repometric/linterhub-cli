# Linterhub Error
This document describes the structure of linterhub error
## Structure
The problem definition including code and explandatory text

|Key|Type|Required|Description|
|-|:-:|:-:|-|
|code|integer|+|The error code|
|title|string|+|The error title|
|description|string|-|The error decription|
## Example
```
{
    "code": 0,
    "title": "string",
    "description": "string"
}
```