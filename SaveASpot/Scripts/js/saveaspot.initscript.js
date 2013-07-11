(function () {
	var documentReadyCollection = [];
	window.q = function (handler) {
		if (window.q.init == undefined) {
			documentReadyCollection.push(handler);
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


////compressed: (function(){var a=[];window.m=function(b){if(window.m.init==undefined){a.push(b)}return window.m};window.m.ready=function(b){window.m(b);return window.m};window.m.readyCollection=function(){return a}})();