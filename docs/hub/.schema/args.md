# Args
This document describes the engine arguments
## Structure
The engine arguments

|Key|Type|Required|Description|
|-|:-:|:-:|-|
|title|string|+|The schema title|
|description|string|+|The schema description|
|definitions|[definitions](#definitions)|+|The arguments definitions|
### definitions
The arguments definitions

|Key|Type|Required|Description|
|-|:-:|:-:|-|
|options|object|-|The engine options|
|section|object|+|The engine configuration section|
## Example
```
{
    "title": "string",
    "description": "string",
    "definitions": {
        "options": {},
        "section": {}
    }
}
```
