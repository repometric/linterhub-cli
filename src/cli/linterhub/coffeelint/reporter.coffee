module.exports = class Reporter

    constructor: (@errorReport, options = {}) ->
        { @quiet } = options

    print: (message) ->
        # coffeelint: disable=no_debugger
        console.log message
        # coffeelint: enable=no_debugger

    publish: () ->
        filesResult = []
        for path, errors of @errorReport.paths
            fileResult = {
                path: path
                messages: []
            }
            for e in errors when not @quiet or e.level is 'error'
                fileResult.messages.push({
                    message: e.message
                    description: e.description
                    severity: e.level
                    context: e.context
                    line: e.line
                    row: {
                        start: e.lineNumber - 1,
                        end: if e.lineNumberEnd then e.lineNumberEnd - 1 else e.lineNumber - 1
                    }
                    column: {
                        start: 0,
                        end: 1000
                    }
                    rule: {
                        id: e.value,
                        name: e.name,
                        namespace: e.rule
                    }
                })
            filesResult.push(fileResult)

        @print JSON.stringify(filesResult, undefined, 0)
