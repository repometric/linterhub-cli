node script/js/md.js src/hub/.schema/linterhub.output.json > docs/hub/.schema/linterhub.output.md
node script/js/md.js src/hub/.schema/linterhub.config.json > docs/hub/.schema/linterhub.config.md
node script/js/md.js src/hub/.schema/engine.output.json > docs/hub/.schema/engine.output.md
node script/js/md.js src/hub/.schema/engine.json > docs/hub/.schema/engine.md
node script/js/md.js src/hub/.schema/args.json > docs/hub/.schema/args.md

node script/js/cs.js src/hub/.schema/linterhub.output.json > src/engine/Schema/LinterhubOutputSchema.cs
node script/js/cs.js src/hub/.schema/linterhub.config.json > src/engine/Schema/LinterhubConfigSchema.cs
node script/js/cs.js src/hub/.schema/engine.output.json > src/engine/Schema/EngineOutputSchema.cs
node script/js/cs.js src/hub/.schema/engine.json > src/engine/Schema/EngineSchema.cs
#node script/js/cs.js src/hub/.schema/args.json > src/engine/Schema/ArgsSchema.cs