(function(){function a(h,g){function f(k){if(k.errors[0]){g({error:{code:k.rc,message:k.text}})}else{var l=k.data;if(l.error){g(l)}else{var j={};for(var m=0;m<l.length;m++){j[l[m].id]=l[m]}g(j)}}}var e={POST_DATA:gadgets.json.stringify(h),CONTENT_TYPE:"JSON",METHOD:"POST",AUTHORIZATION:"SIGNED"};var c=this.name;var d=shindig.auth.getSecurityToken();if(d){c+="?st=";c+=encodeURIComponent(d)}gadgets.io.makeNonProxiedRequest(c,f,e,"application/json")}function b(f){var h=f["osapi.services"];if(h){for(var e in h){if(h.hasOwnProperty(e)){if(e.indexOf("http")==0){var c=e.replace("%host%",document.location.host);var j={name:c,execute:a};var d=h[e];for(var g=0;g<d.length;g++){osapi._registerMethod(d[g],j)}}}}}}gadgets.config.register("osapi.services",null,b)})();