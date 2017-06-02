var App = angular.module('myApp', ['ngCookies','ngSanitize']);


App.directive('loadingss', ['$http', function ($http) {
      return {
          restrict: 'A',
          link: function (scope, element, attrs) {
              scope.isLoading = function () {
                  return $http.pendingRequests.length > 0;
              };
              scope.$watch(scope.isLoading, function (value) {
                  if (value) {
                      $("#loadingss").animate({top: '0px'},800);
                  } else {
                      $("#loadingss").animate({ top: '-100px' },800);
                  }
              });
          }
      };
  } ]);


App.directive('validNumber', function() {
  return {
    require: '?ngModel',
    link: function(scope, element, attrs, ngModelCtrl) {
      if(!ngModelCtrl) {
        return; 
      }

      ngModelCtrl.$parsers.push(function(val) {
        if (angular.isUndefined(val)) {
            var val = '';
        }
        var clean = val.replace( /[^- +0-9]+/g, '');
        if (val !== clean) {
          ngModelCtrl.$setViewValue(clean);
          ngModelCtrl.$render();
        }
        return clean;
      });

      element.bind('keypress', function(event) {
        if(event.keyCode === 32) {
          event.preventDefault();
        }
      });
    }
  };
});

// /app/directives/ng-confirm-click.js
App.directive('ngConfirmClick', [
  function(){
    return {
      priority: -1,
      restrict: 'A',
      link: function(scope, element, attrs){
        element.bind('click', function(e){
          var message = attrs.ngConfirmClick;
          // confirm() requires jQuery
          if(message && !confirm(message)){
            e.stopImmediatePropagation();
            e.preventDefault();
          }
        });
      }
    }
  }
]);



  App.directive('fileModel', ['$parse', function ($parse) {
      return {
          restrict: 'A',
          link: function (scope, element, attrs) {
              var model = $parse(attrs.fileModel);
              var modelSetter = model.assign;

              element.bind('change', function () {
                  scope.$apply(function () {
                      modelSetter(scope, element[0].files[0]);
                  });
              });
          }
      };
  } ]);


App.filter('newline', function () {
    return function(text) {
        return text.replace(/\\n/g, '<br/>');
    }
});


  App.directive('stringToNumber', function () {
      return {
          require: 'ngModel',
          link: function (scope, element, attrs, ngModel) {
              ngModel.$parsers.push(function (value) {
                  return '' + value;
              });
              ngModel.$formatters.push(function (value) {
                  return parseFloat(value, 10);
              });
          }
      }
  })


  App.directive('ngEnter', function () {
      return function (scope, element, attrs) {
          element.bind("keydown keypress", function (event) {
              if (event.which === 13) {
                  scope.$apply(function () {
                      scope.$eval(attrs.ngEnter, { 'event': event });
                  });

                  event.preventDefault();
              }
          });
      };
  });
