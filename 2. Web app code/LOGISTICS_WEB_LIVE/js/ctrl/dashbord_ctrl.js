App.controller('dashbord_ctrl', ['$scope', '$rootScope', '$http', '$window', '$cookies','util_SERVICE','$location',

function ($scope, $rootScope, $http, $window, $cookies,US,$location) {
	US.islogin();
    
    $scope.user = angular.fromJson($cookies.get('UserData'));
	$scope.Host = US.Host;

	if($scope.user[0].UserName == 'Admin')
	{
		$scope.hideme = false;
	}
	else
	{
		$scope.hideme = true;
	}	

	$scope.sText = {
 "DriverName": "",
 "DocDate": "",
 "DocDueDate": "",
 "DocNum": "",
 "SvcCall": "",
 "CustomerName": "",
 "EquipmentCode": "",
 "EquipmentName": "",
 "AMPM": ""
}

$scope.sfilter = function()
{
	$scope.sText = {
 "DriverName": "",
 "DocDate": "",
 "DocDueDate": "",
 "DocNum": "",
 "SvcCall": "",
 "CustomerName": "",
 "EquipmentCode": "",
 "EquipmentName": "",
 "AMPM": ""
}

 $('#DocDate').val("");
 $('#DocDueDate').val("");


}

 
 $scope.updateDrogOrder = function()
 {
	var myTable = document.getElementById('myTable');
	var rows =  myTable.rows;
	console.log(rows.length);
	var forcount = rows.length-2;
	for(var i=1;i<=forcount;i++)
	{
		var firstRow = rows[i].cells[0].innerHTML;
   		$scope.CallAssignment[firstRow-1].DragOrder=i.toString();
	}
   
        var parms = encodeURIComponent(JSON.stringify($scope.CallAssignment));

   console.log(parms);
    US.UpdateDragOrder($scope.user[0].CompanyCode,encodeURIComponent(JSON.stringify($scope.CallAssignment))).then(function (response) 
				{
					$scope.result = response.data;
					if($scope.result[0].Result=="SUCCESS")
					{
						alert($scope.result[0].DisplayMessage);
						$scope.GetCallAssignment();
					}
					else
					{
						alert($scope.result[0].DisplayMessage);
					}
				});
	
 }
 
 $scope.Search = function()
 {
	 //alert($('#DocDate').val());
	 $scope.sText.DocDate =  $('#DocDate').val();
	 $scope.sText.DocDueDate =  $('#DocDueDate').val();

	 if($("#realtime").is(':checked'))
		   $scope.sText.AMPM = "PM";
		else
		   $scope.sText.AMPM = "AM";

	 US.SearchCallAssignment($scope.user[0].CompanyCode,$scope.user[0].UserName,$scope.sText).then(function (response) 
				{
					console.log(response);
					$scope.CallAssignment = response.data;
				});
	 
	 US.SearchCallAssignment_ItemList($scope.user[0].CompanyCode,$scope.user[0].UserName,$scope.sText).then(function (response) 
				{
					console.log(response);
					$rootScope.CallAssignmentItem = response.data;
					$rootScope.CallAssignmentItemTotal = 0;
					
					for(var i=0;i<$rootScope.CallAssignmentItem.length;i++)
					{
						$rootScope.CallAssignmentItemTotal = $rootScope.CallAssignmentItemTotal +parseInt($rootScope.CallAssignmentItem[i].Quantity);
					}
				});
 }
 
 $scope.GetCallAssignment = function()
 {
	 //alert(number);
	 US.GetCallAssignment($scope.user[0].CompanyCode,$scope.user[0].UserName).then(function (response) 
				{
					console.log(response);
					$scope.CallAssignment = response.data;
				});
	 
	 US.GetCallAssignmentItem($scope.user[0].CompanyCode,$scope.user[0].UserName).then(function (response) 
				{
					console.log(response);
					$rootScope.CallAssignmentItem = response.data;
					$rootScope.CallAssignmentItemTotal = 0;
					
					for(var i=0;i<$rootScope.CallAssignmentItem.length;i++)
					{
						$rootScope.CallAssignmentItemTotal = $rootScope.CallAssignmentItemTotal +parseInt($rootScope.CallAssignmentItem[i].Quantity);
					}
				});
 }
 
 
 
 $scope.GetCallAssignmentItem = function()
 {
	 //alert(number);
	 $('#AssignmentItem').modal('show');
	 
 }
 
  $scope.GetCompletedCall = function()
 {
	 //alert(number);
	 US.GetCompletedCall($scope.user[0].CompanyCode,$scope.user[0].UserName).then(function (response) 
				{
					//console.log(response);
					$scope.GetCompletedCallDATA = response.data;
				});
	 
	 US.GetCompletedCallItems($scope.user[0].CompanyCode,$scope.user[0].UserName).then(function (response) 
				{
					
					//console.log(response);
					$scope.GetCompletedCallItems = response.data;
					$rootScope.GetCompletedCallItemsTotal = 0;
					
					for(var i=0;i<$scope.GetCompletedCallItems.length;i++)
					{
						$rootScope.GetCompletedCallItemsTotal = $rootScope.GetCompletedCallItemsTotal +parseInt($scope.GetCompletedCallItems[i].Quantity);
					}
					
					
				});
 }
 
 $scope.dostatuscheck = function()
 {
 	var myTable = document.getElementById('myTable');
	var rows =  myTable.rows;
	var firstRow = rows[1].cells[0].innerHTML;
   //console.log(firstRow);
   
   
    $scope.callsdata = $scope.CallAssignment[firstRow-1];
	$scope.Itemdata = $scope.getSjsonInput2($scope.callsdata.DocEntry);
 	console.log($scope.callsdata.DeliveryType);
 	if($scope.callsdata.DeliveryType=="Toners")
 		$scope.DOSTATUSTP();
 	else
 		$scope.DOCALL();
 }
 
 $scope.DOCALL = function()
 {
	var myTable = document.getElementById('myTable');
	var rows =  myTable.rows;
	var firstRow = rows[1].cells[0].innerHTML;
   //console.log(firstRow);
   
   
    $scope.callsdata = $scope.CallAssignment[firstRow-1];
	$scope.Itemdata = $scope.getSjsonInput2($scope.callsdata.DocEntry);
	
	$cookies.put('HeaderData', JSON.stringify($scope.callsdata));
	$cookies.put('ItemData', JSON.stringify($scope.Itemdata));
	
	window.location = "dostatus.html";
 }



 $scope.DOSTATUSTP = function()
 {
	var myTable = document.getElementById('myTable');
	var rows =  myTable.rows;
	var firstRow = rows[1].cells[0].innerHTML;
   //console.log(firstRow);
   
   
    $scope.callsdata = $scope.CallAssignment[firstRow-1];
	$scope.Itemdata = $scope.getSjsonInput2($scope.callsdata.DocEntry);
	
	$cookies.put('HeaderData', JSON.stringify($scope.callsdata));
	$cookies.put('ItemData', JSON.stringify($scope.Itemdata));
	
	window.location = "dostatusTonersParts.html";
 }
 
 
 //opup EAT window 
 $scope.EATcal = function()
 {
	 $('#EATModal').modal('show');
	 var myTable = document.getElementById('myTable');
	var rows =  myTable.rows;
	var firstRow = rows[1].cells[0].innerHTML;
   //console.log(firstRow);
   $( "#EATTIME" ).val("");
	$( "#EATDATE" ).val("");
   
    $rootScope.EATD = $scope.CallAssignment[firstRow-1];
 
		 
	
		 
	 var cuttentGPSX = "";
	 var cuttentGPSY = ""; 
	 var GPSSTATUS = true;
	
	if (navigator.geolocation) {
		navigator.geolocation.getCurrentPosition(showPosition);
	} else {
		GPSSTATUS = false;
		$( "#EATTIMEERROR" ).innerHTML("Can't Get your Current Location.");
	}
	
	function showPosition(position) {
		cuttentGPSX = position.coords.latitude;
		cuttentGPSY = position.coords.longitude;
		$scope.getEATFROMGOOGLE(cuttentGPSX,cuttentGPSY);
	}
	
 }
 
 
 
 $scope.getEATFROMGOOGLE = function(x,y)
 {
	 
			   //var origin = "International Business Park, Singapore",
			   var origin = new google.maps.LatLng(x, y);
			   destination =  $rootScope.EATD.CustomerName,
			   service = new google.maps.DistanceMatrixService();
			
				service.getDistanceMatrix(
					{
						origins: [origin],
						destinations: [destination],
						travelMode: google.maps.TravelMode.DRIVING,
						avoidHighways: false,
						avoidTolls: false
					}, 
					callback
				);
				
				function callback(response, status) {
					console.log(response);
				if(status=="OK") 
				{
						if(response.rows[0].elements[0].status!="NOT_FOUND" && response.rows[0].elements[0].status!="ZERO_RESULTS")
						{
								console.log(response);
								var t = new Date();
								t.setSeconds(t.getSeconds() + response.rows[0].elements[0].duration.value);
								
								$( "#EATTIME" ).val(moment(t).format('hh:mm A'));
								$( "#EATDATE" ).val(moment(t).format('DD/MM/YYYY'));
								
						}
					   
				} else {
						$( "#EATTIMEERROR" ).innerHTML("Error: " + status);
					}
				}
				
	 
	

 }
 
 $scope.getSjsonInput2 = function(docentry)
 {
	 $scope.sJsonInput2 = [];
	 console.log(docentry);
	 for(var j=0;j<$rootScope.CallAssignmentItem.length;j++)
					{
						console.log($rootScope.CallAssignmentItem[j].DocEntry);
						if($rootScope.CallAssignmentItem[j].DocEntry==docentry)
						{
							$scope.sJsonInput2.push($rootScope.CallAssignmentItem[j]);
						}
					}
				return $scope.sJsonInput2;
 }
 
 $rootScope.UpdateEAT = function()
 {
	 
	 console.log($rootScope.EATD);
	 
	 var EATVALTIME = $( "#EATTIME" ).val()
	 var EATVALDATE = $( "#EATDATE" ).val()	 

	 if(EATVALTIME=="")
	 {
		 alert("Kindly Enter ETA Time");
	 }
	 else if(EATVALDATE=="")
	 {
		alert("Kindly Enter ETA Date");
	 }
	 else
	 {
	 
	 
	 $scope.sJsonInput2= $scope.getSjsonInput2($rootScope.EATD.DocEntry);
	 
	  US.UpdateEAT($rootScope.EATD,$scope.user[0].CompanyCode,$scope.sJsonInput2,EATVALTIME,EATVALDATE).then(function (response) 
				{
					console.log(response);
					//$scope.CallAssignment = response.data;
					if(response.data[0].Result=="SUCCESS")
					{
						$('#EATModal').modal('hide');
						 var myTable = document.getElementById('myTable');
						var rows =  myTable.rows;
						var firstRow = rows[1].cells[0].innerHTML;
					   //console.log(firstRow);
   
   
    					$scope.CallAssignment[firstRow-1].ETA=EATVALTIME;
						alert(response.data[0].DisplayMessage);
					}
					else
					{
						alert(response.data[0].DisplayMessage);
					}
				});
	  }
 }
 
 $scope.addAttchment = function(id)
 {
	 $rootScope.CurrentId = id;
	 $scope.AttachmentData =  $scope.GetCompletedCallDATA[$rootScope.CurrentId].Attachments;
	 $('#AttchmentModal').modal('show');
 }
 
 //file upload and attavchment 
 
  $scope.Att_file="";
   $scope.Fileloading = true;
   //file upload 
   $scope.uploadFile = function() {
	
   $scope.Fileloading = false;
   var fd = new FormData();
         fd.append('file', $scope.myFile);
         var uploadUrl = US.Host+"FileUpload.php";
         $http.post(uploadUrl, fd, {
             transformRequest: angular.identity,
             headers: {'Content-Type': undefined}
         })
         .success(function(data){
         $scope.Fileloading = true;
            console.log(data);
            $scope.myFile = []
            $("#fileopen").val("");
            $scope.ATT =  {
            "DocEntry": $scope.GetCompletedCallDATA[$rootScope.CurrentId].DocEntry,
            "FilePath": data.FilePath,
            "FileName": data.FileName,
            "FileExtension": data.FileExten,
			"IsNew": "Y"
        };
        if(data.Status=="true")
        {
			
            $scope.GetCompletedCallDATA[$rootScope.CurrentId].Attachments.push(angular.copy($scope.ATT));
            }
            else{
                alert(data.errMSG);
            }
         })
         .error(function(){
            $scope.Fileloading = true;
            alert("File Upload Failed");
         });

};

$scope.saveAttachment = function()
{
		//alert(number);
	 US.SaveAttachments($scope.user[0].CompanyCode,$scope.GetCompletedCallDATA[$rootScope.CurrentId].Attachments).then(function (response) 
				{
					$scope.result = response.data;
					if($scope.result[0].Result=="SUCCESS")
					{
						alert($scope.result[0].DisplayMessage);
						$('#AttchmentModal').modal('hide');
					}
					else
					{
						alert($scope.result[0].DisplayMessage);
					}
					
				});
}


 
 
 $scope.GetCallAssignment();
 $scope.GetCompletedCall();
 
} ]);
