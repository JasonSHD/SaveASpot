(function ($) {
	var oldQ = window.q;
	
	window.q.runReadyHandlers = function(filter, runEmpty){
		var readyHandlers = oldQ.readyCollection();
		
		for (var readyHandlerIndex in readyHandlers) {
			var readyHandler = readyHandlers[readyHandlerIndex];

			if(readyHandler.filter == filter || (readyHandler.filter === undefined && runEmpty)){
				readyHandler.handler();
			}
		}
	};

	$(function () {		
		var filter = $("#qFilter").val();
		
		q.runReadyHandlers(filter, true);		
	});
})(jQuery);

q.controls = q.controls || {};
(function(namespace, $){
	namespace.ajaxTab = function($container){
		var result = {_data: { container: $($container) } };
		
		$("[data-ajaxtab]").each(function(){
			$(this).click(function(){
				result.update(this);
			});
		});
		
		result.update = function(contextElement){
			var $context = $(contextElement);
			var url = $context.attr("data-ajaxtab");
			
			console.log("update from url:" + url);
			
			q.runReadyHandlers("test")
			
			return result;
		};
		
		return result;
	};
})(q, jQuery);
