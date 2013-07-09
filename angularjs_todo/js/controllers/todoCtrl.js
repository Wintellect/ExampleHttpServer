/*global todomvc */
'use strict';

/**
 * The main controller for the app. The controller:
 * - retrieves and persists the model via the todoStorage service
 * - exposes the model to the template and provides event handlers
 */
todomvc.controller('TodoCtrl', function TodoCtrl($scope, $location, todoStorage, filterFilter) {
	$scope.todos = [];
	
	todoStorage.get()
	.success(function(result){
		$scope.todos = result;
	});

	$scope.newTodo = '';
	$scope.editedTodo = null;

	$scope.$watch('todos', function () {
		$scope.remainingCount = filterFilter($scope.todos, { completed: false }).length;
		$scope.completedCount = $scope.todos.length - $scope.remainingCount;
		$scope.allChecked = !$scope.remainingCount;
		todoStorage.put($scope.todos);
	}, true);

	if ($location.path() === '') {
		$location.path('/');
	}

	$scope.location = $location;

	$scope.$watch('location.path()', function (path) {
		$scope.statusFilter = (path === '/active') ?
			{ completed: false } : (path === '/completed') ?
			{ completed: true } : null;
	});

	$scope.addTodo = function () {
		var newTodo = $scope.newTodo.trim();
		if (!newTodo.length) {
			return;
		}

		todoStorage.put([{
			title: newTodo,
			completed: false
		}]).success(function(){
		$scope.newTodo = '';
				todoStorage.get()
				.success(function(result){
					$scope.todos = result;
				});
		});
	};

	$scope.editTodo = function (todo) {
		$scope.editedTodo = todo;
	};

	$scope.doneEditing = function (todo) {
		$scope.editedTodo = null;
		todo.title = todo.title.trim();

		if (!todo.title) {
			$scope.removeTodo(todo);
		}
	};

	$scope.removeTodo = function (todo) {
		var removed = $scope.todos.splice($scope.todos.indexOf(todo), 1);
		
		todoStorage.remove(removed);
	};

	$scope.clearCompletedTodos = function () {
		var removed = $scope.todos.filter(function (val) {
			return val.completed;
		});
		
		todoStorage.remove(removed);
		
		$scope.todos = $scope.todos.filter(function (val) {
			return !val.completed;
		}); 
	};

	$scope.markAll = function (completed) {
		$scope.todos.forEach(function (todo) {
			todo.completed = completed;
		});
	};
});
