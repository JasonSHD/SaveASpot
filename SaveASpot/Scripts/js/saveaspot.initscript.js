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
})();


////compressed: (function(){var a=[];window.q=function(c,b){if(b===undefined&&c===undefined){return window.q}if(b==undefined&&typeof c==="function"){b=c;c=undefined}if(window.q.init==undefined){a.push({filter:c,handler:b})}return window.q};window.q.ready=function(b){window.q(b);return window.q};window.q.readyCollection=function(){return a}})();