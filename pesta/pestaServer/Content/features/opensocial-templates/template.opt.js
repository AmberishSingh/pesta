os.createContext=function(c,d){var a=JsEvalContext.create(c);a.setVariable(os.VAR_callbacks,[]);a.setVariable(os.VAR_identifierresolver,os.getFromContext);if(d){for(var b in d){if(d.hasOwnproperty(b)){a.setVariable(b,d[b])}}}return a};os.Template=function(a){this.templateRoot_=document.createElement("span");this.id=a||("template_"+os.Template.idCounter_++)};os.Template.idCounter_=0;os.registeredTemplates_={};os.registerTemplate=function(a){os.registeredTemplates_[a.id]=a};os.unRegisterTemplate=function(a){delete os.registeredTemplates_[a.id]};os.getTemplate=function(a){return os.registeredTemplates_[a]};os.Template.prototype.setCompiledNode_=function(a){os.removeChildren(this.templateRoot_);this.templateRoot_.appendChild(a)};os.Template.prototype.setCompiledNodes_=function(a){os.removeChildren(this.templateRoot_);for(var b=0;b<a.length;b++){this.templateRoot_.appendChild(a[b])}};os.Template.prototype.render=function(a,b){if(!b){b=os.createContext(a)}return os.renderTemplateNode_(this.templateRoot_,b)};os.Template.prototype.renderInto=function(c,b,d){if(!d){d=os.createContext(b)}var a=this.render(b,d);os.removeChildren(c);os.appendChildren(a,c);os.fireCallbacks(d)};