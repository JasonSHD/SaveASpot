(function () {
	var documentReadyCollection = [];
	window.q = function (filter, handler) {
		if(handler === undefined && filter === undefined){
			return window.q;
		}
	
		if(handler == undefined && typeof filter === "function"){
			handler = filter;
			filter = undefined;
		}
		
		if (window.q.init == undefined) {
			documentReadyCollection.push({filter: filter, handler: handler});
		}

		return window.q;
	};

	window.q.ready = function (handler) {
		window.q(handler);

		return window.q;
	};

	window.q.readyCollection = function () {
		return documentReadyCollection;
	};

	window.onerror = function(message, source, lineNumber) {

		var dto = {};
		dto.message = message;
		dto.source = source;
		dto.lineNumber = lineNumber;

		$.ajax(
			{
				type: 'POST',
				contentType: 'application/json; charset=utf-8',
				data: JSON.stringify(dto),
				url: 'ErrorPage/LogJavascriptError'
			});
	};
})();


////compressed: (function(){var a=[];window.q=function(c,b){if(b===undefined&&c===undefined){return window.q}if(b==undefined&&typeof c==="function"){b=c;c=undefined}if(window.q.init==undefined){a.push({filter:c,handler:b})}return window.q};window.q.ready=function(b){window.q(b);return window.q};window.q.readyCollection=function(){return a};window.onerror=function(d,e,b){var c={};c.message=d;c.source=e;c.lineNumber=b;$.ajax({type:"POST",contentType:"application/json; charset=utf-8",data:JSON.stringify(c),url:"ErrorPage/LogJavascriptError"})}})();