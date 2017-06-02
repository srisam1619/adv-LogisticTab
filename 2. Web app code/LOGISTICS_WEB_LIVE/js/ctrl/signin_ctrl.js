App.controller('signin', ['$scope', '$rootScope', '$http', '$window', '$cookies','util_SERVICE',

function ($scope, $rootScope, $http, $window, $cookies,US) {
    // $scope.items = Data;
    $scope.userId = "";
    $scope.password = "";
    $cookies.put('MenuInfo', "");
    $cookies.put('UserData', "");
    $cookies.put('Islogin', "false");
	
	var url = US.url;
	//var url = "http://192.168.0.38:82/DOService.asmx/";
	$scope.company = "ADV_DO";
	
	$scope.loadcompany = function () {
		
		

        var data = {"sUserName" : $scope.userId, "sPassword" : $scope.password, "sCompany" : $scope.company}

        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var parms = JSON.stringify(data);
        $http.post(url+"GetCompanyList", "sJsonInput=" + "", config)
   .then(
       function (response) {
           // success callback
           console.log(response.data);
           if (response.data !="") {
             $scope.companylist = response.data;
			 $scope.company = $scope.companylist[0].U_DBName;
           }
            else
               $scope.companylist = [];
       },
       function (response) {
           // failure callback

       }
    );

    
		
		
	}

    $scope.checklogin = function () {

        var data = {"sUserName" : $scope.userId, "sPassword" : $scope.password, "sCompany" : $scope.company}

        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var parms = JSON.stringify(data);
        $http.post(url+"LoginValidation", "sJsonInput=" + parms, config)
   .then(
       function (response) {
           // success callback
           console.log(response.data);
           if (response.data[0].UserId != "" && response.data[0].UserId!==undefined) {
               //$cookies.put('MenuInfo', JSON.stringify(response.data.MenuInfo));
               $cookies.put('UserData', JSON.stringify(response.data));
               $cookies.put('Islogin', "true");
               window.location = "index.html";
           }
            else
               alert(response.data[0].Message);
       },
       function (response) {
           // failure callback

       }
    );

    }


$scope.loadcompany();
} ]);
