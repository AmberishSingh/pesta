var osapi=osapi||{};(function(){osapi._registerMethod=function(f,e){var c=f.split(".");var b=osapi;for(var a=0;a<c.length-1;a++){b[c[a]]=b[c[a]]||{};b=b[c[a]]}var d=function(i){var h=osapi.newBatch();var g={};g.execute=function(j){h.add(f,this);h.execute(function(k){if(k.error){j(k.error)}else{j(k[f])}})};i=i||{};i.userId=i.userId||"@viewer";i.groupId=i.groupId||"@self";g.method=f;g.transport=e;g.rpc=i;return g};if(b[c[c.length-1]]){gadgets.warn("Duplicate osapi method definition "+f)}b[c[c.length-1]]=d}})();