os.Container={};os.Container.inlineTemplates_=[];os.Container.domLoadCallbacks_=null;os.Container.domLoaded_=false;os.Container.autoProcess_=true;if(window.gadgets){var params=gadgets.util.getFeatureParameters("opensocial-templates");if(params&&params.disableAutoProcessing&&params.disableAutoProcessing.toLowerCase!="false"){os.Container.autoProcess_=false}}os.Container.processed_=false;os.Container.disableAutoProcessing=function(){if(os.Container.processed_){throw"Document already processed."}os.Container.autoProcess_=false};os.disableAutoProcessing=os.Container.disableAutoProcessing;os.Container.registerDomLoadListener_=function(){var a=window.gadgets;if(a&&a.util){a.util.registerOnLoadHandler(os.Container.onDomLoad_)}else{if(typeof(navigator)!="undefined"&&navigator.product&&navigator.product=="Gecko"){window.addEventListener("DOMContentLoaded",os.Container.onDomLoad_,false)}}if(window.addEventListener){window.addEventListener("load",os.Container.onDomLoad_,false)}else{if(!document.body){setTimeout(arguments.callee,0);return}var b=window.onload||function(){};window.onload=function(){b();os.Container.onDomLoad_()}}};os.Container.onDomLoad_=function(){if(os.Container.domLoaded_){return}while(os.Container.domLoadCallbacks_.length){try{os.Container.domLoadCallbacks_.pop()()}catch(a){os.log(a)}}os.Container.domLoaded_=true};os.Container.executeOnDomLoad=function(a){if(os.Container.domLoaded_){setTimeout(a,0)}else{if(os.Container.domLoadCallbacks_==null){os.Container.domLoadCallbacks_=[];os.Container.registerDomLoadListener_()}os.Container.domLoadCallbacks_.push(a)}};os.Container.registerDocumentTemplates=function(e){var f=e||document;var b=f.getElementsByTagName(os.Container.TAG_script_);for(var c=0;c<b.length;++c){var d=b[c];if(os.Container.isTemplateType_(d.type)){var a=d.getAttribute("tag");if(a){os.Container.registerTagElement_(d,a)}else{if(d.getAttribute("name")){os.Container.registerTemplateElement_(d,d.getAttribute("name"))}}}}};os.Container.compileInlineTemplates=function(a,g){var h=g||document;var b=h.getElementsByTagName(os.Container.TAG_script_);for(var d=0;d<b.length;++d){var f=b[d];if(os.Container.isTemplateType_(f.type)){var c=f.getAttribute("name")||f.getAttribute("tag");if(!c||c.length<0){var e=os.compileTemplate(f);if(e){os.Container.inlineTemplates_.push({template:e,node:f})}else{os.warn("Failed compiling inline template.")}}}}};os.Container.defaultContext=null;os.Container.getDefaultContext=function(){if(!os.Container.defaultContext){if((window.gadgets&&gadgets.util.hasFeature("opensocial-data"))||(opensocial.data.DataContext)){os.Container.defaultContext=os.createContext(opensocial.data.DataContext.getData())}else{os.Container.defaultContext=os.createContext({})}}return os.Container.defaultContext};os.Container.renderInlineTemplates=function(g){var j=g||document;var b=os.Container.getDefaultContext();var d=os.Container.inlineTemplates_;for(var f=0;f<d.length;++f){var k=d[f].template;var e=d[f].node;var a="_T_"+k.id;var c=j.getElementById(a);if(!c){c=j.createElement("div");c.setAttribute("id",a);e.parentNode.insertBefore(c,e)}if((window.gadgets&&gadgets.util.hasFeature("opensocial-data"))||(opensocial.data.DataContext)){var m=e.getAttribute("before")||e.getAttribute("beforeData");if(m){var n=m.split(/[\, ]+/);opensocial.data.DataContext.registerListener(n,os.Container.createHideElementClosure(c))}var h=e.getAttribute("require")||e.getAttribute("requireData");if(h){var n=h.split(/[\, ]+/);var l=os.Container.createRenderClosure(k,c,null,os.Container.getDefaultContext());if("true".equalsIgnoreCase(e.getAttribute("autoUpdate"))){opensocial.data.DataContext.registerListener(n,l)}else{opensocial.data.DataContext.registerOneTimeListener_(n,l)}}else{k.renderInto(c,null,b)}}else{k.renderInto(c,null,b)}}};os.Container.createRenderClosure=function(c,b,a,d){var e=function(){c.renderInto(b,a,d)};return e};os.Container.createHideElementClosure=function(a){var b=function(){displayNone(a)};return b};os.Container.registerTemplate=function(a){var b=document.getElementById(a);return os.Container.registerTemplateElement_(b)};os.Container.registerTag=function(a){var b=document.getElementById(a);os.Container.registerTagElement_(b,a)};os.Container.renderElement=function(b,d,a){var e=os.getTemplate(d);if(e){var c=document.getElementById(b);if(c){e.renderInto(c,a)}else{os.warn("Element ("+b+") not found to render into.")}}else{os.warn("Template ("+d+") not registered.")}};os.Container.processInlineTemplates=function(a){os.Container.compileInlineTemplates(a);os.Container.renderInlineTemplates(a)};os.Container.processDocument=function(a){os.Container.registerDocumentTemplates(a);os.Container.processInlineTemplates(a);os.Container.processed_=true};os.process=os.Container.processDocument;os.Container.executeOnDomLoad(function(){if(os.Container.autoProcess_){os.Container.processDocument()}});os.Container.TAG_script_="script";os.Container.templateTypes_={};os.Container.templateTypes_["text/os-template"]=true;os.Container.templateTypes_["text/template"]=true;os.Container.isTemplateType_=function(a){return os.Container.templateTypes_[a]!=null};os.Container.registerTemplateElement_=function(a,c){var b=os.compileTemplate(a,c);if(b){os.registerTemplate(b)}else{os.warn("Could not compile template ("+a.id+")")}return b};os.Container.registerTagElement_=function(d,c){var e=os.Container.registerTemplateElement_(d);if(e){var b=c.split(":");var a=os.getNamespace(b[0]);if(!a){a=os.createNamespace(b[0],null)}a[b[1]]=os.createTemplateCustomTag(e)}};