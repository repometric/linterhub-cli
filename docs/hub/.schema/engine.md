# Engine
This document describes the engine definition
## Structure
The meta information of engine

|Key|Type|Required|Description|
|-|:-:|:-:|-|
|name|string|+|The engine name|
|executable|string|-|The engine executable name; by default it's equal to engine name|
|runLocally|boolean|-|Way how to run engine. If engine installed locally for current project, than cant execute it with just engine name|
|description|string|+|The engine description|
|url|string|-|The engine url or homepage|
|version|string|-|The engine version|
|languages|string[]|+|The list of supported languages. Possible values: `coffeescript`, `css`, `html`, `xml`, `javascript`, `json`, `jsx`, `sass`, `typescript`, `c`, `cpp`, `csharp`, `groovy`, `php`, `ruby`, `fortran`, `plsql`, `scala`, `objectivec`, `perl`, `swift`, `python`, `java`, `pug`|
|extensions|string[]|-|Common file extensions parsed by engine|
|configs|string[]|-|List of file names which could be treated as engine config|
|license|string|+|The engine license. Possible values: `Unknown`, `AGPL-3.0`, `Apache-2.0`, `MIT`, `ISC`|
|requirements|[requirement](#requirement)[]|-|The engine requirements|
|areas|string[]|+|The engine areas. Possible values: `code simplification`, `commented code`, `complexity`, `documentation`, `duplication`, `formatting`, `grammar`, `memory leak`, `security`, `simplification`, `smell`, `spelling`, `syntax`, `undefined element`, `unreachable code`, `unused code`|
|acceptMask|boolean|-|Can use masks for multiple files analyze. Default is `true`|
|output|string|-|The engine output format. Possible values: `json`, `xml`. Default is `json`|
|postfix|string|-|Posstfix in terminal (normaly post processor)|
|optionsDelimiter|string|-|Delimiter for options (space by default). Default is ` `|
|successCode|integer|-|Success exit code|
|active|boolean|-|A value indicating whether engine is active. Default is `true`|
|defaults|object|-|The default configuration of engine. This property is specific for each engine|
|stdin|object|-|The stdin configuration of engine. This property is specific for each engine. Must be an empty object, if engine needs no params, but supports stdin|
### requirement
The engine dependency

|Key|Type|Required|Description|
|-|:-:|:-:|-|
|manager|string|-|The manager for dependency. Possible values: `system`, `url`, `composer`, `gem`, `npm`, `pip`|
|package|string|-|The package name|
|version|string|-|The package version|
## Example
```
{
    "name": "string",
    "executable": "string",
    "runLocally": false,
    "description": "string",
    "url": "string",
    "version": "string",
    "languages": [],
    "extensions": [],
    "configs": [],
    "license": "Unknown",
    "requirements": [
        {
            "manager": "system",
            "package": "string",
            "version": "string"
        }
    ],
    "areas": [],
    "acceptMask": false,
    "output": "json",
    "postfix": "string",
    "optionsDelimiter": "string",
    "successCode": 0,
    "active": false,
    "defaults": {},
    "stdin": {}
}
```