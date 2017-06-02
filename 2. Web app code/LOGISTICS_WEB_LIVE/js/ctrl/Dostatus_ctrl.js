App.controller('Dostatus_ctrl', ['$scope', '$rootScope', '$http', '$window', '$cookies','util_SERVICE','$location',

function ($scope, $rootScope, $http, $window, $cookies,US,$location) {
	US.islogin();
    
    $scope.user = angular.fromJson($cookies.get('UserData'));
	
	$scope.Items = angular.fromJson($cookies.get('ItemData'));
	$scope.Header = angular.fromJson($cookies.get('HeaderData'));
	console.log($scope.Header);
	$scope.AttachmentData = [];
	$scope.AdditionalItems = [];
	$scope.searchText ={"ItemCode":"","ItemName":""};

	
	$scope.disable = true;
	
	if($scope.Header.Remarks === undefined)
		$scope.Header.Remarks = "";
	
	$scope.checkDriver = function()
	{
		if($scope.Header.Driver1==$scope.Header.Driver2)
		{
			alert("Same driver Selected.");
			$scope.Header.Driver2 = "";
		}
	}
	
	$scope.valdata = function()
	{
		var rvalue = true;
	
		if($scope.Header.ArrivalTime===undefined || $scope.Header.ArrivalDate===undefined )
		{
			alert("Kindly Click Arrival Button");
			rvalue = false;
		}
		
		if($scope.Header.Driver1===undefined)
			$scope.Header.Driver1 = "";
		if($scope.Header.Driver2===undefined)
			$scope.Header.Driver2 = "";
	
		
		var itemqty = true;
		 for(var j=0;j<$scope.Items.length;j++)
	 	 {
		 	if($scope.Items[j].DOQty=="" || $scope.Items[j].DOQty=="0")
			{
				itemqty = false;
			}
		 }
		 
		 if(!itemqty)
		 {
			 alert("Kindly Enter Do Qty");
			 rvalue = false;
		 }
		  var DOstatusVal = true;
		 for(var m=0;m<$scope.Items.length;m++)
	 	 {
		 	if($scope.Items[m].DeliveryStatus===undefined || $scope.Items[m].DeliveryStatus=="Closed")
			{
				DOstatusVal = false;
			}
		 }
		 if(!DOstatusVal)
		 {
			 alert("Kindly Select Do Status");
			 rvalue = false;
		 }
		 
		 
		 var Additemqty = true;
		 for(var y=0;y<$scope.AdditionalItems.length;y++)
	 	 {
		 	if($scope.AdditionalItems[y].Qty=="" || $scope.AdditionalItems[y].Qty=="0")
			{
				Additemqty = false;
			}
		 }
		 
		 if(!Additemqty)
		 {
			 alert("Kindly Enter Add Item Do Qty");
			 rvalue = false;
		 }
		
		
		
		return rvalue;
	}
	
	$scope.SaveDoData = function()
	{
		if($scope.Header.RecipientName=="" ||$scope.Header.RecipientName===undefined)
		{
			alert("Kindly Enter Recipient Name");
			return false;
		}
		if($scope.Header.RecipientEmail=="" ||$scope.Header.RecipientEmail===undefined)
		{
			alert("Kindly Enter Recipient Email");
			return false;
		}

		$scope.Header.DoNumber = $scope.Header.DocNum;
		$scope.Header.RecipientSignature = getSJdata();
		if($scope.Header.RecipientSignature.length<=1716)
		{
			alert("Kindly Draw your Signature");
			return false;
		}
		if($("#realtime").is(':checked'))
		   $scope.Header.SuppliesPrinted = "Yes";
		else
		   $scope.Header.SuppliesPrinted = "No";
		
		
		//alert($scope.valdata());
		if($scope.valdata())
		{
			for(var k=0;k<$scope.Items.length;k++)
	 	 {
			 $scope.Items[k].DeliveryStatus = $scope.Items[k].DeliveryStatus.Id;
		 }
		
		US.UpdateDeliveryOrder($scope.Header,$scope.AttachmentData,$scope.Items,$scope.AdditionalItems,$scope.user[0].CompanyCode)
		.then(function (response){
						if(response.data[0].Result=="SUCCESS")
					   {
						   alert(response.data[0].DisplayMessage);
						   window.location ="index.html";
					   }
					   else
					   {
						   alert(response.data[0].DisplayMessage);
					   }
												   });
		}
		
	}

	$scope.DriverList = [];
	US.GetDriverList($scope.user[0].CompanyCode).then(function (response)
     {
		 for(var j=0;j<response.data.length;j++)
	 	 {
			 if(response.data[j].IsActive=="1")
			  $scope.DriverList.push(response.data[j]);
	 	 }
	 
		 
	 });
	US.GetItemList($scope.user[0].CompanyCode).then(function (response){$scope.AIMaster = response.data;});
	US.GetDeliveryStatusList($scope.user[0].CompanyCode).then(function (response){$scope.DeliveryStatusList = response.data;});
	US.getDOCusData($scope.user[0].CompanyCode,$scope.Header.DocNum).then(function (response){
		$scope.cusd = response.data;
		$scope.Header.ContactPerson = $scope.cusd[0].RcpName;
		$scope.Header.Email = $scope.cusd[0].RcpEmail;
	});
	

	$scope.getDoCusData = function()
 {
	 US.getDOCusData($scope.user[0].CompanyCode,$scope.Header.DocNum)
	 .then(function (response)
		{
			$scope.CusData = response.data;
			// $('#CusDataModal').modal('show');
			$scope.Header.RecipientName = $scope.CusData[0].RcpName;
			$scope.Header.RecipientEmail = $scope.CusData[0].RcpEmail;
		});
 }



 $scope.getCusData = function()
 {
	 US.getCusData($scope.user[0].CompanyCode,$scope.Header.CustomerCode)
	 .then(function (response)
		{
			$scope.CusData = response.data;
			 $('#CusDataModal').modal('show');
			//$scope.Header.RecipientName = $scope.CusData[0].RcpName;
			//$scope.Header.RecipientEmail = $scope.CusData[0].RcpEmail;
		});
 }

 $scope.getDoCusData = function()
 {
	 US.getDOCusData($scope.user[0].CompanyCode,$scope.Header.DocNum)
	 .then(function (response)
		{
			$scope.CusData = response.data;
			// $('#CusDataModal').modal('show');
			$scope.Header.RecipientName = $scope.CusData[0].RcpName;
			$scope.Header.RecipientEmail = $scope.CusData[0].RcpEmail;
		});
 }


 $scope.setCusData = function(name,email)
 {
		$scope.Header.RecipientName = name;
	    $scope.Header.RecipientEmail = email;
	    $('#CusDataModal').modal('hide');
 }
 $scope.Arrived = function()
 {
	 var currentdate = new Date(); 
	var month = currentdate.getMonth()+1;
	//$scope.Header.ArrivalDate = currentdate.getDate()+"-"+month+"-"+currentdate.getFullYear();
	//$scope.Header.ArrivalTime = currentdate.getHours()+":"+currentdate.getMinutes()+":"+currentdate.getSeconds();
	
	$scope.Header.ArrivalTime = moment(currentdate).format('hh:mm A');
	$scope.Header.ArrivalDate = moment(currentdate).format('DD/MM/YYYY')
 }

 $scope.delAitem = function(id)
 {
 	$scope.AdditionalItems.splice(id, 1);;
 	
 }
 
 $scope.AIpopup = function()
 {
	 //alert(number);
	 $('#AIpopup').modal('show');
	 $scope.searchText.ItemCode ="";
	 $scope.searchText.ItemName ="";

	 $scope.DItemCode ="";
	 $scope.DItemName ="";

	 
 }
 
  $scope.addAttchment = function(id)
 {
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
            "DocEntry": $scope.Header.DocEntry,
			"DocNum": $scope.Header.DocNum,
            "FilePath": data.FilePath,
            "FileName": data.FileName,
            "FileExtension": data.FileExten,
			"IsNew": "Y"
        };
        if(data.Status=="true")
        {
			
            $scope.AttachmentData.push(angular.copy($scope.ATT));
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




 $scope.setAIteam = function(ItemCode,ItemName)
 {
 	if(ItemCode=="" || ItemName=="")
 	{
 		alert("Item Code or name should not be empty!");
 	}
 	else
 	{
	 var d = {
			  "DocEntry": $scope.Header.DocEntry,
			  "DocNum": $scope.Header.DocNum,
			  "ItemCode": ItemCode,
			  "ItemDescription": ItemName,
			  "Qty": "1",
			  "ItemCondition": "Used",
			 }
 	
		$scope.AdditionalItems.push(d);
		$('#AIpopup').modal('hide');
	}
	
 }
 
  $scope.GetCompletedCall = function()
 {
	 //alert(number);
	 US.GetCompletedCall($scope.user[0].CompanyCode).then(function (response) 
				{
					console.log("ssss");
					console.log(response);
					$scope.GetCompletedCallDATA = response.data;
				});
 }
 
 
 $scope.applytoall = function(status)
 {
	 //alert(status);
	 for(var j=0;j<$scope.Items.length;j++)
	 {
		 $scope.Items[j].DeliveryStatus = status;
	 }
	 console.log($scope.Items);
	 
 }
 
 
} ]);
