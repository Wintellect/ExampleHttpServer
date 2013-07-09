/*global todomvc */
'use strict';

/**
 * Services that persists and retrieves TODOs from localStorage
 */
todomvc.factory('todoStorage', function ($http) {
	var STORAGE_ID = 'todos-angularjs';

	return {
		get: function () {
			return $http({method: 'GET', url: '/api/todo'});
		},

		put: function (todos) {
			return $http({method: 'PUT', url: '/api/todo', data: todos});
		},
		
		remove: function (todos) {
			return $http({
				method: 'DELETE', 
				url: '/api/todo', 
				data: todos,
				headers: {"content-type": "application/json"}
			});
		}
	};
});
