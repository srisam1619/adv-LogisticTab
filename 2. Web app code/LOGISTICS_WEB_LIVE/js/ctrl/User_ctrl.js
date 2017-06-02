App.controller('User_ctrl', ['$scope', '$rootScope', '$http', '$window', '$cookies','util_SERVICE','$location',

function ($scope, $rootScope, $http, $window, $cookies,US,$location) {
	US.islogin();
    
    $scope.user = angular.fromJson($cookies.get('UserData'));
	$scope.Host = US.Host;
	
$rootScope.newdriver = {"Id":"","DriverId":"","DriverName":"","UserName":"","Password":"","ContactNumber":"","IsActive":"1","TypeofOperation":"1"}
$rootScope.Edriver = {}
 
 
 $scope.GetDriverList = function()
 {
	 //alert(number);
	 US.GetDriverList($scope.user[0].CompanyCode).then(function (response) 
				{
					console.log(response);
					$scope.DriverList = response.data;
				});
	 
 }
 
 
 
 //opup NewDriver window 
 $scope.NewDriver = function()
 {
	 $('#NewDriver').modal('show');
	 $rootScope.getDriverNameList();
 }
 
 //opup editUser window 
 $scope.editUser = function(data)
 {
	 $('#EditDriver').modal('show');
	 $rootScope.Edriver = angular.copy(data);
 }
 
 $rootScope.getDriverNameList = function()
 {
	  US.GetDriverNames($scope.user[0].CompanyCode).then(function (response) 
				{
					$rootScope.PoPDriverList = response.data;
				});
 }
 
 $rootScope.DeleteDriver = function(d)
 {
	d.TypeofOperation='3';
	  US.InsertUpdateDeleteDriver(d,$scope.user[0].CompanyCode).then(function (response) 
				{
					console.log(response);
					//$scope.CallAssignment = response.data;
					if(response.data[0].Result=="SUCCESS")
					{
						alert(response.data[0].DisplayMessage);
						$scope.GetDriverList();
					}
					else
					{
						alert(response.data[0].DisplayMessage);
					}
				});
	  }
	  
	  
 $rootScope.validate = function(d)
 {
	 if(d.DriverName=="")
	 {
		 alert("Kindly Enter Driver Name");
		 return false;
	 }
	 if(d.UserName=="")
	 {
		 alert("Kindly Enter User Name");
		 return false;
	 }
	  if(d.Password=="")
	 {
		 alert("Kindly Enter Password");
		 return false;
	 }
	return true;
	 
 }
 
 $rootScope.UpdateDriver = function()
 {
	  $rootScope.Edriver.TypeofOperation='2';
	  if($rootScope.validate($rootScope.Edriver))
	  {
	  US.InsertUpdateDeleteDriver($rootScope.Edriver,$scope.user[0].CompanyCode).then(function (response) 
				{
					console.log(response);
					//$scope.CallAssignment = response.data;
					if(response.data[0].Result=="SUCCESS")
					{
						alert(response.data[0].DisplayMessage);
						$('#EditDriver').modal('hide');
						$scope.GetDriverList();
					}
					else
					{
						alert(response.data[0].DisplayMessage);
					}
				});
	  }
 }
	  
	  
 $rootScope.SaveDriver = function()
 {
	  if($rootScope.validate($rootScope.newdriver))
	  {
	  US.InsertUpdateDeleteDriver($rootScope.newdriver,$scope.user[0].CompanyCode).then(function (response) 
				{
					console.log(response);
					//$scope.CallAssignment = response.data;
					if(response.data[0].Result=="SUCCESS")
					{
						alert(response.data[0].DisplayMessage);
						$('#NewDriver').modal('hide');
						$scope.GetDriverList();
						$rootScope.newdriver = {"Id":"","DriverId":"","DriverName":"","UserName":"","Password":"","ContactNumber":"","IsActive":"1","TypeofOperation":"1"}
					}
					else
					{
						alert(response.data[0].DisplayMessage);
					}
				});
	  }
}
 
 

 
 
 $scope.GetDriverList();
 
} ]);
