var JSON=gadgets.json;var _IG_Prefs=(function(){var a=null;var b=function(){if(!a){a=new gadgets.Prefs();a.setDontEscape_()}return a};b._parseURL=gadgets.Prefs.parseUrl;return b})();function _IG_Fetch_wrapper(b,a){b(a.data?a.data:"")}function _IG_FetchContent(b,g,c){var f=c||{};if(f.refreshInterval){f.REFRESH_INTERVAL=f.refreshInterval}else{f.REFRESH_INTERVAL=3600}for(var e in f){var d=f[e];delete f[e];f[e.toUpperCase()]=d}var a=gadgets.util.makeClosure(null,_IG_Fetch_wrapper,g);gadgets.io.makeRequest(b,a,f)}function _IG_FetchXmlContent(b,e,c){var d=c||{};if(d.refreshInterval){d.REFRESH_INTERVAL=d.refreshInterval}else{d.REFRESH_INTERVAL=3600}d.CONTENT_TYPE="DOM";var a=gadgets.util.makeClosure(null,_IG_Fetch_wrapper,e);gadgets.io.makeRequest(b,a,d)}function _IG_FetchFeedAsJSON(b,f,c,a,d){var e=d||{};e.CONTENT_TYPE="FEED";e.NUM_ENTRIES=c;e.GET_SUMMARIES=a;gadgets.io.makeRequest(b,function(i){i.data=i.data||{};if(i.errors&&i.errors.length>0){i.data.ErrorMsg=i.errors[0]}if(i.data.link){i.data.URL=b}if(i.data.title){i.data.Title=i.data.title}if(i.data.description){i.data.Description=i.data.description}if(i.data.link){i.data.Link=i.data.link}if(i.data.items&&i.data.items.length>0){i.data.Entry=i.data.items;for(var g=0;g<i.data.Entry.length;++g){var h=i.data.Entry[g];h.Title=h.title;h.Link=h.link;h.Summary=h.summary||h.description;h.Date=h.pubDate}}f(i.data)},e)}function _IG_GetCachedUrl(a,b){var c={REFRESH_INTERVAL:3600};if(b&&b.refreshInterval){c.REFRESH_INTERVAL=b.refreshInterval}return gadgets.io.getProxyUrl(a,c)}function _IG_GetImageUrl(a,b){return _IG_GetCachedUrl(a,b)}function _IG_GetImage(b){var a=document.createElement("img");a.src=_IG_GetCachedUrl(b);return a}function _IG_RegisterOnloadHandler(a){gadgets.util.registerOnLoadHandler(a)}function _IG_Callback(b,c){var a=arguments;return function(){var d=Array.prototype.slice.call(arguments);b.apply(null,d.concat(Array.prototype.slice.call(a,1)))}}var _args=gadgets.util.getUrlParameters;function _gel(a){return document.getElementById?document.getElementById(a):null}function _gelstn(a){if(a==="*"&&document.all){return document.all}return document.getElementsByTagName?document.getElementsByTagName(a):[]}function _gelsbyregex(d,f){var c=_gelstn(d);var e=[];for(var b=0,a=c.length;b<a;++b){if(f.test(c[b].id)){e.push(c[b])}}return e}function _esc(a){return window.encodeURIComponent?encodeURIComponent(a):escape(a)}function _unesc(a){return window.decodeURIComponent?decodeURIComponent(a):unescape(a)}function _hesc(a){return gadgets.util.escapeString(a)}function _striptags(a){return a.replace(/<\/?[^>]+>/g,"")}function _trim(a){return a.replace(/^\s+|\s+$/g,"")}function _toggle(a){a=_gel(a);if(a!==null){if(a.style.display.length===0||a.style.display==="block"){a.style.display="none"}else{if(a.style.display==="none"){a.style.display="block"}}}}var _global_legacy_uidCounter=0;function _uid(){return _global_legacy_uidCounter++}function _min(d,c){return(d<c?d:c)}function _max(d,c){return(d>c?d:c)}function _exportSymbols(a,b){var h={};for(var m=0,f=b.length;m<f;m+=2){h[b[m]]=b[m+1]}var e=a.split(".");var n=window;for(var d=0,c=e.length-1;d<c;++d){var g={};n[e[d]]=g;n=g}n[e[e.length-1]]=h};